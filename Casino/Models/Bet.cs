namespace Casino.Models
{
    public class Bet
    {

        public int Id { get; set; }

        public string? Color {  get; set; }

        public int? number {  get; set; }

        public string? EvenOdd {  get; set; }

        public int Money { get; set; }

        public DateTime? CreatedAt{ get; set; }

        public int PlayerId { get; set; }



    }
}
