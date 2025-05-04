using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameService.Models
{
    public class SavingsAccount
    {
        [Key]
        public int AccountId { get; set; }      // PK

        [Required]
        public string PlayerId { get; set; }      // FK → Player.PlayerId

        public decimal Balance { get; set; }      // principal deposited
        public DateTime DrawDate { get; set; }      // earliest-with-interest

        public decimal InterestRate { get; set; }      // e.g. 5.00 means 5%

        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }
    }
}
