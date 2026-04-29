using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.ViewComponents.DashboardViewComponents
{
    public class _DashboardSubCharts3ComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;
        public _DashboardSubCharts3ComponentPartial(InsureContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // Sadece içinde bulunulan ayın giderlerini al
            var expenseData = await _context.Expenses
                .Where(e => e.ProcessDate.Month == currentMonth && e.ProcessDate.Year == currentYear)
                .GroupBy(e => e.Detail)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            ViewBag.ExpenseLabels = expenseData.Select(x => x.Category).ToList();
            ViewBag.ExpenseValues = expenseData.Select(x => x.TotalAmount).ToList();

            return View();
        }
    }
}
