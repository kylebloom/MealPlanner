using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MealPlannerRazor.Data;
using MealPlannerRazor.Models;

namespace MealPlannerRazor.Pages.Recipes
{
    public class EditModel : PageModel
    {
        private readonly MealPlannerRazor.Data.MealPlannerRazorContext _context;

        public EditModel(MealPlannerRazor.Data.MealPlannerRazorContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RecipeModel RecipeModel { get; set; }

        [BindProperty]
        public IngredientModel IngredientModel { get; set; }

        public List<IngredientModel> IngredientList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var copy = id;

            

            if (id == null)
            {
                return NotFound();
            }

            RecipeModel = await _context.RecipeModel.FirstOrDefaultAsync(m => m.Id == id);
           
            IngredientList = await _context.IngredientModel
                .Where(r => r.RecipeId == id)
                .ToListAsync();

            //var query = from i in _context.IngredientModel
            //            where i.RecipeId == RecipeModel.Id
            //            select i;



            if (RecipeModel == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RecipeModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeModelExists(RecipeModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAddIngredientAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.IngredientModel.Add(IngredientModel);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Edit", new { id = Request.Form["recipeId"] });
        }

        private bool RecipeModelExists(int id)
        {
            return _context.RecipeModel.Any(e => e.Id == id);
        }
    }
}
