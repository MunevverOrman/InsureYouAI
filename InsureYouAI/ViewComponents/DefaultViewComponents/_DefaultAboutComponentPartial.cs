using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DefaultViewComponents
{
    public class _DefaultAboutComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultAboutComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {  
            ViewBag.title=_context.Abouts.Select(x => x.Title).FirstOrDefault();   
            ViewBag.description=_context.Abouts.Select(x=>x.Description).FirstOrDefault();
            ViewBag.imageUrl=_context.Abouts.Select(x=>x.ImageUrl).FirstOrDefault();

            var aboutItemValues=_context.AboutItems.ToList();

            return View(aboutItemValues); 
        }
    }
}
