using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MealPlannerRazor.Pages.CreateMealPlan
{
    public class MealPlanTestModel : PageModel
    {
        
        public int numberOfMeals { get; set; }
        public bool monChecked { get; set; }
        public List<string> daysChecked { get; set; }

        public void OnPost()
        {
            List<string> daysChecked = new List<string>();

           if (Request.Form["monCheck"] == "on")
            {
                daysChecked.Add("Monday");
            }
            if (Request.Form["tueCheck"] == "on")
            {    
                daysChecked.Add("Tuesday");
            }
            if (Request.Form["wedCheck"] == "on")
            { 
                daysChecked.Add("Wednesday");
            }
            if (Request.Form["thuCheck"] == "on")
            {    
                daysChecked.Add("Thursday");
            }
            if (Request.Form["friCheck"] == "on")
            {    
                daysChecked.Add("Friday");
            }
            if (Request.Form["satCheck"] == "on")
            {  
                daysChecked.Add("Saturday");
            }
            if (Request.Form["sunCheck"] == "on")
            {  
                daysChecked.Add("Sunday");
            }

            numberOfMeals = daysChecked.Count;
        }
    }
}
