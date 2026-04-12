using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DashboardViewComponents
{
    public class _DashboardRadialChartComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardRadialChartComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.v1=_context.Policies.Count();
            ViewBag.r1=_context.Policies.Where(p=>p.Status=="Active").Count();
            return View();
        }
    }
}
