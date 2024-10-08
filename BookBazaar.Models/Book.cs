﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookBazaar.Models
{
    public class Book
    {
        [Key]
        public int bookId { get; set; }
        public string title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        //[Range(1, 10000)]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }
        [Required]
        //[Range(1, 10000)]
        [Display(Name = "Price for 1-50")]
        public double Price { get; set; }

        [Required]
        //[Range(1, 10000)]
        [Display(Name = "Price for 51-100")]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        //[Range(1, 10000)]
        public double Price100 { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        //[Required]
        //[Display(Name = "Cover Type")]
        //public int CoverTypeId { get; set; }
        //[ValidateNever]
        //public CoverType CoverType { get; set; }
    }
}
