using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealPlannerRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MealPlannerRazor.Pages.CreateMealPlan
{
    public class MealPlanModel : PageModel
    {

        private readonly MealPlannerRazor.Data.MealPlannerRazorContext _context;
        public MealPlanModel(MealPlannerRazor.Data.MealPlannerRazorContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<RecipeModel> Mealplan { get; set; }
     
        public RecipeModel RandomRecipe { get; set; }
        public string[] DaysOfWeek = new string[] { 
            "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
      

        public void OnGet()
        {
            // check to make sure pasta is only being selected once per week. 
            bool pastaFound = false;

            // random integer to select a ID at random. 
            Random rand = new Random();
            
            Mealplan = new List<RecipeModel>();

            int i = 0;
            while (i < 7)
            {
                int toSkip = rand.Next(0, _context.RecipeModel.Count());
                RandomRecipe = _context.RecipeModel.Skip(toSkip).Take(1).First();
                if (RandomRecipe.Type == "Pasta" && pastaFound == false)
                {
                    pastaFound = true;
                    Mealplan.Add(RandomRecipe);
                    i++;
                }
                else if(RandomRecipe.Type != "Pasta")
                {
                    Mealplan.Add(RandomRecipe);
                    i++;
                }
            }
        }
    }
}
