﻿using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KardelenMFAServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            if (fc["username"] == "Admin" && fc["password"] == "Admin123")
            {
                Session["tempid"] = fc["username"];
                return RedirectToAction("VerifyAuth");
            }
            else
            {
                TempData["msg"] = "Admin id or Passowrd is wrong...!";
                return View();
            }
            
        }
        public ActionResult VerifyAuth()
        {
            if (Session["tempid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
            
           
           
        }
        string key = "test987@76";
        [HttpPost]
        public ActionResult VerifyAuth(FormCollection fc)
        {
            var token = fc["passcode"];
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string useruniquekey = Convert.ToString(Session["tempid"])+key;
            bool isvalid = tfa.ValidateTwoFactorPIN(useruniquekey, token);
            if (isvalid)
            {
                Session["id"] = Convert.ToString(Session["tempid"]);
                return RedirectToAction("MyProfile");
            }
            return RedirectToAction("Login");
            
        }
        public ActionResult AdminQR()
        {
            if (Session["tempid"] != null)
            {
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                string UserUniqueKey = (Convert.ToString(Session["tempid"])+key);
                Session["Useruniquekey"] = UserUniqueKey;
                var setupinfo = tfa.GenerateSetupCode("Google Auth Test",UserUniqueKey,300,300);
                ViewBag.qrcode = setupinfo.QrCodeSetupImageUrl;
                ViewBag.SetupCode = setupinfo.ManualEntryKey;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
            

        }
        public ActionResult Myprofile()

        {

            if (Session["id"] != null)

            {

                return View();

            }

            else

            {

                return RedirectToAction("Login");

            }



        }

    }
}