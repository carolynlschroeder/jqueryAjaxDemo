using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jQueryAjaxDemo.BusinessLogic;
using jQueryAjaxDemo.Entities;
using jQueryAjaxDemo.Models;

namespace jQueryAjaxDemo.Controllers
{
    public class GalleryController : Controller
    {
        private jQueryAjaxDemoEntities _context = new jQueryAjaxDemoEntities();

        public ActionResult Index()
        {
            var images = AzureMethods.GetBlobs().OrderByDescending(b => b.LastModifiedDate);
            var model = new List<ImageModel>();
            foreach (var i in images)
            {
                var m = new ImageModel();
                m.ImageUri = i.BlobImageUri;
                m.ImageName = i.BlobImageName;
                var image = _context.Images.FirstOrDefault(img => img.ImageName == i.BlobImageName);
                m.ImageId = image.ImageId;
                m.TotalLikes = image.UserLikes.Count;
                model.Add(m);
            }
            return View(model);
        }

        public ActionResult AddImage()
        {
            return View();
        }

        public JsonResult SaveFiles()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0)
                {
                    SaveFile(file);
                }
            }

            return Json(new {Message = string.Empty}, JsonRequestBehavior.AllowGet);
        }

        private void SaveFile(HttpPostedFileBase file)
        {

            var name = Guid.NewGuid() + BusinessLogic.BusinessMethods.GetFileExtension(file.FileName);
            var fileData = BusinessMethods.FileToBytes(file);
            AzureMethods.AddToAzureStorage(fileData, name);

            var image = new Image
            {
                ImageId = Guid.NewGuid(),
                ImageName = name
            };
            _context.Images.Add(image);
            _context.SaveChanges();
        }

        public JsonResult AddLike(string userId, Guid imageId)
        {
            var userLike = new UserLike
            {
                Id = userId,
                ImageId = imageId
            };
            _context.UserLikes.Add(userLike);
            _context.SaveChanges();

            return Json(new { Message = string.Empty }, JsonRequestBehavior.AllowGet);
        }

    }

}