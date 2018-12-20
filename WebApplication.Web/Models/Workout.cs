using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Workout
    {
        public int WorkoutID { get; set; }
        public int VisitID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Reps { get; set; }
        public decimal TotalReps { get; set; }
        public decimal Weight { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int EquipmentID { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }

        public static List<SelectListItem> WorkoutCategories = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "Warm-Up", Value = "warmup" },
            new SelectListItem() { Text = "Cardio", Value = "cardio" },
            new SelectListItem() { Text = "Strength - Upper Body", Value = "upperBody" },
            new SelectListItem() { Text = "Strength - Lower Body", Value = "lowerBody" },
            new SelectListItem() { Text = "Core", Value = "core" },
            new SelectListItem() { Text = "Cool-down", Value = "cooldown" },
            };

        public static List<SelectListItem> GoalTypes = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "Run in Miles", Value = "run" },
            new SelectListItem() { Text = "Push Ups", Value = "pushups" },
            new SelectListItem() { Text = "Bench Press Reps of 125 lbs", Value = "benchpress" },
            new SelectListItem() { Text = "Leg Presses", Value = "legpress" },
            new SelectListItem() { Text = "Situps", Value = "situps" }
            };

        public static List<SelectListItem> GoalReps = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "5", Value = "5" },
            new SelectListItem() { Text = "10", Value = "10" },
            new SelectListItem() { Text = "50", Value = "50" },
            new SelectListItem() { Text = "75", Value = "75" },
            new SelectListItem() { Text = "100", Value = "100" },
            new SelectListItem() { Text = "125 ", Value = "125" }
            };

        public static List<SelectListItem> WorkoutNames = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "Running", Value = "run" },
            new SelectListItem() { Text = "Sit Ups", Value = "situps" },
            new SelectListItem() { Text = "Bench Press", Value = "benchpress" },
            new SelectListItem() { Text = "Push Ups", Value = "pushups" },
            new SelectListItem() { Text = "Leg Press", Value = "legpress" },
            new SelectListItem() { Text = "Bicep Curls", Value = "curls" }
            };
    }
}
