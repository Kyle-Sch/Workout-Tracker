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
    public class WorkoutClass 
    {
        [Required]
        public int classId { get; set; }

        [Required]
        public string Name { get; set; }

        public string InstructorName { get; set; } 

	    public int AvailableSpots { get; set; }
        
        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        
        public bool IsActive { get; set; }

        public IList<Workout> Workouts { get; set; }
        public IList<WorkoutClass> Classes { get; set; }

        public List<User> UserRoster { get; set; }
    }
}
