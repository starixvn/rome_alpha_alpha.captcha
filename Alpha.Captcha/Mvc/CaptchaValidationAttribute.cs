namespace Alpha.Captcha.Mvc
{
    using System;
    using System.Web.Mvc;
    using Alpha.Captcha.Resources;
    using Alpha.Captcha.Services;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CaptchaValidationAttribute : ActionFilterAttribute
    {
        private readonly ISessionService sessionService;
        private readonly bool forceRemove;

        public CaptchaValidationAttribute()
            : this(false)
        {
        }

        public CaptchaValidationAttribute(bool forceRemove)
        {
            this.forceRemove = forceRemove;
            sessionService = DependencyResolver.Current.GetService(typeof(ISessionService)) as ISessionService ?? new SessionService();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var inputCaptcha = filterContext.HttpContext.Request.Form[Configuration.CaptchaInputName];
            if (string.IsNullOrEmpty(inputCaptcha))
            {
                CaptchaValidationFailed(filterContext, 1, Lang.CaptchaIsEmptyErrorMessage);
                return;
            }

            var captchaSession = sessionService.Get();
            if (captchaSession == null)
            {
                CaptchaValidationFailed(filterContext, 2, Lang.CaptchaIsNullErrorMessage);
                return;
            }

            if (!inputCaptcha.Equals(captchaSession.Text, StringComparison.OrdinalIgnoreCase))
            {
                CaptchaValidationFailed(filterContext, 3, string.Format(Lang.ValidateErrorMessage, inputCaptcha.ToUpperInvariant(), captchaSession.Text));
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private void CaptchaValidationFailed(ActionExecutingContext filterContext, int errorCode, string message)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            modelState.AddModelError($"{Configuration.CaptchaInputName}_{errorCode}", message);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            if (forceRemove || modelState.IsValid)
            {
                sessionService.Remove();
            }

            base.OnResultExecuted(filterContext);
        }
    }
}