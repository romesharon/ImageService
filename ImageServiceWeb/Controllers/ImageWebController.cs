using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ImageWebController : Controller
    {
        static ImageWebModel imageWebModel = new ImageWebModel();

        // GET: ImageWeb
        public ActionResult Index()
        {
            return View();
        }

        void Notify()
        {
            ImageWeb();
        }

        // GET: ImageView
        public ActionResult ImageWeb()
        {
            ViewBag.NumofPics = ImageWebModel.GetNumOfPics();
            //ViewBag.IsConnected = ImageViewInfoObj.IsConnected;
            return View(imageWebModel);
        }
    }
}