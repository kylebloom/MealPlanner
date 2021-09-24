using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MealPlannerRazor.Data;
using MealPlannerRazor.Models;

namespace MealPlannerRazor.Pages.Recipes
{
    public class CreateModel : PageModel
    {
        private readonly MealPlannerRazor.Data.MealPlannerRazorContext _context;

        public CreateModel(MealPlannerRazor.Data.MealPlannerRazorContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RecipeModel RecipeModel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RecipeModel.Add(RecipeModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
