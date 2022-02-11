using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MealPlannerRazor.Models;

namespace MealPlannerRazor.Pages.Recipes
{
    public class DeleteIngredientModel : PageModel
    {
        

        private readonly MealPlannerRazor.Data.MealPlannerRazorContext _context;

        public DeleteIngredientModel(MealPlannerRazor.Data.MealPlannerRazorContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IngredientModel IngredientModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IngredientModel = await _context.IngredientModel.FindAsync(id);

            if (IngredientModel != null)
            {
                _context.IngredientModel.Remove(IngredientModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Edit", new { id = IngredientModel.RecipeId });
        }
    }
}
