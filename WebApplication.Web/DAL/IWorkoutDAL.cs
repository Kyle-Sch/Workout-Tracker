using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;

namespace WebApplication.Web.DAL
{
    public interface IWorkoutDAL
    {
        List<Workout> GetWorkout(string username);
        List<Workout> GetWorkoutPerEquipment(int id);
        List<Workout> GetWorkoutPerVist(int id);
        List<Workout> GetWorkoutPerUser(int id);
        void CreateWorkout(Workout workout);
        void UpdateWorkout(Workout workout);
        Workout GetWorkoutTotals(List<Workout> totals, string type, User user);
        void DeleteWorkout(Workout workout);
        Workout GetWorkoutTotals(List<Workout> totals, string name);
    }
}
