namespace InsureYouAI.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public List<Article> Articles { get; set; }
   
    }
}
