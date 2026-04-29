namespace InsureYouAI.Models
{
    public class UserPolicySummaryViewModel
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public string ImageUrl { get; set; }

        public int PolicyCount { get; set; }

        public decimal TotalPremium { get; set; }
    }
}
