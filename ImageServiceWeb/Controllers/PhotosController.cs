using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class PhotosController : Controller
    {
        static PhotosModel photosModel = new PhotosModel();

        public PhotosController()
        {
            photosModel.Update -= Update;
            photosModel.Update += Update;
        }

        private void Update(Object sender, EventArgs args)
        {
            Photos();
        }

        // GET: Photos
        public ActionResult Photos()
        {
            photosModel.CheckUpdate();
            return View(photosModel.Photos);
        }

        // GET: Photos
        public ActionResult ViewPhoto(string path)
        {
            return View(photosModel.PathToPhoto(path));
        }

        // GET: Photos
        public ActionResult DeletePhoto(string path)
        {
            photosModel.SelectedPhoto = path;
            ViewBag.path = Path.GetFileName(path);
            return View(photosModel);
        }

        [HttpPost]
        public ActionResult DeletePhotoOK()
        {
            photosModel.DeletePhoto();
            return RedirectToAction("Photos");
        }
    }
}