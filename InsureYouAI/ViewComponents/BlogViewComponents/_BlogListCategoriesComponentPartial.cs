using InsureYouAI.Context;
using InsureYouAI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.BlogViewComponents
{
    public class _BlogListCategoriesComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogListCategoriesComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //var values=_context.Categories.ToList();
            var categories=_context.Categories
                .Select(c=>new CategoryArticleCountViewModel
                {
                    CategoryId=c.CategoryId,
                    CategoryName=c.CategoryName,
                    ArticleCount=c.Articles.Count()// İlişkili makaleleri sayar
                }).ToList();
            return View(categories);
        }
    }
}
