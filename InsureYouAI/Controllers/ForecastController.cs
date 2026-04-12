using InsureYouAI.Context;
using InsureYouAI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class ForecastController : Controller
    {
        private readonly InsureContext _context;
        private readonly ForecastService _forecastService;

        public ForecastController(InsureContext context)
        {  
            _context = context;
            _forecastService = new ForecastService();
        }

        public IActionResult Index()
        {
            var salesData = _context.Policies
                .GroupBy(p => new { p.StartDate.Year, p.StartDate.Month })
                .Select(g => new 
                {
                    Year=g.Key.Year,
                    Month= g.Key.Month,
                    Count= g.Count()
                }).AsEnumerable()
                .Select(g => new PolicySalesData
                {
                    Date = new DateTime(g.Year, g.Month, 1),
                    SalesCount = g.Count
                }).OrderBy(x=>x.Date).ToList();

            var forecast=_forecastService.GetForecast(salesData,horizon:3);
            ViewBag.Forecast = forecast;

            return View(salesData);
        }
    }
}
