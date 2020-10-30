namespace Alpha.Captcha.Mvc
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Web;
    using System.Web.Mvc;
    using Alpha.Captcha.Helpers;
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Services;

    public class CaptchaController : Controller
    {
        private static readonly string scriptResource = ResourceHelpers.ReadString("Alpha.Captcha.Resources.captcha.min.js");
        private static readonly string stylesheetResource = ResourceHelpers.ReadString("Alpha.Captcha.Resources.captcha.min.css");
        private readonly ICaptchaService captchaService;
        private readonly ISessionService sessionService;

        public CaptchaController()
        {
            captchaService = DependencyResolver.Current.GetService(typeof(ICaptchaService)) as ICaptchaService ?? new CaptchaService();
            sessionService = DependencyResolver.Current.GetService(typeof(ISessionService)) as ISessionService ?? new SessionService();
        }

        [HttpGet]
        public void Image()
        {
            var captchaSession = sessionService.Get();
            if (captchaSession == null)
            {
                captchaSession = sessionService.Create();
            }
            else
            {
                var isRefresh = !string.IsNullOrEmpty(HttpContext.Request.QueryString["t"]);
                if (isRefresh)
                {
                    var captchaSetting = new CaptchaSetting
                    {
                        FunctionName = captchaSession.FunctionName,
                        CaptchaLevel = captchaSession.CaptchaLevel
                    };
                    captchaSession = sessionService.Create(captchaSetting);
                }
            }

            var captchaImage = captchaService.Generate(captchaSession.Text, captchaSession.CaptchaLevel);
            WriteToResponse(Response, captchaImage);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        [HttpGet]
        [OutputCache(Duration = int.MaxValue)]
        public ActionResult Stylesheet()
        {
            return Content(stylesheetResource, "text/css");
        }

        [HttpGet]
        [OutputCache(Duration = int.MaxValue)]
        public JavaScriptResult Script()
        {
            return new JavaScriptResult
            {
                Script = scriptResource
            };
        }

        private void WriteToResponse(HttpResponseBase response, Image image)
        {
            image.Save(response.OutputStream, ImageFormat.Jpeg);
            response.ContentType = "image/jpeg";
            response.StatusCode = 200;
        }
    }
}