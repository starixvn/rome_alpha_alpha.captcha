namespace Alpha.Captcha.Mvc
{
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Resources;
    using Alpha.Captcha.Services;
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CaptchaValidationAttribute : ActionFilterAttribute
    {
        private readonly ISessionService sessionService;

        public CaptchaValidationAttribute()
            :this(Configuration.Resolve<ISessionService>())
        {
        }

        public CaptchaValidationAttribute(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var inputCaptcha = filterContext.HttpContext.Request.Form[Configuration.CaptchaInputName];
            var functionName = filterContext.HttpContext.Request.Form[Configuration.CaptchaFunctionName];
            if (string.IsNullOrEmpty(inputCaptcha))
            {
                CaptchaValidationFailed(filterContext);
                return;
            }

            var sessionKey = sessionService.GetSessionKey(functionName);
            var captchaSession = (CaptchaSession)filterContext.HttpContext.Session[sessionKey];
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
            var sessionKey = sessionService.GetSessionKey(captchaSession.FunctionName);
            filterContext.HttpContext.Session[sessionKey] = null;
        }
    }
}