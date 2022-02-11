using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlannerRazor.Models
{
    public class IngredientModel
    {
        public int Id { get; set; }

        // foreign key to RecipeModel
        public int RecipeId { get; set; }

        [Display(Name = "Ingredient")]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        public double Quantity { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string MeasureType { get; set; }
    }
}
