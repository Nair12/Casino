namespace Casino.Models
{
    public class PlayerArchive
    {
        public int Id {  get; set; }

        public int PlayerId { get; set; }

        public string BetInfo {  get; set; }

        public DateTime CreatedTime { get; set; }

        public int Money {  get; set; }

        public int Result {  get; set; }


    }
}
