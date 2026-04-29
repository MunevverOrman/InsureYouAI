using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.BlogDetailViewComponents
{
    public class _BlogDetailNextAndPrevPostComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogDetailNextAndPrevPostComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int id)
        {
           var article= _context.Articles.FirstOrDefault(x => x.ArticleId == id);

            //Önceki makaleyi bul
            var prevArticle= _context.Articles
                .Where(x => x.ArticleId < id)
                .OrderByDescending(x => x.ArticleId)
                .Select(x =>  x.Title )
                .FirstOrDefault();

            //Sonraki makaleyi bul
            var nextArticle= _context.Articles
                .Where(x => x.ArticleId > id)
                .OrderBy(x => x.ArticleId)
                .Select(x =>  x.Title )
                .FirstOrDefault();

             ViewBag.PrevArticleTitle = prevArticle;
             ViewBag.NextArticleTitle = nextArticle;

            return View();
        }
    }
}
