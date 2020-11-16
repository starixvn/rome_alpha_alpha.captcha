namespace Alpha.Captcha.Mvc
{
    using System;
    using System.Web.Mvc;
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Resources;
    using Alpha.Captcha.Services;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CaptchaValidationAttribute : ActionFilterAttribute
    {
        private readonly ISessionService sessionService;

        public CaptchaValidationAttribute()
        {
            sessionService = DependencyResolver.Current.GetService(typeof(ISessionService)) as ISessionService ?? new SessionService();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var inputCaptcha = filterContext.HttpContext.Request.Form[Configuration.CaptchaInputName];
            if (string.IsNullOrEmpty(inputCaptcha))
            {
                CaptchaValidationFailed(filterContext);
                return;
            }

            var captchaSession = sessionService.Take();
            if (captchaSession != null && inputCaptcha.Equals(captchaSession.Text, StringComparison.InvariantCultureIgnoreCase))
            {
                CaptchaValidationPassed(filterContext, captchaSession);
            }
            else
            {
                CaptchaValidationFailed(filterContext);
            }

            base.OnActionExecuting(filterContext);
        }

        private void CaptchaValidationFailed(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            modelState.AddModelError(Configuration.CaptchaInputName, Lang.ValidateErrorMessage);
        }

        private void CaptchaValidationPassed(ActionExecutingContext filterContext, CaptchaSession captchaSession)
        {
            // Method intentionally left empty.
        }
    }
}