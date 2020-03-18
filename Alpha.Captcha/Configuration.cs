namespace Alpha.Captcha
{
    using Alpha.Captcha.Helpers;
    using System.Web;
    using System.Web.Mvc;

    public class Configuration
    {
        private static readonly string CaptchaSessionKey = "Alpha.Captcha";
        public static readonly string CaptchaInputName = "v";
        public static readonly string CaptchaFunctionName = "f";

        public static string CaptchaStylesheet
        {
            get
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                return $"{urlHelper.Action("Stylesheet", "Captcha")}?v={AssemblyHelpers.Version}";
            }
        }

        public static string GetCaptchaSessionKey(string functionName)
        {
            if (string.IsNullOrEmpty(functionName))
            {
                return CaptchaSessionKey;
            }

            return $"{CaptchaSessionKey}_{functionName.ToLowerInvariant()}";
        }
    }
}