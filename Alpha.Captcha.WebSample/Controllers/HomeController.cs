namespace Alpha.Captcha.WebSample.Controllers
{
    using Alpha.Captcha.Mvc;
    using Alpha.Captcha.WebSample.Models;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidation]
        public ActionResult Index(Feedback feedback)
        {
            return View();
        }
    }
}