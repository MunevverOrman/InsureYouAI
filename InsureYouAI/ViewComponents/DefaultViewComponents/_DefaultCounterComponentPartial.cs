using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DefaultViewComponents
{
    public class _DefaultCounterComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultCounterComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.categoryCount = _context.Categories.Count();
            ViewBag.serviceCount = _context.Services.Count();
            ViewBag.userCount = _context.Users.Count();
            ViewBag.articleCount = _context.Articles.Count();

            return View();
        }
    }
}
