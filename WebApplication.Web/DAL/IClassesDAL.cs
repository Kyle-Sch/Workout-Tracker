using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;

namespace WebApplication.Web.DAL
{
    public interface IClassesDAL
    {        
        void CreateClass(WorkoutClass workoutClass);

        void DeleteClass(WorkoutClass workoutClass);

        WorkoutClass GetClass(string userName);
        List<WorkoutClass> GetAllClasses();
        void UpdateClass(WorkoutClass workoutClass);
    }
}
