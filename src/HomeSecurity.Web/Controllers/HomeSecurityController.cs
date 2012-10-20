using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeSecurity.Web.Controllers
{
    public class HomeSecurityController : Controller
    {
        //
        // GET: /HomeSecurity/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }
    }
}
