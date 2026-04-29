using InsureYouAI.Context;
using InsureYouAI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.ViewComponents.BlogViewComponents
{
    public class _BlogListAllBlogsComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogListAllBlogsComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //var values = _context.Articles.Include(x => x.Category).Include(y=>y.AppUser).ToList();
            var articles = _context.Articles
                .Include(x => x.Category)
                .Include(y => y.AppUser)
                .Include(z => z.Comments)
                .Select(a => new ArticleListViewModel
                {
                    ArticleId = a.ArticleId,
                    Author=a.AppUser.Name+" "+a.AppUser.Surname,
                    CategoryName = a.Category.CategoryName,
                    CreatedDate = a.CreatedDate,
                    Content = a.Content,
                    ImageUrl = a.CoverImageUrl,
                    Title = a.Title,
                    CommentCount = a.Comments.Count()
                }).ToList();
            return View(articles);
        }
    }
}
