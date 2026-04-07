using Microsoft.AspNetCore.Identity;

namespace InsureYouAI.Entities
{
    public class AppUser:IdentityUser
    {
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? ImageURl { get; set; }

        public string? Description { get; set; }

        // Bir kullanıcının TÜM yorumlarını tutan koleksiyon
        // "1" tarafını temsil eder
        public List<Comment>? Comments { get; set; } 

        public List<Article>? Articles { get; set; }

        public List<Policy>? Policies { get; set; }

        public string? Education { get; set; }

        public string? Title { get; set; }

        public string? City { get; set; }
    }
}
