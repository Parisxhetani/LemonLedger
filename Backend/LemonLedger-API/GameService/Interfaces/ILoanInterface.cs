using GameService.DTOs;

namespace GameService.Interfaces
{
    public interface ILoanInterface
    {
        Task<IEnumerable<LoanDto>> GetAllAsync(string playerId);
        Task<LoanDto?> GetByIdAsync(string playerId, int loanId);
        Task<LoanDto> CreateAsync(string playerId, CreateLoanDto dto);
        Task<bool> UpdateAsync(string playerId, int loanId, CreateLoanDto dto);
        Task<bool> DeleteAsync(string playerId, int loanId);
    }
}
