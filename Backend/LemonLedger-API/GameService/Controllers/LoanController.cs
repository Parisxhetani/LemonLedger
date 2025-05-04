using System.Threading.Tasks;
using GameService.DTOs;
using GameService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GameService.Services;

namespace GameService.Controllers
{
    [ApiController]
    [Route("api/players/{playerId}/loans")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        /// <summary>
        /// Applies for a new loan and credits the player's balance.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ApplyLoan(
            [FromRoute] string playerId,
            [FromBody] LoanRequestDto dto)
        {
            var createdLoan = await _loanService.ApplyLoanAsync(playerId, dto);
            return CreatedAtAction(
                nameof(GetLoanById),
                new { playerId, loanId = createdLoan.LoanId },
                createdLoan);
        }

        /// <summary>
        /// Retrieves details of a specific loan.
        /// </summary>
        [HttpGet("{loanId}")]
        public async Task<IActionResult> GetLoanById(
            [FromRoute] string playerId,
            [FromRoute] int loanId)
        {
            var loan = await _loanService.GetLoanByIdAsync(loanId);
            if (loan == null)
                return NotFound();
            return Ok(loan);
        }

        /// <summary>
        /// Lists all loans for the specified player.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLoansForPlayer(
            [FromRoute] string playerId)
        {
            var loans = await _loanService.GetLoansForPlayerAsync(playerId);
            return Ok(loans);
        }

        [HttpPost("{loanId}/payment")]
        public async Task<IActionResult> PayLoan(
    [FromRoute] string playerId,
    [FromRoute] int loanId)
        {
            var loanDto = await _loanService.PayLoanAsync(playerId, loanId);
            if (loanDto is null)
                return NoContent();        // loan is fully paid off and deleted

            return Ok(loanDto);           // return updated loan details
        }
    }
}
