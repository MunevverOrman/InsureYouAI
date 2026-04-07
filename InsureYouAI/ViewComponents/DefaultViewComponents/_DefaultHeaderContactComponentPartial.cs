using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DefaultViewComponents
{
    public class _DefaultHeaderContactComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultHeaderContactComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        { 
            ViewBag.email=_context.Contacts.Select(x => x.Email).FirstOrDefault();
            ViewBag.phone=_context.Contacts.Select(x=>x.Phone).FirstOrDefault();
            return View(); 
        }
    }
}
