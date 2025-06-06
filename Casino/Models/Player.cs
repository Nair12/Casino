using System.ComponentModel.DataAnnotations;

namespace Casino.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        public string NickName {  get; set; }

        public string Password {  get; set; }

        public string Avatar { get; set; }

        public int Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastBetAt {  get; set; }

        public bool IsActive {  get; set; }

        public int totalBet { get; set; }
        public int totalWins {  get; set; }




    }
}
