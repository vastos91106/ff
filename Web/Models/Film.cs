using System;
using System.ComponentModel.DataAnnotations;
using Web.Common;

namespace Web.Models
{
    public sealed class Film
    {
        public Guid IdGuid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [FileExtensions(Extensions = "jpg,jpeg,png")]
        public string ImagePath { get; set; }

        [Required]
        [YearValidation]
        public int Year { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string FilmDirector { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
