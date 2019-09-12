using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookish.Web.Models
{
    public class AddBookViewModel
    {
        [Required]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; }
        [Required]
        [Display(Name = "Number of copies")]
        public int NumberOfCopies { get; set; }
    }
}