using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc2v40.Controllers {
  [HandleError]
  public class HomeController : Controller {
    public ActionResult Index() {
      ViewData["Message"] = "Willkommen bei ASP.NET MVC";

      return View();
    }

    public ActionResult About() {
      return View();
    }
  }
}
