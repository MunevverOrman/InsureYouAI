namespace InsureYouAI.Entities
{
    public class PricingPlanItem
    {

        public int PricingPlanItemId { get; set; }

        public string Title { get; set; }

        public int PricingPlanId { get; set; }

        public PricingPlan PricingPlan { get; set; }
    }
}
