using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
            Bets = new HashSet<Bet>();
        }
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MaxUsernameLength)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.MaxPasswordLength)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.MaxEmailLength)]
        public string Email { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
