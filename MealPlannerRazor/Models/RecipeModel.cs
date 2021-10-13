using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlannerRazor.Models
{
    public class RecipeModel
    {

        public int Id { get; set; }
        [Column(TypeName ="varchar(50)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Meat { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Type { get; set; }

        [Display(Name = "Recipe Directions")]
        [Column(TypeName = "varchar(1000)")]
        public string RecipeDirections { get; set; }
    }
}
