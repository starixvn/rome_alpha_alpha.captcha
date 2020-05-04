namespace Alpha.Captcha.WebSample.Controllers
{
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Mvc;
    using Alpha.Captcha.WebSample.Models;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
#if DEBUG
            ViewData["IsDevelopment"] = true;
#else
            ViewData["IsDevelopment"] = false;
#endif

            var lang = filterContext.HttpContext.Request.QueryString["lang"];
            if (!string.IsNullOrWhiteSpace(lang))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }

            base.OnActionExecuted(filterContext);
        }

        [HttpGet]
        public ActionResult Index()
        {
            CreateIndexCaptchaSetting();
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        [CaptchaValidation]
        public ActionResult PostIndex()
        {
            CreateIndexCaptchaSetting();
            return View();
        }

        [HttpGet]
        public ActionResult Feedback()
        {
            CreateFeedbackCaptchaSetting();
            return View();
        }

        [HttpPost]
        [ActionName("Feedback")]
        [CaptchaValidation]
        public ActionResult PostFeedback(Feedback feedback)
        {
            CreateFeedbackCaptchaSetting();
            return View();
        }

        private void CreateIndexCaptchaSetting()
        {
            var captchaSetting = new CaptchaSetting()
            {
                Level = CaptchaLevels.Low,
                ButtonClassname = "btn-submit"
            };
            ViewData["CaptchaSetting"] = captchaSetting;
        }

        private void CreateFeedbackCaptchaSetting()
        {
            var captchaSetting = new CaptchaSetting
            {
                FunctionName = "high",
                Level = CaptchaLevels.High,
                IsForm = false,
                IsShowButton = false
            };
            ViewData["CaptchaSetting"] = captchaSetting;
        }

        [HttpGet]
        public ActionResult Ajax()
        {
            CreateAjaxCaptchaSetting();
            return View();
        }

        [HttpPost]
        [ActionName("Ajax")]
        [CaptchaValidation]
        public ActionResult AjaxIndex()
        {
            if (ModelState.IsValid)
            {
                return new HttpStatusCodeResult(200, "Valid code");
            }

            return new HttpStatusCodeResult(500, "Invalid code");
        }

        private void CreateAjaxCaptchaSetting()
        {
            var captchaSetting = new CaptchaSetting()
            {
                FunctionName = "ajax",
                Level = CaptchaLevels.Low,
                IsForm = false
            };
            ViewData["CaptchaSetting"] = captchaSetting;
        }
    }
}