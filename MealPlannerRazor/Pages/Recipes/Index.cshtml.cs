using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MealPlannerRazor.Data;
using MealPlannerRazor.Models;

namespace MealPlannerRazor.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly MealPlannerRazor.Data.MealPlannerRazorContext _context;

        public IndexModel(MealPlannerRazor.Data.MealPlannerRazorContext context)
        {
            _context = context;
        }

        public IList<RecipeModel> RecipeModel { get;set; }

        public async Task OnGetAsync()
        {
            RecipeModel = await _context.RecipeModel.ToListAsync();
        }
    }
}
