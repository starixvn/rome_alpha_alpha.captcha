namespace Alpha.Captcha.Services
{
    using System.Web;
    using System.Web.Mvc;
    using Alpha.Captcha.Models;

    public interface ISessionService
    {
        CaptchaSession Create(CaptchaSetting captchaSetting);

        CaptchaSession Create();

        CaptchaSession Get();

        CaptchaSession Take();

        void Remove();
    }

    public class SessionService : ISessionService
    {
        private static readonly string CaptchaSessionKey = "Alpha.Captcha";
        private readonly IRandomService randomService;

        public SessionService()
        {
            randomService = DependencyResolver.Current.GetService(typeof(IRandomService)) as IRandomService ?? new RandomService();
        }

        public CaptchaSession Create(CaptchaSetting captchaSetting)
        {
            var httpContext = HttpContext.Current;
            var captchaText = randomService.RandomText(CaptchaProfiles.DefaultTextOptions);
            var captchaSession = new CaptchaSession(captchaSetting.FunctionName, captchaText, captchaSetting.CaptchaLevel);
            var sessionKey = GetSessionKey(captchaSetting.FunctionName);
            httpContext.Session[sessionKey] = captchaSession;
            return captchaSession;
        }

        public CaptchaSession Create()
        {
            var httpContext = HttpContext.Current;
            var functionName = GetFunctionName(httpContext.Request);
            var captchaSetting = new CaptchaSetting
            {
                FunctionName = functionName
            };
            return Create(captchaSetting);
        }

        public CaptchaSession Get()
        {
            var httpContext = HttpContext.Current;
            var sessionKey = GetSessionKey(httpContext.Request);
            return (CaptchaSession)httpContext.Session[sessionKey];
        }

        public CaptchaSession Take()
        {
            var httpContext = HttpContext.Current;
            var sessionKey = GetSessionKey(httpContext.Request);
            var captchaSession = (CaptchaSession)httpContext.Session[sessionKey];
            httpContext.Session[sessionKey] = null;
            return captchaSession;
        }

        public void Remove()
        {
            var httpContext = HttpContext.Current;
            var sessionKey = GetSessionKey(httpContext.Request);
            httpContext.Session[sessionKey] = null;
        }

        private static string GetFunctionName(HttpRequest httpRequest)
        {
            return httpRequest.QueryString[Configuration.CaptchaFunctionName] ?? httpRequest.Form[Configuration.CaptchaFunctionName];
        }

        private static string GetSessionKey(HttpRequest httpRequest)
        {
            var functionName = GetFunctionName(httpRequest);
            return GetSessionKey(functionName);
        }

        private static string GetSessionKey(string functionName)
        {
            return string.IsNullOrEmpty(functionName) ? CaptchaSessionKey : $"{CaptchaSessionKey}_{functionName.ToLowerInvariant()}";
        }
    }
}