using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models;
using Capstone.Web.Models.ViewModels;

namespace Capstone.Web.Controllers
{
    public class HomeController : Controller
    {
        private IDatabaseSvc _db = null;

        public HomeController(IDatabaseSvc db)
        {
            _db = db;
        }

        // GET: Home
        public ActionResult Start()
        {
            return View();
        }

        // GET: About
        public ActionResult About()
        {
            return View();
        }

        //GET: Home/Login
        public ActionResult LogIn()
        {
            LoginViewModel model = new LoginViewModel();
            if (TempData["LoginError"] != null)
            {
                model.LoginError = TempData["LoginError"] as string;
            }
            return View("LogIn", model);
        }

        //POST: Home/Login
        [HttpPost]
        public ActionResult LogIn(LoginViewModel model)
        {
            ActionResult result;

            //Validate the model before proceeding

            if (!ModelState.IsValid)
            {
                TempData["LoginError"] = "Please Fill Out All Forms";
                result = View("LogIn", model);
            }
            else
            {
                //if the model is valid, create a record with the user entered username
                UserItem record = _db.GetUserItem(model.UserName);

                //Check if user exists in database
                if (record.Id == -1)  // <-- GetUser DAL returns an id of -1 if user is not in database
                {
                    TempData["LoginError"] = $"User does not exist";
                    result = RedirectToAction("LogIn");
                }
                else
                {
                    //create new user object and generate a hash with the attempted password
                    UserItem newPh = new UserItem(model.Password, record.Salt);

                    //verify if the hashes match
                    bool isVerified = newPh.Verify(record.Password);

                    if (isVerified)
                    {
                        //TODO: add user to session data
                        //ask chris if this is the correct way to store the user model in the session
                        Session["User"] = record;

                        result = RedirectToAction("ViewDecks", "Deck");
                    }
                    else
                    {
                        //TODO: flash message saying password or username are incorrect
                        TempData["LoginError"] = "Incorrect Password";
                        result = RedirectToAction("LogIn");
                    }
                }

            }

            return result;
        }

        //GET: Home/Register
        public ActionResult Register()
        {
            RegistrationViewModel model = new RegistrationViewModel();
            if (TempData["RegistrationError"] != null)
            {
                model.RegistrationError = TempData["RegistrationError"] as string;
            }

            return View("Register", model);
        }

        //POST: Home/Register
        [HttpPost]
        public ActionResult Register(RegistrationViewModel model)
        {
            ActionResult result;

            //Validate the model 
            if (!ModelState.IsValid)
            {
                result = View("Register", model);
            }
            else
            {
                //Check if user exists
                UserItem existingUserTest = _db.GetUserItem(model.UserName);
                UserItem existingEmailTest = _db.GetUserItem(model.Email);
                if (existingUserTest.Id >= 0)
                {
                    //if (existingEmailTest.Email == model.Email)
                    //{
                    //    TempData["RegistrationError"] = $"Email {model.Email} already exists";
                    //    result = RedirectToAction("Register", "Home");
                    //}
                    TempData["RegistrationError"] = $"Username {model.UserName} already exists";
                    result = RedirectToAction("Register", "Home");
                }
                else
                {
                    UserItem user = new UserItem(model.Password)
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        Email = model.Email,
                        IsAdmin = false
                    };

                    user.Id = _db.AddUserItem(user);

                    TempData["Success"] = "Added Successfully!";
                    result = RedirectToAction("LogIn", "Home");
                }
            }

            return result;
        }

        //POST: Home/Logout
        public ActionResult Logout()
        {
            Session.Abandon();

            return View("Start");
        }
    }
}