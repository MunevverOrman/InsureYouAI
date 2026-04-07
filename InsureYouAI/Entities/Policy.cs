namespace InsureYouAI.Entities
{
    public class Policy
    {
        public int PolicyId { get; set; }

        public string PolicyNumber { get; set; } = null!;

        public string PolicyType { get; set; } = null!;

        public string AppUserId { get; set; }

        public decimal PremiumAmount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Active";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        // Navigation Property
        public AppUser AppUser { get; set; } = null!;
    }
}
