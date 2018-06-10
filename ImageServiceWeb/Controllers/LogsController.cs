using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class LogsController : Controller
    {
        static LogsModel model = new LogsModel();

        public LogsController()
        {
            model.Update -= Update;
            model.Update += Update;
        }

        private void Update(Object sender, EventArgs args)
        {
            Logs();
        }

        // GET: Logs
        public ActionResult Logs()
        {
            return View(model);
        }


    }
}