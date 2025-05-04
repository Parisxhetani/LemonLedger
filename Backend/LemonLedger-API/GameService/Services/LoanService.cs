using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameService.Data;
using GameService.DTOs;
using GameService.Interfaces;
using GameService.Models;
using Microsoft.EntityFrameworkCore;

namespace GameService.Services
{
    public class LoanService : ILoanService
    {
        private readonly GameDbContext _db;

        public LoanService(GameDbContext db)
        {
            _db = db;
        }

        public async Task<LoanResponseDto> ApplyLoanAsync(string playerId, LoanRequestDto dto)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();

            // 1) Load player
            var player = await _db.Players.FindAsync(playerId);
            if (player == null)
                throw new KeyNotFoundException($"Player '{playerId}' not found.");

            // 2) Compute monthly payment
            var monthlyRate = dto.AnnualInterestRate / 100m / 12m;
            var monthlyPayment = dto.Principal * monthlyRate
                / (1m - (decimal)Math.Pow(1 + (double)monthlyRate, -dto.TermMonths));

            // 3) Create loan record
            var loan = new Loan
            {
                PlayerId = playerId,
                Principal = dto.Principal,
                AnnualInterestRate = dto.AnnualInterestRate,
                TermMonths = dto.TermMonths,
                MonthlyPayment = Math.Round(monthlyPayment, 2),
                RemainingBalance = dto.Principal,
                PaymentsMade = 0,
                NextDueDate = DateTime.UtcNow.AddMonths(1),
                Status = "active"
            };
            _db.Loans.Add(loan);

            // 4) Update player's balance
            player.CurrentBalance += dto.Principal;
            _db.Players.Update(player);

            // 5) Save and commit transaction
            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            return MapToDto(loan);
        }

        public async Task<LoanResponseDto> GetLoanByIdAsync(int loanId)
        {
            var loan = await _db.Loans.FindAsync(loanId);
            if (loan == null)
                throw new KeyNotFoundException($"Loan '{loanId}' not found.");

            return MapToDto(loan);
        }

        public async Task<IEnumerable<LoanResponseDto>> GetLoansForPlayerAsync(string playerId)
        {
            var loans = await _db.Loans
                                 .Where(l => l.PlayerId == playerId)
                                 .ToListAsync();

            return loans.Select(MapToDto);
        }

        private LoanResponseDto MapToDto(Loan loan)
            => new LoanResponseDto
            {
                LoanId = loan.LoanId,
                Principal = loan.Principal,
                AnnualInterestRate = loan.AnnualInterestRate,
                TermMonths = loan.TermMonths,
                MonthlyPayment = loan.MonthlyPayment,
                RemainingBalance = loan.RemainingBalance,
                PaymentsMade = loan.PaymentsMade,
                NextDueDate = loan.NextDueDate,
                Status = loan.Status
            };

        public async Task<LoanResponseDto?> PayLoanAsync(string playerId, int loanId)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();

            // 1) load player & loan
            var player = await _db.Players.FindAsync(playerId)
                         ?? throw new KeyNotFoundException($"Player '{playerId}' not found.");
            var loan = await _db.Loans.FindAsync(loanId)
                         ?? throw new KeyNotFoundException($"Loan '{loanId}' not found.");

            // 2) ensure you have enough balance
            if (player.CurrentBalance < loan.MonthlyPayment)
                throw new InvalidOperationException("Insufficient funds for payment.");

            // 3) apply payment
            player.CurrentBalance -= loan.MonthlyPayment;
            player.CreditScore += 1;                      // or whatever point scheme
            _db.Players.Update(player);

            loan.TermMonths -= 1;
            loan.PaymentsMade += 1;
            loan.NextDueDate = loan.NextDueDate.AddMonths(1);

            if (loan.TermMonths <= 0)
            {
                _db.Loans.Remove(loan);
            }
            else
            {
                _db.Loans.Update(loan);
            }

            // 4) commit and return
            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            return loan.TermMonths <= 0
                ? null
                : MapToDto(loan);
        }
    }
}
