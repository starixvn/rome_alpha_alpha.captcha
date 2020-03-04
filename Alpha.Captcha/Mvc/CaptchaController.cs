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
        private readonly ICaptchaService captchaService;

        public CaptchaController()
            : this(new CaptchaService())
        {
        }

        private CaptchaController(ICaptchaService captchaService)
        {
            this.captchaService = captchaService;
            scriptResource = ResourceHelpers.ReadString("Alpha.Captcha.Resources.captcha.min.js");
        }

        [HttpGet]
        public void Image()
        {
            var captchaLevelObject = Session[Configuration.CaptchaSessionLevel];
            var captchaLevel = captchaLevelObject == null ? CaptchaLevels.Extreme : (CaptchaLevels)captchaLevelObject;
            var captchaResult = captchaService.Generate(captchaLevel);
            WriteToSession(HttpContext.Session, captchaResult.Text);
            WriteToResponse(Response, captchaResult.Image);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        [OutputCache(Duration = int.MaxValue)]
        public ActionResult RefreshIcon()
        {
            return File(ResourceHelpers.ReadStream("Alpha.Captcha.Resources.refresh-icon.svg"), "image/svg+xml; charset=utf-8");
        }

        [OutputCache(Duration = int.MaxValue)]
        public ActionResult StyleSheet()
        {
            return Content(ResourceHelpers.ReadString("Alpha.Captcha.Resources.jquery.captcha.css"), "text/css");
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

        private void WriteToSession(HttpSessionStateBase session, string text)
        {
            Session[Configuration.CaptchaSessionValue] = text;
        }

        private void WriteToResponse(HttpResponseBase response, Image image)
        {
            image.Save(response.OutputStream, ImageFormat.Jpeg);
            response.ContentType = "image/jpeg";
            response.StatusCode = 200;
        }
    }
}