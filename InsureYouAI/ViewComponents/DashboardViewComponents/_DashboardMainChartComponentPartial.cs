using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DashboardViewComponents
{
    public class _DashboardMainChartComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardMainChartComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //"Gelirleri al → aylara göre grupla → her ayın toplam parasını hesapla"
            //Revenues tablosundaki verileri aylara göre gruplandırarak toplam gelirleri hesaplıyoruz
            var revenuedata = _context.Revenues
                .GroupBy(r => r.ProcessDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(r => r.Amount)
                })
            .OrderBy(x => x.Month)
                .ToList();
            //Expenses tablosundaki verileri aylara göre gruplandırarak toplam giderleri hesaplıyoruz
            var expenseData = _context.Expenses
                .GroupBy(e => e.ProcessDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.Month)
                .ToList();

            //all Months
            var allMonths=revenuedata.Select(x=>x.Month)
                .Union(expenseData.Select(x=>x.Month))
                .OrderBy(x=>x)
                .ToList();

            var model = new Models.RevenueExpenseChartViewModel
            {
                Months = allMonths.Select(x =>new System.Globalization.DateTimeFormatInfo().GetAbbreviatedMonthName(x)).ToList(),
                RevenueTotals=allMonths.Select(m => revenuedata.FirstOrDefault(r => r.Month == m)?.TotalAmount ?? 0).ToList(),
                ExpenseTotals=allMonths.Select(m => expenseData.FirstOrDefault(e => e.Month == m)?.TotalAmount ?? 0).ToList()
            };

            ViewBag.v1=_context.Revenues.Sum(x=>x.Amount);
            ViewBag.v2=_context.Expenses.Sum(x=>x.Amount);

            return View(model);
        }
    }
}
