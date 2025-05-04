using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameService.Models
{
    public class Player
    {
        [Key]
        public string PlayerId { get; set; }      // PK

        public decimal CurrentBalance { get; set; }     // e.g. 123.45
        public int CreditScore { get; set; }     // XP points

        // navigation properties (optional, but useful)
        public ICollection<Loan> Loans { get; set; }
        public ICollection<SavingsAccount> SavingsAccounts { get; set; }
    }
}
