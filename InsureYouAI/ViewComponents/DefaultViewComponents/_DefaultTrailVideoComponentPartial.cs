using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DefaultViewComponents
{
    public class _DefaultTrailVideoComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultTrailVideoComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values=_context.TrailerVideos.ToList();
            return View(values);
        }
    }
}
