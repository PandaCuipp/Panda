using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panda.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            string username = Request.Form["username"];
            string password = Request.Form["password"];
            string iconPath = string.Empty;
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                fileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssfff"), "_", file.FileName);
                string filePath = Server.MapPath("~/Upload/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                iconPath = string.Concat(filePath, fileName);
                file.SaveAs(iconPath);
            }

            return Json(new { success = true, name = fileName });
        }
    }
}