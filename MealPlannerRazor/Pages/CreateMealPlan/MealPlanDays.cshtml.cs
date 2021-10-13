using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MealPlannerRazor.Pages.CreateMealPlan
{
    public class MealPlanDaysModel : PageModel
    {

        public void OnPost()
        {
            int numberOfMeals = int.Parse(Request.Form["numberOfMeals"]);
            Days(numberOfMeals);
            {
                DateTime todaysDate = DateTime.Now.Date;
            }
        }


        public void Days(int numberOfMeals)
        {

        }
    }
}
