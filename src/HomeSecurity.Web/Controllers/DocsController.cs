using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeSecurity.Web.Controllers
{
    public class DocsController : Controller
    {
        //
        // GET: /Docs/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GettingReadyForTheMeeting()
        {
            return View();
        }

        public ActionResult ExternalDoorEntry()
        {
            return View();
        }

        public ActionResult DoorBell()
        {
            return View();
        }
        public ActionResult Alarm()
        {
            return View();
        }
        public ActionResult AlarmControlPanel()
        {
            return View();
        }
        public ActionResult Garage()
        {
            return View();
        }
        public ActionResult MasterControlPanel()
        {
            return View();
        }
    }
}
