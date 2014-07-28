// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="XxxAM">
//   Copyright (c) XxxAM. All rights reserved.
// </copyright>
// <summary>
//   The home controller.
// </summary>
// --------------------------------------------------------------------------

namespace Mvc2v35.Controllers {
  using System.Web.Mvc;

  /// <summary>The home controller.</summary>
  [HandleError]
  public class HomeController : Controller {

    /// <summary>The index.</summary>
    /// <returns>The <see cref="ActionResult"/>.</returns>
    public ActionResult Index() {
      this.ViewData["Message"] = "Willkommen bei ASP.NET MVC";

      return this.View();
    }

    /// <summary>The about.</summary>
    /// <returns>The <see cref="ActionResult"/>.</returns>
    public ActionResult About() {
      return this.View();
    }
  }
}
