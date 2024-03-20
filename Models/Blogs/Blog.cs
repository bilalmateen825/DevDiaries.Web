﻿namespace DevDiaries.Web.Models.Blogs
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string? FeaturedImageURL { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        public List<Tag> Tags { get; set; }

        public ICollection<BlogPostLike> Likes { get; set; }
    }
}
