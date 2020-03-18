namespace Alpha.Captcha.Mvc
{
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Resources;
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CaptchaValidationAttribute : ActionFilterAttribute
    {
        public CaptchaValidationAttribute()
            :this(Lang.ValidateErrorMessage)
        {
        }

        public CaptchaValidationAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var inputCaptcha = filterContext.HttpContext.Request.Form[Configuration.CaptchaInputName];
            var functionName = filterContext.HttpContext.Request.Form[Configuration.CaptchaFunctionName];
            if (string.IsNullOrEmpty(inputCaptcha))
            {
                CaptchaValidationFailed(filterContext);
                return;
            }

            var sessionKey = Configuration.GetCaptchaSessionKey(functionName);
            var captchaSession = (CaptchaSession)filterContext.HttpContext.Session[sessionKey];
            if (captchaSession != null && inputCaptcha.Equals(captchaSession.Text, StringComparison.InvariantCultureIgnoreCase))
            {
                CaptchaValidationPassed(filterContext);
            }
            else
            {
                CaptchaValidationFailed(filterContext);
            }

            filterContext.HttpContext.Session[sessionKey] = null;
            base.OnActionExecuting(filterContext);
        }

        private void CaptchaValidationFailed(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            modelState.AddModelError(Configuration.CaptchaInputName, ErrorMessage);
        }

        private void CaptchaValidationPassed(ActionExecutingContext filterContext)
        {
        }
    }
}