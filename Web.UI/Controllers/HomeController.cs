using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EFData;
using Entities;

namespace Web.UI.Controllers
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
        [HttpPost]
        public JsonResult UserLogin(LoginRequestVO vo)
        {
            EFData.dt_users dt = new EFData.DBHelper().GetUser(vo);
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("message", dt == null ? "登录失败" : "登录成功");
            ret.Add("success", "200");
            return Json(ret);
        }
    }
}