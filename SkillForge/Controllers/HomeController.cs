﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillForge.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

          public ActionResult Login()
          {
               return View();
          }

        public ActionResult NewCourses()
        {
            return View();
        }

     }
}