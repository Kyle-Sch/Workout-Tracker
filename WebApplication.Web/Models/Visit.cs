using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Visit
    {
        public int VisitID { get; set; }
        public int MemberId { get; set; } /// <summary>
        /// member id
        /// </summary>
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }

        public bool IsActive { get; set; }

        public IList<Workout> Workouts { get; set; }
    }
}
