using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Models.Authentication;
using WebApplication.Web.Models;
using WebApplication.Web.Extensions;
using Microsoft.AspNetCore.Http;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Web.Controllers
{
    public class ClassesController : Controller
    {
        private const string USERNAMEKEY = "Auth_Username";
        private IUserDAL userDAL;
        private IWorkoutDAL workoutDAL;
        private readonly IAuthProvider authProvider;
        private IClassesDAL classDAL;

        public ClassesController(IUserDAL userDAL, IAuthProvider authProvider, IWorkoutDAL workoutDAL, IClassesDAL classDAL)
        {
            this.userDAL = userDAL;
            this.authProvider = authProvider;
            this.workoutDAL = workoutDAL;
            this.classDAL = classDAL;
        }

        public IActionResult RemoveClass(WorkoutClass removed)
        {
            
            classDAL.DeleteClass(removed);
            return RedirectToAction("ClassList", "Classes");
        }
        public IActionResult ClassList()
        {
            WorkoutClass newClass = new WorkoutClass();
            newClass.Classes = new List<WorkoutClass>();
            newClass.Classes = classDAL.GetAllClasses();
            return View(newClass);
        }
        //public IActionResult EditClassList(WorkoutClass edited)
        //{
        //    User newUser = userDAL.GetUserWithDapper(editedUser.Username);
        //    return View(newUser);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult EditClass(WorkoutClass edited)
        //{
        //    userDAL.UpdateUser(editedUser);
        //    return RedirectToAction("UserList", "Account");
        //}
        
    }
}