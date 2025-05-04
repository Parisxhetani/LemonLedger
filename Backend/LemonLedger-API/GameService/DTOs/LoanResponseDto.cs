namespace GameService.DTOs
{
    public class LoanResponseDto
    {
        public int LoanId { get; set; }
        public decimal Principal { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public int TermMonths { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal RemainingBalance { get; set; }
        public int PaymentsMade { get; set; }
        public DateTime NextDueDate { get; set; }
        public string Status { get; set; }
    }
}
