using InsureYouAI.Entities;

namespace InsureYouAI.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }

        public string CommentDetail { get; set; }

        public DateTime CommentDate{ get; set; }

        // Yorumun hangi kullanıcıya ait olduğunu tutar
        // "Many" tarafını temsil eder
        public AppUser AppUser { get; set; }      // Navigation Property
        public string AppUserId { get; set; }     // Foreign Key (Yabancı Anahtar)

        // Yorumun hangi makaleye ait olduğunu tutar
        // "Many" tarafını temsil eder
        public int ArticleId { get; set; }      // Foreign Key
        public Article Article { get; set; }    // Navigation Property

        public string? CommentStatus { get; set; }
                                              


    }
}





//Bir kullanıcı → Birden fazla yorum yazabilir
//Bir yorum    → Sadece bir kullanıcıya ait olabilir
//AppUser, IdentityUser'dan miras aldığı için Identity'nin varsayılan Primary Key tipi string (GUID) formatındadır. Bu yüzden foreign key de string olarak tanımlanmış.
//Bir makale  → Birden fazla yorum alabilir
//Bir yorum   → Sadece bir makaleye ait olabilir
//Comment entity'si aslında bir **köprü/junction** görevi görüyor:

//AppUser  ──────┐
//               │
//               ▼
//            Comment  ──────►  Article
               
//Bir yorum hem bir kullanıcıya
//hem de bir makaleye aittir