namespace Alpha.Captcha.Services
{
    using Alpha.Captcha.Models;
    using System.Web;

    public interface ISessionService
    {
        CaptchaSession Create(CaptchaSetting setting);

        string GetSessionKey(string functionName);
    }

    public class SessionService : ISessionService
    {
        private static readonly string CaptchaSessionKey = "Alpha.Captcha";
        private readonly ICaptchaService captchaService;

        public SessionService(ICaptchaService captchaService)
        {
            this.captchaService = captchaService;
        }

        public string GetSessionKey(string functionName)
        {
            if (string.IsNullOrEmpty(functionName))
            {
                return CaptchaSessionKey;
            }

            return $"{CaptchaSessionKey}_{functionName.ToLowerInvariant()}";
        }

        public CaptchaSession Create(CaptchaSetting setting)
        {
            var session = HttpContext.Current.Session;
            var captchaText = captchaService.RandomText(CaptchaProfiles.DefaultTextOptions);
            var captchaSession = new CaptchaSession(setting.FunctionName, captchaText, setting.Level);
            var sessionKey = GetSessionKey(setting.FunctionName);
            session[sessionKey] = captchaSession;
            return captchaSession;
        }
    }
}