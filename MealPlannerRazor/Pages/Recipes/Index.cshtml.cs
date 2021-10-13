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
using Microsoft.AspNetCore.Mvc.Rendering;

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
        [BindProperty(SupportsGet=true)]
        public string SearchString { get; set; }
        public SelectList Meats { get; set; }
        [BindProperty(SupportsGet =true)]
        public string MeatSelection { get; set; }
        public int CountOfRecipes { get; set; }
     
        public async Task OnGetAsync()
        {
            var recipes = from r in _context.RecipeModel
                          orderby r.Name
                          select r;
            IQueryable<string> meatQuery = from m in _context.RecipeModel
                                           orderby m.Meat
                                           select m.Meat;
            string sortMethod;
            if (!String.IsNullOrEmpty(Request.Query["sort"]))
            {
                // sorts the list of recipes based off the selected button on the Recipes page
                sortMethod = Request.Query["sort"];
                
                switch (sortMethod)
                {
                    case "name":
                         recipes = from r in _context.RecipeModel
                                  orderby r.Name
                                  select r;
                        break;
                    case "namebackwards":
                        recipes = from r in _context.RecipeModel
                                      orderby r.Name descending
                                      select r;
                        break;
                    case "meat":
                        recipes = from r in _context.RecipeModel
                                      orderby r.Meat
                                      select r;
                        break;
                    case "meat2":
                        recipes = from r in _context.RecipeModel
                                  orderby r.Meat descending
                                  select r;
                        break;
                    case "type":
                        recipes = from r in _context.RecipeModel
                                  orderby r.Type 
                                  select r;
                        break;
                    case "type2":
                        recipes = from r in _context.RecipeModel
                                  orderby r.Type descending
                                  select r;
                        break;
                    case "directions":
                        recipes = from r in _context.RecipeModel
                                  orderby r.RecipeDirections
                                  select r;
                        break;
                    case "directions2":
                        recipes = from r in _context.RecipeModel
                                  orderby r.RecipeDirections descending
                                  select r;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                recipes = (IOrderedQueryable<RecipeModel>)recipes.Where(r => r.Name.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MeatSelection))
            {
                recipes = (IOrderedQueryable<RecipeModel>)recipes.Where(r => r.Meat == MeatSelection);
            }
            Meats = new SelectList(await meatQuery.Distinct().ToListAsync());
            RecipeModel = await recipes.ToListAsync();
            
            
        }
    }
}
