using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MealPlannerRazor.Models;

namespace MealPlannerRazor.Data
{
    public class MealPlannerRazorContext : DbContext
    {
        public MealPlannerRazorContext()
        {

        }

        public MealPlannerRazorContext (DbContextOptions<MealPlannerRazorContext> options)
            : base(options)
        {
        }

        public DbSet<MealPlannerRazor.Models.RecipeModel> RecipeModel { get; set; }
        public DbSet<MealPlannerRazor.Models.PastMeals> PastMeals { get; set; }
    }
}
