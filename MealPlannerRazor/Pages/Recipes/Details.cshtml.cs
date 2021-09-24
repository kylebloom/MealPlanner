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
    public class DetailsModel : PageModel
    {
        private readonly MealPlannerRazor.Data.MealPlannerRazorContext _context;

        public DetailsModel(MealPlannerRazor.Data.MealPlannerRazorContext context)
        {
            _context = context;
        }

        public RecipeModel RecipeModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RecipeModel = await _context.RecipeModel.FirstOrDefaultAsync(m => m.Id == id);

            if (RecipeModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
