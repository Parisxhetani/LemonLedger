namespace GameService.DTOs
{
    public class LoanRequestDto
    {
        public decimal Principal { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public int TermMonths { get; set; }
    }

}
