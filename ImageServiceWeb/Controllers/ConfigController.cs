using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ConfigController : Controller
    {
        private static string selectedHandler;
        private static ConfigModel configModel = new ConfigModel();

        private void Update(Object sender, EventArgs args)
        {
            Config();
        }

        public ConfigController()
        {
            configModel.Update -= Update;
            configModel.Update += Update;
        }


        public ActionResult Config()
        {
            return View(configModel);
        }

        public ActionResult DeleteHandler(string handler)
        {
            configModel.SelectedHandler = handler;
            return RedirectToAction("Confirm");
        }

        [HttpPost]
        public ActionResult DeleteHandlerOK(string handler)
        {
            configModel.CloseHandler();
            return RedirectToAction("Config");
        }

        // GET: Confirm
        public ActionResult Confirm()
        {
            return View(configModel);
        }
    }
}