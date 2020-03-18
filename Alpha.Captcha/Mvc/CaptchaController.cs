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

        public CaptchaController()
            : this(new CaptchaService())
        {
        }

        private CaptchaController(ICaptchaService captchaService)
        {
            this.captchaService = captchaService;
            scriptResource = ResourceHelpers.ReadString("Alpha.Captcha.Resources.captcha.min.js");
            stylesheetResource = ResourceHelpers.ReadString("Alpha.Captcha.Resources.captcha.min.css");
        }

        [HttpGet]
        public void Image()
        {
            var captchaText = "ERROR";
            var captchaLevel = CaptchaLevels.Low;
            var functionName = HttpContext.Request.QueryString[Configuration.CaptchaFunctionName];
            var isRefresh = !string.IsNullOrEmpty(HttpContext.Request.QueryString["t"]);
            var sessionKey = Configuration.GetCaptchaSessionKey(functionName);
            var captchaSession = (CaptchaSession)Session[sessionKey];
            if(captchaSession != null && isRefresh)
            {
                var captchaService = new CaptchaService();
                captchaText = captchaService.RandomText(CaptchaProfiles.DefaultTextOptions);
                captchaSession = new CaptchaSession(captchaSession.FunctionName, captchaText, captchaSession.Level);
                Session[sessionKey] = captchaSession;
            }

            if (captchaSession != null)
            {
                captchaText = captchaSession.Text;
                captchaLevel = captchaSession.Level;
            }

            var captchaImage = captchaService.Generate(captchaText, captchaLevel);
            WriteToResponse(Response, captchaImage);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
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