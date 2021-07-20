namespace Alpha.Captcha.WebSample.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Mvc;
    using Alpha.Captcha.Services;
    using Alpha.Captcha.WebSample.Models;

    public class HomeController : Controller
    {
        private readonly ISessionService sessionService;
        private readonly CaptchaSetting indexCaptchaSetting = new CaptchaSetting
        {
            Level = CaptchaLevels.Low,
            ButtonClassname = "btn-submit",
            HiddenProps = new Dictionary<string, string>
            {
                ["test"] = "123"
            }
        };
        private readonly CaptchaSetting feedbackCaptchaSetting = new CaptchaSetting
        {
            FunctionName = "high",
            Level = CaptchaLevels.High,
            IsForm = false,
            IsShowButton = false
        };
        private readonly CaptchaSetting ajaxCaptchaSetting = new CaptchaSetting
        {
            FunctionName = "ajax",
            Level = CaptchaLevels.Low,
            IsForm = false
        };

        public HomeController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var lang = filterContext.HttpContext.Request.QueryString["lang"];
            if (!string.IsNullOrWhiteSpace(lang))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
            var langList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "English",
                    Value = "en-US"
                },
                new SelectListItem
                {
                    Text = "Thailand",
                    Value = "th-TH"
                },
                new SelectListItem
                {
                    Text = "Vietnam",
                    Value = "vi-VN"
                }
            };
            langList.ForEach(item => item.Selected = item.Value == lang);
            ViewBag.Langs = langList;

            base.OnActionExecuting(filterContext);
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewData["CaptchaSetting"] = indexCaptchaSetting;
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        [CaptchaValidation]
        public ActionResult IndexPost()
        {
            ViewData["CaptchaSetting"] = indexCaptchaSetting;
            return View();
        }

        [HttpGet]
        public ActionResult Feedback()
        {
            ViewData["CaptchaSetting"] = feedbackCaptchaSetting;
            return View();
        }

        [HttpPost]
        [CaptchaValidation]
        public ActionResult Feedback(Feedback feedback)
        {
            ViewData["CaptchaSetting"] = feedbackCaptchaSetting;
            return View();
        }

        [HttpGet]
        public ActionResult Ajax()
        {
            ViewData["CaptchaSetting"] = ajaxCaptchaSetting;
            return View();
        }

        [HttpPost]
        [CaptchaValidation()]
        public ActionResult Ajax(string username)
        {
            sessionService.Create(ajaxCaptchaSetting);
            if (string.IsNullOrWhiteSpace(username))
            {
                return new HttpStatusCodeResult(500, "Username is empty");
            }

            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(500, "Invalid code");
            }

            return new HttpStatusCodeResult(200, "OK");
        }
    }
}