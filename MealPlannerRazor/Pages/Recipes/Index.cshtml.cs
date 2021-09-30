using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MealPlannerRazor.Data;
using MealPlannerRazor.Models;
using System.Reflection;

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
            string sortMethod;
            if (!String.IsNullOrEmpty(Request.Query["sort"]))
            {
                // Query string value is there so now use it
                sortMethod = Request.Query["sort"];

                switch (sortMethod)
                {
                    case "name":
                        RecipeModel = await _context.RecipeModel
                        .OrderBy(r => r.Name)
                        .ToListAsync();
                        break;
                    case "meat":
                        RecipeModel = await _context.RecipeModel
                        .OrderBy(r => r.Meat)
                        .ToListAsync();
                        break;
                    case "type":
                        RecipeModel = await _context.RecipeModel
                        .OrderBy(r => r.Type)
                        .ToListAsync();
                        break;
                    case "directions":
                        RecipeModel = await _context.RecipeModel
                        .OrderBy(r => r.RecipeDirections)
                        .ToListAsync();
                        break;
                }
            }
            else
            {
                RecipeModel = await _context.RecipeModel
                .OrderBy(r => r.Name)
                .ToListAsync();
            }
            
        }
    }
}
