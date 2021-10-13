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


        public List<string> daysChecked = new List<string>();
        public Dictionary<string, DateTime> daysForMeals = new Dictionary<string, DateTime>();



        public void OnPost()
        {
            DateTime today = DateTime.Today;

            if (Request.Form["monCheck"] == "on")
            {
                daysChecked.Add("Monday");

                // calculates the next date 
                int date = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
                daysForMeals.Add("Monday", today.AddDays(date));
            }
            if (Request.Form["tueCheck"] == "on")
            {
                daysChecked.Add("Tuesday");
                int date = ((int)DayOfWeek.Tuesday - (int)today.DayOfWeek + 7) % 7;
                daysForMeals.Add("Tuesday", today.AddDays(date));
            }
            if (Request.Form["wedCheck"] == "on")
            {
                daysChecked.Add("Wednesday");
                int date = ((int)DayOfWeek.Wednesday - (int)today.DayOfWeek + 7) % 7;
                daysForMeals.Add("Wednesday", today.AddDays(date));
            }
            if (Request.Form["thuCheck"] == "on")
            {
                daysChecked.Add("Thursday");
                int date = ((int)DayOfWeek.Thursday - (int)today.DayOfWeek + 7) % 7;
                daysForMeals.Add("Thursday", today.AddDays(date));
            }
            if (Request.Form["friCheck"] == "on")
            {
                daysChecked.Add("Friday");
                int date = ((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7;
                daysForMeals.Add("Friday", today.AddDays(date));
            }
            if (Request.Form["satCheck"] == "on")
            {
                daysChecked.Add("Saturday");
                int date = ((int)DayOfWeek.Saturday - (int)today.DayOfWeek + 7) % 7;
                daysForMeals.Add("Saturday", today.AddDays(date));
            }
            if (Request.Form["sunCheck"] == "on")
            {
                daysChecked.Add("Sunday");
                int date = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
                daysForMeals.Add("Sunday", today.AddDays(date));
            }

            CreateMealPlan(_context, daysForMeals.Count);
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

            foreach(var day in daysForMeals)
            {
                bool mealselected = false;
                while (!mealselected)
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
                            AddMealToPastMeals(RandomRecipe, day.Value);
                            mealselected = true;
                        }
                        else if (RandomRecipe.Type != "Pasta")
                        {
                            // add conditional to check how many times meal has been had. if more than the current count of meals repetitive, then do not use that recipe.
                            mealplan.Add(RandomRecipe);
                            AddMealToPastMeals(RandomRecipe, day.Value);
                            mealselected = true;
                        }
                    }
                }
            }
            this.Mealplan = mealplan;
        }

        // adds the currently selected meal to the database with a datetime stamp.
        public void AddMealToPastMeals(RecipeModel recipe, DateTime dateOfMeal)
        {
            
            PastMealsModel currPastMeal = new PastMealsModel();
            currPastMeal.Date = dateOfMeal;
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
