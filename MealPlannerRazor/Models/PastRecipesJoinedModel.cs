using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlannerRazor.Models
{
    public class PastRecipesJoinedModel
    {
        public PastMealsModel Pastmeal { get; set; }
        public RecipeModel Recipe { get; set; }
    }
}
