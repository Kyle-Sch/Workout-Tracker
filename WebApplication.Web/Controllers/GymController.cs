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
using WebApplication.Web.Providers;

namespace WebApplication.Web.Controllers
{
    public class GymController : Controller
    {
        private const string USERNAMEKEY = "Auth_Username";
        private IEquipmentDAL equipmentDAL;
        private IWorkoutDAL workoutDAL;
        private IVisitDAL visitDAL;
        private IUserDAL userDAL;
        private IAuthProvider authProvider;

        public GymController(IEquipmentDAL equipmentDAL, IWorkoutDAL workoutDAL, IVisitDAL visitDAL, 
            IAuthProvider authProvider, IUserDAL userDAL)
        {
            this.workoutDAL = workoutDAL;
            this.userDAL = userDAL;
            this.equipmentDAL = equipmentDAL;
            this.visitDAL = visitDAL;
            this.authProvider = authProvider;
        }
        //Check in/out
        public IActionResult Index()
        {
            bool isCheckedIn = false;
            if (authProvider.IsLoggedIn)
            {
                isCheckedIn = visitDAL.HasOpenVisit(authProvider.GetCurrentUser().Id);
            }
            return View(isCheckedIn);
        }
        
        [HttpPost]
        public IActionResult CheckIn()
        {
            User currentUser = authProvider.GetCurrentUser();
            if (currentUser == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (visitDAL.HasOpenVisit(currentUser.Id))
                {
                    visitDAL.CheckOut(currentUser.Id);
                }
                else
                {
                    visitDAL.CheckIn(currentUser.Id);

                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Checkout()
        {
            User currentUser = authProvider.GetCurrentUser();
            if (currentUser == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (visitDAL.HasOpenVisit(currentUser.Id))
                {
                    visitDAL.CheckOut(currentUser.Id);
                }
            }
            return RedirectToAction("Index");
        }
        //Equipment

        public IActionResult EquipmentAssistance()
        {
            List<Equipment> equipList = new List<Equipment>();
            equipList = equipmentDAL.GetAllEquipment();
            return View(equipList);
        }
        public IActionResult EquipList()
        {
            Equipment newEquip = new Equipment();
            newEquip.Workouts = new List<Workout>();
            newEquip.EquipList = equipmentDAL.GetAllEquipment();
            return View(newEquip);
        }

        [HttpPost]
        public IActionResult EquipmentDetail(Equipment selectedEquip)
        {
            Equipment newEquip = equipmentDAL.GetEquipment(selectedEquip.EquipmentID);
            newEquip.Workouts = workoutDAL.GetWorkoutPerEquipment(selectedEquip.EquipmentID);
            newEquip.TotalWorkout = workoutDAL.GetWorkoutTotals(newEquip.Workouts, newEquip.Name);
            return View(newEquip);
        }
        
        public IActionResult EditEquipment(Equipment selectedEquip)
        {
            Equipment newEquip = equipmentDAL.GetEquipment(selectedEquip.EquipmentID);
            return View(newEquip);
        }

        public IActionResult EditEquip(Equipment selectedEquip)
        {
            selectedEquip.ImgMedia = selectedEquip.Name;
            selectedEquip.FormMedia = selectedEquip.ImgMedia;
            equipmentDAL.UpdateEquipment(selectedEquip);
            return RedirectToAction("EquipList", "Gym");
        }
        
        public IActionResult RemoveEquipment(Equipment removed)
        {
            equipmentDAL.DeleteEquipment(removed);
            return RedirectToAction("Index", "Home");
        }
        //Workout
        [HttpGet]
        public IActionResult AddWorkout()
        {
            string name = HttpContext.Session.Get<string>(USERNAMEKEY);
            User loggedInUser = userDAL.GetUserWithDapper(name);
            if (visitDAL.HasOpenVisit(loggedInUser.Id))
            {
                Workout workIt = new Workout();
                workIt.VisitID = visitDAL.GetVisit(loggedInUser.Id).VisitID;
                workIt.UserId = loggedInUser.Id;
                return View(workIt);
            } else
            {
                return RedirectToAction("Index", "Gym");
            }
        }

        [HttpPost]
        public IActionResult AddWorkout(Workout workIt)
        {
            workoutDAL.CreateWorkout(workIt);
            //workoutDAL.UpdateWorkout(workIt);
            return RedirectToAction("Index", "Gym");
        }
        [HttpGet]
        public IActionResult Metrics()
        {
            VisitMetricsModel model = new VisitMetricsModel();
            model.Visits = visitDAL.GetVisits(authProvider.GetCurrentUser().Id);
            model.averageVisitTimeSpan = VisitCalculator.GetAvgVisit(model.Visits);
            return View(model);
        }
    }
}