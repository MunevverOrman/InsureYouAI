namespace InsureYouAI.Entities
{
    public class Revenue
    {
        public int RevenueId { get; set; }

        public string NameSurname { get; set; }

        public decimal Amount { get; set; }

        public DateTime ProcessDate { get; set; }

        public string Detail { get; set; }
    }
}
