using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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

        public StringBuilder calLink = new StringBuilder();


        public void OnPost()
        {
            DateTime today = DateTime.Today;

            // using the checkmarks, finds the next date of the selected days to use that as the date entry in the database. 
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
            StartCalendarFile();
            CreateMealPlan(_context, daysForMeals.Count);
        }

        public void OnGet()
        {
            
        }

        public void CreateMealPlan(Data.MealPlannerRazorContext _context, int numberOfMeals)
        {


            // create new email client using gmail with mealplanner.kyle@gmail.com address
            // credentials would not be kept in the code in an application like this in the real world. 
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("mealplanner.kyle@gmail.com", "Swimming@1993NM"),
                EnableSsl = true
            };

            // build string for subject and body of email 
            StringBuilder subjectBuilder = new StringBuilder();
            subjectBuilder.Append("Meal plan for " + DateTime.Now.ToString("MM/dd/yyyy"));
            StringBuilder bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine("Below you will find a list of each recipes' ingredients for this week's meal plan. Check to see if we have them and if not add them to Alexa shopping list.");
            bodyBuilder.AppendLine("");
            
            


            RecipeModel RandomRecipe = new RecipeModel();

            List<IngredientModel> IngredientList = new List<IngredientModel>();
            IngredientModel recipeIngredients = new IngredientModel();

            // check to make sure pasta is only being selected once per week. 
            bool pastaFound = false;

            // random integer to select a ID at random. 
            Random rand = new Random();

            List<RecipeModel> mealplan = new List<RecipeModel>();
            DateTime currentDate = DateTime.Now;

            // a list of past meals had in the last 30 days to compare with randomly selected meals.
            // get date 30 days ago 
            var thirtyDaysAgo = currentDate.AddMonths(-1);
            PastMealsList = _context.PastMealsModel
                .AsEnumerable()
                .Where(x => x.Date > thirtyDaysAgo)
                .ToList();

            // start vcalendar file here 
            // each meal needs to be an event to the calendar file
            foreach(var day in daysForMeals)
            {
                bodyBuilder.AppendLine(day.Key + " " + day.Value.ToString("MM/dd/yyyy"));

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

                            //function to add meal info to calendar event file.
                            AddMealEntryToCalendarFile(day.Value, RandomRecipe.Name, RandomRecipe.RecipeDirections);

                            // add ingredients to recipe email body
                            IngredientList = _context.IngredientModel
                                .Where(r => r.RecipeId == RandomRecipe.Id)
                                .ToList();

                            bodyBuilder.AppendLine(RandomRecipe.Name);
                            bodyBuilder.AppendLine("");

                            if (IngredientList.Count == 0)
                            {
                                bodyBuilder.AppendLine("No ingredients found for recipe.\n");
                                
                            }
                            else
                            {
                                foreach (var ingredient in IngredientList)
                                {                                 
                                    bodyBuilder.AppendLine(ingredient.Name);
                                }
                            }
                            
                            bodyBuilder.AppendLine("\n");

                        }
                        else if (RandomRecipe.Type != "Pasta")
                        {
                            // add conditional to check how many times meal has been had. if more than the current count of meals repetitive, then do not use that recipe.
                            mealplan.Add(RandomRecipe);
                            AddMealToPastMeals(RandomRecipe, day.Value);
                            mealselected = true;

                            //function to add meal info to calendar event file.
                            AddMealEntryToCalendarFile(day.Value, RandomRecipe.Name, RandomRecipe.RecipeDirections);

                            // add ingredients to recipe email body
                            IngredientList = _context.IngredientModel
                                .Where(r => r.RecipeId == RandomRecipe.Id)
                                .ToList();

                            bodyBuilder.AppendLine(RandomRecipe.Name);
                            if (IngredientList.Count == 0)
                            {
                                bodyBuilder.Append("No ingredients found for recipe.");
                            }
                            else
                            {
                                foreach (var ingredient in IngredientList)
                                {
                                    bodyBuilder.AppendLine(ingredient.Name);
                                }
                            }
                            bodyBuilder.AppendLine("");
                        }
                    }
                }
            }
            // end the calendar file.
            calLink.AppendLine("END:VCALENDAR");

            string calendarItem = calLink.ToString();
            System.IO.File.WriteAllText("wwwroot/data/CalendarItem.ics", calendarItem);
            this.Mealplan = mealplan;

            client.Send("mealplanner.kyle@gmail.com", "kylebloom05@gmail.com", subjectBuilder.ToString(), bodyBuilder.ToString());
            client.Send("mealplanner.kyle@gmail.com", "anna_bishop@live.com", subjectBuilder.ToString(), bodyBuilder.ToString());
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




        // check to ensure meal hasn't been used in the last 30 days, returns false if it has.
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


        public void StartCalendarFile()
        {
            calLink.AppendLine("BEGIN:VCALENDAR");
            calLink.AppendLine("VERSION:2.0");
            calLink.AppendLine("PRODID:Meal Planner App");
            calLink.AppendLine("CALSCALE:GREGORIAN");
            calLink.AppendLine("METHOD:PUBLISH");
        }

        public void AddMealEntryToCalendarFile(DateTime recipeDate, string recipeName, string recipeDirection)
        {
            // start the event
            calLink.AppendLine("BEGIN:VEVENT");

            //input date of recipe
            calLink.AppendLine("DTSTART:" + recipeDate.AddHours(17).ToString("yyyyMMddTHHmm00"));
            calLink.AppendLine("DTEND:" + recipeDate.AddHours(19).ToString("yyyyMMddTHHmm00"));

            // add recipe information
            calLink.AppendLine($"SUMMARY: {recipeName}");
            calLink.AppendLine("LOCATION:");
            calLink.AppendLine($"DESCRIPTION: {recipeDirection}");
            calLink.AppendLine("PRIORITY:3");

            // add unique ID and date stamp of created calendar event
            calLink.AppendLine("UID:" + (Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddTHHmm00")));
            calLink.AppendLine("DTSTAMP:" + DateTime.Now.ToString("yyyyMMddTHHmm00"));

            calLink.AppendLine("BEGIN: VALARM");
            calLink.AppendLine("TRIGGER:-PT0H");
            calLink.AppendLine("REPEAT: 1");
            calLink.AppendLine("DURATION: PT15M");
            calLink.AppendLine("ACTION:DISPLAY");
            calLink.AppendLine($"DESCRIPTION: {recipeName}");
            calLink.AppendLine("END:VALARM");

            //end event 
            calLink.AppendLine("END:VEVENT");
        }
        
    }
}
