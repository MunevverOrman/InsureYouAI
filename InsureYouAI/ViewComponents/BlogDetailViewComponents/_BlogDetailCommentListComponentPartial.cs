using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.ViewComponents.BlogDetailViewComponents
{
    public class _BlogDetailCommentListComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogDetailCommentListComponentPartial(InsureContext context)
        {
            _context = context;
        }
        //Belirli bir makaleye ait, onaylanmış yorumları veritabanından çekip View'e gönderiyor.
        public IViewComponentResult Invoke(int id)
        {
            var values=_context.Comments.Where(x=>x.ArticleId==id && x.CommentStatus== "Yorum Onaylandı").Include(y=>y.AppUser).ToList();
            return View(values);
        }
    }
}
//Makale ID gelir
//    ↓
//O makaleye ait onaylı yorumlar filtrelenir
//    ↓
//Yorum yazan kullanıcı bilgileri de dahil edilir
//    ↓
//Liste halinde View'e gönderilir