// --------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="XxxAM">
//   Copyright (c) XxxAM. All rights reserved.
// </copyright>
// <summary>
//   Defines the AccountController type.
// </summary>
// --------------------------------------------------------------------------

namespace Mvc2v35.Controllers {

  using System.Web.Mvc;
  using System.Web.Routing;
  using System.Web.Security;

  using Mvc2v35.Models;

  /// <summary>The account controller.</summary>
  [HandleError]
  public class AccountController : Controller {

    /// <summary>Gets or sets the forms service.</summary>
    public IFormsAuthenticationService FormsService { get; set; }

    /// <summary>Gets or sets the membership service.</summary>
    public IMembershipService MembershipService { get; set; }

    // **************************************
    // URL: /Account/LogOn
    // **************************************

    /// <summary>The log on.</summary>
    /// <returns>The <see cref="ActionResult"/>.</returns>
    public ActionResult LogOn() {
      return this.View();
    }

    /// <summary>The log on.</summary>
    /// <param name="model">The model.</param>
    /// <param name="returnUrl">The return url.</param>
    /// <returns>The <see cref="ActionResult"/>.</returns>
    [HttpPost]
    public ActionResult LogOn(LogOnModel model, string returnUrl) {
      if (!this.ModelState.IsValid) {
        return this.View(model);
      }

      if (this.MembershipService.ValidateUser(
        model.UserName, 
        model.Password)) {
          
        this.FormsService.SignIn(model.UserName, model.RememberMe);
        if (!string.IsNullOrEmpty(returnUrl)) {
          return this.Redirect(returnUrl);
        }

        return this.RedirectToAction("Index", "Home");
      }

      const string S = "Der angegebene Benutzername oder das angegebene " +
          "Kennwort ist ungültig.";
      this.ModelState.AddModelError(string.Empty, S);

      // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; 
      // Formular erneut anzeigen.
      return this.View(model);
    }

    // **************************************
    // URL: /Account/LogOff
    // **************************************

    /// <summary>The log off.</summary>
    /// <returns>The <see cref="ActionResult"/>.</returns>
    public ActionResult LogOff() {
      this.FormsService.SignOut();

      return this.RedirectToAction("Index", "Home");
    }

    // **************************************
    // URL: /Account/Register
    // **************************************

    /// <summary>The register.</summary>
    /// <returns>The <see cref="ActionResult"/>.</returns>
    public ActionResult Register() {
      this.ViewData["PasswordLength"] = this.MembershipService
          .MinPasswordLength;
      return this.View();
    }

    /// <summary>The register.</summary>
    /// <param name="model">The model.</param>
    /// <returns>The <see cref="ActionResult"/>.</returns>
    [HttpPost]
    public ActionResult Register(RegisterModel model) {
      if (ModelState.IsValid) {
        // Versuch, den Benutzer zu registrieren
        var createStatus = this.MembershipService
            .CreateUser(model.UserName, model.Password, model.Email);

        if (createStatus == MembershipCreateStatus.Success) {
          this.FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
          return this.RedirectToAction("Index", "Home");
        }

        this.ModelState.AddModelError(
            string.Empty, 
            AccountValidation.ErrorCodeToString(createStatus));
      }

      // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; 
      // Formular erneut anzeigen.
      this.ViewData["PasswordLength"] = this.MembershipService
          .MinPasswordLength;
      return this.View(model);
    }

    // **************************************
    // URL: /Account/ChangePassword
    // **************************************

    /// <summary>The change password.</summary>
    /// <returns>The <see cref="ActionResult"/>.</returns>
    [Authorize]
    public ActionResult ChangePassword() {
      this.ViewData["PasswordLength"] = this.MembershipService
          .MinPasswordLength;
      return this.View();
    }

    /// <summary>The change password.</summary>
    /// <param name="model">The model.</param>
    /// <returns>The <see cref="ActionResult"/>.</returns>
    [Authorize]
    [HttpPost]
    public ActionResult ChangePassword(ChangePasswordModel model) {
      if (ModelState.IsValid) {
        if (this.MembershipService.ChangePassword(
              User.Identity.Name, 
              model.OldPassword, 
              model.NewPassword)) {
          return this.RedirectToAction("ChangePasswordSuccess");
        }

        const string S = "Das aktuelle Kennwort ist nicht korrekt, oder " +
            "das Kennwort ist ungültig.";
        this.ModelState.AddModelError(string.Empty, S);
      }

      // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; Formular 
      // erneut anzeigen.
      this.ViewData["PasswordLength"] = 
          this.MembershipService.MinPasswordLength;
      return this.View(model);
    }

    // **************************************
    // URL: /Account/ChangePasswordSuccess
    // **************************************

    /// <summary>The change password success.</summary>
    /// <returns>The <see cref="ActionResult"/>.</returns>
    public ActionResult ChangePasswordSuccess() {
      return this.View();
    }

    /// <summary>The initialize.</summary>
    /// <param name="requestContext">The request context.</param>
    protected override void Initialize(RequestContext requestContext) {
      if (this.FormsService == null) {
        this.FormsService = new FormsAuthenticationService();
      }

      if (this.MembershipService == null) {
        this.MembershipService = new AccountMembershipService();
      }

      base.Initialize(requestContext);
    }
  }
}
