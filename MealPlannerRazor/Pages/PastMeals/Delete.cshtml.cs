using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealPlannerRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MealPlannerRazor.Pages.PastMeals
{
    public class DeleteModel : PageModel
    {
        private readonly MealPlannerRazor.Data.MealPlannerRazorContext _context;

        public DeleteModel(MealPlannerRazor.Data.MealPlannerRazorContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PastMealsModel PastMealModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PastMealModel = _context.PastMealsModel.FirstOrDefault(m => m.Id == id);

            if (PastMealModel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PastMealModel =  _context.PastMealsModel.Find(id);

            if (PastMealModel != null)
            {
                _context.PastMealsModel.Remove(PastMealModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
