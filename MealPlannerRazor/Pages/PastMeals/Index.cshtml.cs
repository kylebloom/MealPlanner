using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealPlannerRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace MealPlannerRazor.Pages.PastMeals
{
    public class IndexModel : PageModel
    {
        private readonly MealPlannerRazor.Data.MealPlannerRazorContext _context;

        public IndexModel(MealPlannerRazor.Data.MealPlannerRazorContext context)
        {
            _context = context;
        }

        public IList<PastMealsModel> PastRecipesModel { get; set; }
        public IList<PastRecipesJoinedModel> PastRecipesModel2 { get; set; }
        public async Task OnGetAsync()
        {
            var query = from recipes in _context.RecipeModel
                                join pastmeals in _context.PastMealsModel on recipes.Id equals pastmeals.RecipeId into oldmeals
                                from o in oldmeals
                                orderby o.Date descending
                                select new PastRecipesJoinedModel { Pastmeal = o, Recipe = recipes };
            PastRecipesModel2 = await query
                                .ToListAsync();           

            /*PastRecipesModel = await _context.PastMealsModel
                .OrderBy(p => p.Date)
                .ToListAsync();
            */
        }
    }
}
