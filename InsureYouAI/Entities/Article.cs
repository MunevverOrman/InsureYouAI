namespace InsureYouAI.Entities
{
    public class Article
    {
        public int ArticleId { get; set; }

        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Content { get; set; }

        public string CoverImageUrl { get; set; }

        public string MainCoverImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string? AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Bir makalenin TÜM yorumlarını tutan koleksiyon
        // "1" tarafını temsil eder
        public List<Comment> Comments { get; set; }
    }
}
