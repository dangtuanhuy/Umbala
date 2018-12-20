using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hachi.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name can not null")]
        [Display(Name = "Category Name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Display Order can not null")]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
    }
}
