﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevDiaries.Web.Models.ViewModels
{
    public class EditBlogPostRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Heading { get; set; }

        [Required]
        public string PageTitle { get; set; }

        [Required]
        public string Content { get; set; }
        [Required]

        public string ShortDescription { get; set; }

        [Required]
        public string? FeaturedImageURL { get; set; }

        [Required]
        public string UrlHandle { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }

        [Required]
        public string Author { get; set; }
        public bool Visible { get; set; }
    }
}
