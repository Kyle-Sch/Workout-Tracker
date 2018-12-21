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
    public class AccountController : Controller
    {
        private const string USERNAMEKEY = "Auth_Username";
        private IUserDAL userDAL;
        private IWorkoutDAL workoutDAL;
        private readonly IAuthProvider authProvider;

        public AccountController(IUserDAL userDAL, IAuthProvider authProvider, IWorkoutDAL workoutDAL)
        {
            this.userDAL = userDAL;
            this.authProvider = authProvider;
            this.workoutDAL = workoutDAL;
        }

        [HttpGet]
        public IActionResult Index(User user)
        {
            string name = HttpContext.Session.Get<string>(USERNAMEKEY);
            if (name == null || name == "" )
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Logoff()
        {
            //HttpContext.Session.Set(USERNAMEKEY, null);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //This is the Seed code
                bool validLogin = authProvider.SignIn(model.Email, model.Password);
                if (validLogin)
                {
                    // Redirect the user where you want them to go after successful login
                    return RedirectToAction("Index", "Account");
                }


            }

            return RedirectToAction("Feedback", new Message("Invalid Username or Password"));
        }

        [HttpPost]
        public ActionResult<dynamic> Register(RegisterViewModel model)
        {                      
            bool wasThereASuccessfulRegistration = authProvider.Register(model.Email, model.Password, "member");

            Message message = new Message();
            if (wasThereASuccessfulRegistration)
            {

                message.MyMessage = $"Congratulations! {model.Email} has been registered";
            }
            else
            {
                message.MyMessage = "There was an error in registering you.";
                return RedirectToAction("Feedback", message);
            }

            bool validLogin = authProvider.SignIn(model.Email, model.Password);
            if (!validLogin)
            {

                //userDAL.UpdateErrorMessage(message.MyMessage, userDAL.GetUserWithDapper(model.Email));
                return RedirectToAction("Index", userDAL.GetUserWithDapper(model.Email));
            }
            else
            {
                return RedirectToAction("Index", message);
            }
            
        }

        [HttpGet]
        public IActionResult profile(Message message)
        {
            string newUser = HttpContext.Session.Get<string>(USERNAMEKEY);
            if(newUser == null)
            {
                return RedirectToAction("Login");
            }
            User valuedUser = userDAL.GetUser(newUser);
            valuedUser.Workouts = workoutDAL.GetWorkoutPerUser(valuedUser.Id);
            if (valuedUser.Workouts.Count > 0)
            {
                valuedUser.TotalWorkout = workoutDAL.GetWorkoutTotals(valuedUser.Workouts, valuedUser.GoalType, valuedUser);
            }
            return View(valuedUser);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            string newUser = HttpContext.Session.Get<string>(USERNAMEKEY);
            User valuedUser = userDAL.GetUser(newUser);
            return View(valuedUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(User editedUser)
        {
            userDAL.UpdateUser(editedUser);
            if(editedUser.Username != HttpContext.Session.Get<string>(USERNAMEKEY))
            {
                HttpContext.Session.Set<string>(USERNAMEKEY, editedUser.Username);
            }
            return RedirectToAction("Profile", "Account");
        }
        [HttpGet]
        public IActionResult RemoveProfile()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AdminRemoveProfile(User model)
        {
            model = userDAL.GetUser(model.Id);
            return View(model);
        }
        [HttpPost]
        public IActionResult AdminRemoveUser(User toRemove)
        {
            User currentUser = authProvider.GetCurrentUser();
            if(currentUser != null && currentUser.Role == "admin")
            {
                userDAL.DeleteUser(toRemove);
                return RedirectToAction("Feedback", new Message("User removed"));
            }
            else
            {
                return RedirectToAction("Feedback", new Message("You don't have permission for that"));
            }
        }
        [HttpPost]
        public IActionResult RemoveProfile(User toRemove)
        {
            toRemove.Password = toRemove.Password == null ? "" : toRemove.Password;
            string username = HttpContext.Session.Get<string>(USERNAMEKEY);
            if(authProvider.VerifyPassword(username, toRemove.Password))
            {
                userDAL.DeleteUser(userDAL.GetUser(username));
                HttpContext.Session.Clear();
                return RedirectToAction("Feedback", new Message("Your profile was deleted"));
            }
            
            return RedirectToAction("Feedback", new Message("Incorrect Password. Profile Deletion failed."));
        }
        public IActionResult UserList()
        {
            User newUsers = new User();
            newUsers.UserList = new List<User>();
            newUsers.UserList = userDAL.GetAllUser();
            newUsers.Workouts = workoutDAL.GetWorkoutPerUser(newUsers.Id);
            if (newUsers.Workouts.Count > 0)
            {
                newUsers.TotalWorkout = workoutDAL.GetWorkoutTotals(newUsers.Workouts, newUsers.GoalType, newUsers);
            }
            return View(newUsers);
        }
        
        public IActionResult EditUserList(User editedUser)
        {
            User newUser = userDAL.GetUserWithDapper(editedUser.Username);
            return View(newUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(User editedUser)
        {
            userDAL.UpdateUser(editedUser);
            return RedirectToAction("UserList", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePass(User editedUser)
        {
            User newUser = userDAL.GetUserWithDapper(editedUser.Username);
            var hashProvider = new HashProvider();
            var hashedPassword = hashProvider.HashPassword(editedUser.Password);
            var user = new User
            {
                Password = hashedPassword.Password,
                Salt = hashedPassword.Salt,
                Username = newUser.Username,
                Role = newUser.Role,
                GoalType = newUser.GoalType,
                GoalReps = newUser.GoalReps,
                Id = newUser.Id

            };
            userDAL.UpdateUser(user);
            return RedirectToAction("UserList", "Account");
        }
        
        public IActionResult ChangePassword(User editedUser)
        {
            return View(userDAL.GetUser(editedUser.Username));
       }
        //this method is for a User changing their own password
          [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePasswordSelf(User model)
        {
            Message message = null;
            model.Password = model.Password == null ? "" : model.Password;
            model.NewPassword = model.NewPassword == null ? "" : model.NewPassword;

            bool successfulChange = authProvider.ChangePassword(model.Password, model.NewPassword);

            if (successfulChange)
            {
                message = new Message("Password Changed Successfully");
            }
            else
            {
                message = new Message("Password Not Changed");
            }

            return RedirectToAction("Feedback", message);
        }
        [HttpGet]
        public IActionResult Feedback(Message model)
        {
            return View(model);
        }
    }
}