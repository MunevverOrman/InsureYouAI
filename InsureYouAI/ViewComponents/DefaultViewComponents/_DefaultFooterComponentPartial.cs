using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DefaultViewComponents
{
    public class _DefaultFooterComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;
        public _DefaultFooterComponentPartial(InsureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.description = _context.Contacts.Select(x => x.Description).FirstOrDefault();
            ViewBag.phone = _context.Contacts.Select(x => x.Phone).FirstOrDefault();
            ViewBag.email = _context.Contacts.Select(x => x.Email).FirstOrDefault();
            ViewBag.address = _context.Contacts.Select(x => x.Adress).FirstOrDefault();
            return View();
        }
    }
}
