using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.ViewComponents.BlogDetailViewComponents
{
    public class _BlogDetailContentComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogDetailContentComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int id)    
        {
            var values=_context.Articles.Where(x=>x.ArticleId==id).Include(x=>x.AppUser).Include(y=>y.Category).FirstOrDefault();
            ViewBag.CommentCount=_context.Comments.Where(x=>x.ArticleId==id).Count();
            return View(values);
        }
    }
}
