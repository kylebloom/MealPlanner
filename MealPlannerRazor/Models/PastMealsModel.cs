using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlannerRazor.Models
{
    public class PastMealsModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int RecipeId { get; set; }
    }
}
