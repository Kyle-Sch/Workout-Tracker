using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebApplication.Web.Extensions;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Models.Authentication;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Web.Controllers
{
    public class AdministratorController : Controller
    {

        private const string USERNAMEKEY = "Auth_Username";
        UserSqlDAL userDAL = new UserSqlDAL(@"Data Source=.\SQLEXPRESS;Initial Catalog=WorkoutDB;Integrated Security=True");
        public IActionResult Index()
        {
            string currentUserName = HttpContext.Session.Get<string>(USERNAMEKEY);
            User currentUser = userDAL.GetUser(currentUserName);
            if (currentUser.Role != "admin")
            {
                return RedirectToAction("NotAuthorized");
            }
            else
            {
                return View();
            }
        }

        public IActionResult AddEmployee()
        {

            return View();
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        [HttpPost]
        public ActionResult<dynamic> Register(RegisterViewModel model)
        {
            AuthenticationController myAuthentication = new AuthenticationController(userDAL);

            NewUserModel newUser = new NewUserModel()
            {
                Username = model.Email,
                Password = model.Password
            };

            var result = myAuthentication.EmployeeRegister(newUser);
            Message message = new Message();
            if (result.Result.GetType() == typeof(Microsoft.AspNetCore.Mvc.OkResult))
            {

                message.MyMessage = $"Congratulations! {newUser.Username} has been registered";
            }
            else
            {
                message.MyMessage = "There was an error in registering you.";
            }

            return RedirectToAction("Index", message);
        }
    }
}