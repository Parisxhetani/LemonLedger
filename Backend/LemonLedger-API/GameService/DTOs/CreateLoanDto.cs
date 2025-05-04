namespace GameService.DTOs
{
    public class CreateLoanDto
    {
        //public string PlayerId { get; set; }
        public decimal Principal { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public int TermMonths { get; set; }
    }
}
