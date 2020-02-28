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
        private readonly CaptchaService captchaService;

        public CaptchaController()
            : this(new CaptchaOptions())
        {
        }

        public CaptchaController(CaptchaOptions captchaOptions)
            : this(new CaptchaServiceImp(captchaOptions))
        {
        }

        public CaptchaController(CaptchaService captchaService)
        {
            this.captchaService = captchaService;
            scriptResource = ResourceHelpers.LoadEmbeddedResource("Alpha.Captcha.Resources.jquery.captcha.min.js");
        }

        [HttpGet]
        public void Image()
        {
            var captchaResult = captchaService.Generate();
            WriteToSession(HttpContext.Session, captchaResult.Text);
            WriteToResponse(Response, captchaResult.Image);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
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
            Session[Configuration.CaptchaSessionKey] = text;
        }

        private void WriteToResponse(HttpResponseBase response, Image image)
        {
            image.Save(response.OutputStream, ImageFormat.Jpeg);
            response.ContentType = "image/jpeg";
            response.StatusCode = 200;
        }
    }
}