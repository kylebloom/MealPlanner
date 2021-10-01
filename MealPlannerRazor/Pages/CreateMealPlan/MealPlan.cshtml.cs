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
        public IQueryable<PastMealsModel> PastMealsQuery { get; set; }
        public IList<PastMealsModel> PastMealsList { get; set; }


        public string[] DaysOfWeek = new string[] {
            "Monday", "Tuesday","Wednesday", "Thursday","Friday", "Saturday","Sunday"};
        


        public void OnPost()
        {
            int numberOfMeals = int.Parse(Request.Form["numberOfMeals"]);
            CreateMealPlan(_context, numberOfMeals);
        }

        public void OnGet()
        {
            
        }

        public void CreateMealPlan(Data.MealPlannerRazorContext _context, int numberOfMeals)
        {
            
            RecipeModel RandomRecipe = new RecipeModel();

            // check to make sure pasta is only being selected once per week. 
            bool pastaFound = false;

            // random integer to select a ID at random. 
            Random rand = new Random();

            List<RecipeModel> mealplan = new List<RecipeModel>();
            DateTime currentDate = DateTime.Now;

            PastMealsList = _context.PastMealsModel
                .AsEnumerable()
                .Where(x => x.Date.Subtract(currentDate).TotalDays < 30)
                .ToList();



            int i = 0;
            while (i < numberOfMeals)
            {
                int toSkip = rand.Next(0, _context.RecipeModel.Count());
                RandomRecipe = _context.RecipeModel.Skip(toSkip).Take(1).First();

                if (MealNotUsed(RandomRecipe))
                {
                    if (RandomRecipe.Type == "Pasta" && pastaFound == false)
                    {
                        pastaFound = true;

                        // add conditional to check how many times meal has been had. if more than the current count of meals repetitive, then do not use that recipe.
                        mealplan.Add(RandomRecipe);
                        i++;
                        AddMealToPastMeals(RandomRecipe);
                    }
                    else if (RandomRecipe.Type != "Pasta")
                    {
                        // add conditional to check how many times meal has been had. if more than the current count of meals repetitive, then do not use that recipe.
                        mealplan.Add(RandomRecipe);
                        i++;
                        AddMealToPastMeals(RandomRecipe);
                    }
                }
            }
            this.Mealplan = mealplan;
        }

        // adds the currently selected meal to the database with a datetime stamp.
        public void AddMealToPastMeals(RecipeModel recipe)
        {
            DateTime currentDate = DateTime.Now;
            PastMealsModel currPastMeal = new PastMealsModel();
            currPastMeal.Date = currentDate;
            currPastMeal.RecipeId = recipe.Id;
            PastMealsList.Add(currPastMeal);
            _context.PastMealsModel.Add(currPastMeal);
            _context.SaveChanges();
        }

        public bool MealNotUsed(RecipeModel currRecipe)
        {
            foreach (var pastmeal in PastMealsList)
            {
                if (pastmeal.RecipeId == currRecipe.Id)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
