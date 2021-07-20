namespace Alpha.Captcha.Services
{
    using System.Web;
    using System.Web.Mvc;
    using Alpha.Captcha.Models;

    public interface ISessionService
    {
        CaptchaModel Create(CaptchaSetting setting);

        CaptchaModel Create();

        CaptchaModel Get();

        string Take();

        void Remove();
    }

    public class SessionService : ISessionService
    {
        private static readonly string SessionKeyPrefix = "Alpha.Captcha";
        private static readonly string SessionKeySettingSuffix = "Setting";
        private static readonly string SessionKeyTextSuffix = "Text";
        private readonly IRandomService randomService;

        public SessionService()
        {
            randomService = DependencyResolver.Current.GetService(typeof(IRandomService)) as IRandomService ?? new RandomService();
        }

        public CaptchaModel Create(CaptchaSetting setting)
        {
            var text = randomService.RandomText(CaptchaProfiles.DefaultTextOptions);
            var captchaModel = new CaptchaModel(setting, text);
            var httpContext = HttpContext.Current;
            var sessionKey = GetSessionKey(captchaModel.Setting.FunctionName);
            httpContext.Session[sessionKey.Setting] = captchaModel.Setting;
            httpContext.Session[sessionKey.Text] = captchaModel.Text;
            return captchaModel;
        }

        public CaptchaModel Create()
        {
            var httpContext = HttpContext.Current;
            var functionName = GetFunctionName(httpContext.Request);
            var captchaSetting = new CaptchaSetting
            {
                FunctionName = functionName
            };
            return Create(captchaSetting);
        }

        public CaptchaModel Get()
        {
            var httpContext = HttpContext.Current;
            var sessionKey = GetSessionKey(httpContext.Request);
            var setting = (CaptchaSetting)httpContext.Session[sessionKey.Setting];
            var text = (string)httpContext.Session[sessionKey.Text];
            return new CaptchaModel(setting, text);
        }

        public string Take()
        {
            var httpContext = HttpContext.Current;
            var sessionKey = GetSessionKey(httpContext.Request);
            var captchaText = (string)httpContext.Session[sessionKey.Text];
            httpContext.Session[sessionKey.Text] = null;
            return captchaText;
        }

        public void Remove()
        {
            var httpContext = HttpContext.Current;
            var sessionKey = GetSessionKey(httpContext.Request);
            httpContext.Session[sessionKey.Text] = null;
        }

        private static string GetFunctionName(HttpRequest httpRequest)
        {
            return httpRequest.QueryString[Configuration.CaptchaFunctionName] ?? httpRequest.Form[Configuration.CaptchaFunctionName];
        }

        private static SessionKey GetSessionKey(HttpRequest httpRequest)
        {
            var functionName = GetFunctionName(httpRequest);
            return GetSessionKey(functionName);
        }

        private static SessionKey GetSessionKey(string functionName)
        {
            var sessionKeyPrefix = string.IsNullOrEmpty(functionName) ? SessionKeyPrefix : $"{SessionKeyPrefix}_{functionName.ToLowerInvariant()}";
            return new SessionKey($"{sessionKeyPrefix}_{SessionKeySettingSuffix}", $"{sessionKeyPrefix}_{SessionKeyTextSuffix}");
        }
    }
}