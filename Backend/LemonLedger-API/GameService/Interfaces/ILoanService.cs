using System.Collections.Generic;
using System.Threading.Tasks;
using GameService.DTOs;

namespace GameService.Interfaces
{
    public interface ILoanService
    {
        /// <summary>
        /// Applies a new loan for the given player, updates their balance,
        /// and returns the created loan.
        /// </summary>
        Task<LoanResponseDto> ApplyLoanAsync(string playerId, LoanRequestDto dto);

        /// <summary>
        /// Retrieves a single loan by its ID.
        /// </summary>
        Task<LoanResponseDto> GetLoanByIdAsync(int loanId);

        /// <summary>
        /// Lists all loans for a specific player.
        /// </summary>
        Task<IEnumerable<LoanResponseDto>> GetLoansForPlayerAsync(string playerId);

        /// <summary>
    /// Applies one monthly payment on the loan, adjusts balances/term/credit score,
    /// and returns the updated loan (or null if it’s paid off).
    /// </summary>
    Task<LoanResponseDto?> PayLoanAsync(string playerId, int loanId);
    }
}
