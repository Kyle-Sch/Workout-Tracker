using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Equipment
    {
        public int EquipmentID { get; set; }
        public string Name { get; set; }
        public bool NeedsMaintenance { get; set; }
        public string FormMedia { get; set; }
        public string Instructions { get; set; }
        public bool IsActive { get; set; }

        public static List<SelectListItem> EquipmentCategories = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "Bench", Value = "benchpress" },
            new SelectListItem() { Text = "Treadmill", Value = "treadmill" },
            new SelectListItem() { Text = "Elliptical", Value = "elliptical" },
            new SelectListItem() { Text = "Hand Weights", Value = "handweights" },
            new SelectListItem() { Text = "Leg Press", Value = "legpress" },
            new SelectListItem() { Text = "None", Value = "none" },

            };

        public static List<SelectListItem> EquipmentIds = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "None", Value = "6" },
            new SelectListItem() { Text = "Bench", Value = "3" },
            new SelectListItem() { Text = "Treadmill", Value = "1" },
            new SelectListItem() { Text = "Elliptical", Value = "2" },
            new SelectListItem() { Text = "Hand Weights", Value = "4" },
            new SelectListItem() { Text = "Leg Press", Value = "5" },
            };

        public List<Workout> Workouts { get; set; }
        public Workout TotalWorkout { get; set; }
        public List<Equipment> EquipList { get; set; }
        
        //this is to get rid of errors -Corey
        public object FormVideo { get; set; }
    }
}
