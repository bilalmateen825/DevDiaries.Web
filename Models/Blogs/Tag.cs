namespace DevDiaries.Web.Models.Blogs
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid BlogId { get; set; }
    }
}
