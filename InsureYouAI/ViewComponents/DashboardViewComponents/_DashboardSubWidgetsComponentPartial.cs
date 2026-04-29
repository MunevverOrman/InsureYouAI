using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DashboardViewComponents
{
    public class _DashboardSubWidgetsComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardSubWidgetsComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.TotalCategoryCount = _context.Categories.Count();
            ViewBag.TotalArticleCount = _context.Articles.Count();
            ViewBag.TotalPoliciesCount = _context.Policies.Count();

            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            ViewBag.TotalPoliciesByThisMonthCount = _context.Policies.Where(x => x.CreatedDate >= startOfMonth && x.CreatedDate < startOfNextMonth).Count();

            ViewBag.TotalCommentCount = _context.Comments.Count();
            ViewBag.TotalUserCount = _context.Users.Count();
            ViewBag.AvgPolicyAmount = _context.Policies.Average(x => x.PremiumAmount);
            ViewBag.LastRevenueAmount = _context.Revenues.OrderByDescending(x => x.RevenueId).Take(1).Select(y => y.Amount).FirstOrDefault();
            return View();
        }
    }
}
