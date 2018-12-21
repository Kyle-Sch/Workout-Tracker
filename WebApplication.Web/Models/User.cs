using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Web;

namespace WebApplication.Web.Models
{
    public class User 
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Salt { get; set; }
        
        public string Role { get; set; }
        
        public string Photo { get; set; }

        public string GoalType { get; set; }

        public decimal GoalReps { get; set; }

        public bool IsActive { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string message { get; set; }

        public static List<SelectListItem> RoleCategories = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "Member", Value = "member" },
            new SelectListItem() { Text = "Employee", Value = "employee" },
            new SelectListItem() { Text = "Administrator", Value = "admin" },
            };

        public static List<SelectListItem> PhotoCategories = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "Icon 1", Value = "Icon1.png" },
            new SelectListItem() { Text = "Icon 2", Value = "Icon2.png" },
            new SelectListItem() { Text = "Icon 3", Value = "Icon3.png" },
            new SelectListItem() { Text = "Icon 4", Value = "Icon4.png" },
            new SelectListItem() { Text = "Icon 5", Value = "Icon5.png" },
            new SelectListItem() { Text = "Icon 6", Value = "Icon6.png" },
            new SelectListItem() { Text = "Icon 7", Value = "Icon7.png" },
            new SelectListItem() { Text = "Icon 8", Value = "Icon8.png" },
            new SelectListItem() { Text = "Icon 9", Value = "Icon9.png" },
            new SelectListItem() { Text = "Icon 10", Value = "Icon10.png" },
            new SelectListItem() { Text = "Icon 11", Value = "Icon11.png" },
            };

        public List<Workout> Workouts { get; set; }
        public Workout TotalWorkout { get; set; }

        public List<User> UserList { get; set; }
        public string NewPassword { get; set; }
    }
}
