namespace Alpha.Captcha
{
    using System.Web;
    using System.Web.Mvc;

    public class Configuration
    {
        public static string CaptchaSessionValue = "Alpha.Captcha.Value";
        public static string CaptchaSessionLevel = "Alpha.Captcha.Level";
        public static string CaptchaInputName = "Captcha";
        public static string CaptchaStylesheet
        {
            get
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                return urlHelper.Action("Stylesheet", "Captcha");
            }
        }
    }
}