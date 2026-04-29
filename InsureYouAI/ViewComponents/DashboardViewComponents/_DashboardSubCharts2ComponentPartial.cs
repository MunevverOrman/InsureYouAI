using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.ViewComponents.DashboardViewComponents
{
    public class _DashboardSubCharts2ComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;
        public _DashboardSubCharts2ComponentPartial(InsureContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentYear = DateTime.Now.Year;

            var monthlyData = await _context.Policies
                .Where(p => p.StartDate.Year == currentYear)
                .GroupBy(p => p.StartDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalPremium = g.Sum(x => x.PremiumAmount)
                })
                .ToListAsync();

            // 12 aylık dizi (boş ayları 0 olarak gösterecek)
            decimal[] revenues = new decimal[12];
            foreach (var item in monthlyData)
            {
                revenues[item.Month - 1] = item.TotalPremium;
            }

            ViewBag.MonthlyRevenues = revenues;

            return View();
        }
    }
}
