namespace Alpha.Captcha.Mvc
{
    using Alpha.Captcha.Helpers;
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Services;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Web;
    using System.Web.Mvc;

    public class CaptchaController : Controller
    {
        private static string scriptResource;
        private static string stylesheetResource;
        private readonly ICaptchaService captchaService;
        private readonly ISessionService sessionService;

        public CaptchaController()
            : this(Configuration.Resolve<ICaptchaService>(), Configuration.Resolve<ISessionService>())
        {
        }

        private CaptchaController(ICaptchaService captchaService, ISessionService sessionService)
        {
            this.captchaService = captchaService;
            this.sessionService = sessionService;
            scriptResource = ResourceHelpers.ReadString("Alpha.Captcha.Resources.captcha.min.js");
            stylesheetResource = ResourceHelpers.ReadString("Alpha.Captcha.Resources.captcha.min.css");
        }

        [HttpGet]
        public void Image()
        {
            var functionName = HttpContext.Request.QueryString[Configuration.CaptchaFunctionName];
            var sessionKey = sessionService.GetSessionKey(functionName);
            var captchaSession = (CaptchaSession)Session[sessionKey];
            if(captchaSession == null)
            {
                var captchaSetting = new CaptchaSetting
                {
                    Level = CaptchaLevels.Extreme
                };
                captchaSession = sessionService.Create(captchaSetting);
            }
            else
            {
                var isRefresh = !string.IsNullOrEmpty(HttpContext.Request.QueryString["t"]);
                if (isRefresh)
                {
                    var captchaSetting = new CaptchaSetting
                    {
                        FunctionName = captchaSession.FunctionName,
                        Level = captchaSession.Level
                    };
                    captchaSession = sessionService.Create(captchaSetting);
                }
            }
            
            var captchaImage = captchaService.Generate(captchaSession.Text, captchaSession.Level);
            WriteToResponse(Response, captchaImage);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        private CaptchaSession GenerateNewCaptchaSession(string functionName, CaptchaLevels level)
        {
            var captchaText = captchaService.RandomText(CaptchaProfiles.DefaultTextOptions);
            return new CaptchaSession(functionName, captchaText, level);
        }

        [HttpGet]
        [OutputCache(Duration = int.MaxValue)]
        public ActionResult Stylesheet()
        {
#if DEBUG
            return Content(string.Empty, "text/css");
#else
            return Content(stylesheetResource, "text/css");
#endif
        }

        [HttpGet]
        [OutputCache(Duration = int.MaxValue)]
        public JavaScriptResult Script()
        {
            return new JavaScriptResult
            {
#if DEBUG
                Script = string.Empty
#else
                Script = scriptResource
#endif
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