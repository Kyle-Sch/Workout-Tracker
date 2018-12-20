using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.Providers.Auth;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Web.Controllers
{
    public class EmployeeController : Controller
    {
        //private IAuthProvider authProvider;
        private IVisitDAL visitDAL;
        private IUserDAL userDAL;

        public EmployeeController(IUserDAL userDAL, IVisitDAL visitDAL)
        {
           // this.authProvider = authProvider;
            this.visitDAL = visitDAL;
            this.userDAL = userDAL;
        }
        // GET: /<controller>/
        public IActionResult Index(Message message)
        {
            return View(message);
        }
        
        public IActionResult CheckInCheckOut()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CheckInCheckOut2(MemberToBeCheckInOut model)
        {
            
            User userLookedUp = userDAL.GetUserWithDapper(model.UserEmail);
            if(userLookedUp == null)
            {
                return View("Index", new Message($"{model.UserEmail} does not exist"));
            }
            
            model.UserId = userLookedUp.Id;
            model.IsCheckedIn = visitDAL.HasOpenVisit(userLookedUp.Id);

            return View(model);
        }
        
        [HttpPost]
        public IActionResult CheckIn(MemberToBeCheckInOut model)
        {
            visitDAL.CheckIn(model.UserId);
            return View("Index", new Message($"{model.UserEmail} is checked in"));
        }
        [HttpPost]
        public IActionResult CheckOut(MemberToBeCheckInOut model)
        {
            visitDAL.CheckOut(model.UserId);
            return View("Index", new Message($"{model.UserEmail} is checked out"));
        }

        [HttpGet]
        public IActionResult CheckMemberIn()
        {
            CheckMemberInOutModel model = new CheckMemberInOutModel();
            model.Users = userDAL.GetUsersNotCheckedIn();

            return View(model);
        }
        [HttpPost]
        public IActionResult CheckMemberIn(CheckMemberInOutModel model)
        {
            visitDAL.CheckIn(model.ToBeChecked.Id);
           // Message message = new Message(model.ToBeChecked.Username + "have been logged in");
            return RedirectToAction("CheckMemberIn");
        }
        [HttpGet]
        public IActionResult CheckMemberOut()
        {
            CheckMemberInOutModel model = new CheckMemberInOutModel();
            model.Users = userDAL.GetUsersCheckedIn();
            return View(model);
        }
        [HttpPost]
        public IActionResult CheckMemberOut(CheckMemberInOutModel model)
        {
            visitDAL.CheckOut(model.ToBeChecked.Id);
            //Message message = new Message(model.ToBeChecked.Username + "have been logged in");
            return RedirectToAction("CheckMemberOut");
        }
    }
}
