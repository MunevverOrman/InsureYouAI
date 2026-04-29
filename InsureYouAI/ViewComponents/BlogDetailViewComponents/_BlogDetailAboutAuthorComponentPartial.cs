using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.BlogDetailViewComponents
{
    public class _BlogDetailAboutAuthorComponentPartial : ViewComponent
    {

        private readonly InsureContext _context;
        public _BlogDetailAboutAuthorComponentPartial(InsureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int id)
        {
            string appUserId = _context.Articles.Where(x => x.ArticleId == id).Select(y => y.AppUserId).FirstOrDefault();
            var userValue = _context.Users.Where(x => x.Id == appUserId).FirstOrDefault();
            return View(userValue);
        }
    }
}
