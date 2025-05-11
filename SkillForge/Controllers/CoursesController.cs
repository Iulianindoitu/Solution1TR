using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillForge.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Index()
        {
            return View();
        }

        // GET: Courses/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Courses/Category/5
        public ActionResult Category(int id)
        {
            return View();
        }
    }
} 