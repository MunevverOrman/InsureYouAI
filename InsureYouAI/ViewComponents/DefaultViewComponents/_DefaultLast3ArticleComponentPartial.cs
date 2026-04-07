using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.ViewComponents.DefaultViewComponents
{
    public class _DefaultLast3ArticleComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultLast3ArticleComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values=_context.Articles.OrderByDescending(x=>x.ArticleId).Include(z=>z.AppUser).Include(y=>y.Category).Take(3).ToList();//Son 3 kayıt.
            return View(values);
        }
    }
}
