namespace InsureYouAI.Entities
{
    public class PricingPlan
    {
        public int PricingPlanId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public bool IsFeature { get; set; }

        public List<PricingPlanItem> PricingPlanItem { get; set; }




    }
}
