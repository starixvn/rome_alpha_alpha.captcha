namespace Alpha.Captcha.Mvc
{
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CaptchaValidationAttribute : ActionFilterAttribute
    {
        public CaptchaValidationAttribute()
            :this(Configuration.CaptchaInputName, Configuration.CaptchaValidationErrorMessage)
        {
        }

        public CaptchaValidationAttribute(string errorMessage)
            : this(Configuration.CaptchaInputName, errorMessage)
        {
        }

        public CaptchaValidationAttribute(string inputName, string errorMessage)
        {
            InputName = inputName;
            ErrorMessage = errorMessage;
        }

        public string InputName { get; }

        public string ErrorMessage { get; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var inputCaptcha = filterContext.HttpContext.Request.Form[Configuration.CaptchaInputName];
            if (string.IsNullOrEmpty(inputCaptcha))
            {
                CaptchaValidationFailed(filterContext);
            }
            else
            {
                var sessionCaptcha = filterContext.HttpContext.Session[Configuration.CaptchaSessionValue];
                if (sessionCaptcha != null && inputCaptcha.Equals(sessionCaptcha.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    CaptchaValidationPassed(filterContext);
                }
                else
                {
                    CaptchaValidationFailed(filterContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private void CaptchaValidationFailed(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            modelState.AddModelError(InputName, ErrorMessage);
        }

        private void CaptchaValidationPassed(ActionExecutingContext filterContext)
        {

        }
    }
}