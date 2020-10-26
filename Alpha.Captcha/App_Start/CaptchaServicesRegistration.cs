using Alpha.Captcha.Services;
using SimpleInjector;

namespace Alpha.Captcha.App_Start
{
    public static class CaptchaServicesRegistration
    {
        public static void Register(this Container container)
        {
            container.Register<ICaptchaService, CaptchaService>(Lifestyle.Transient);
            container.Register<ISessionService, SessionService>(Lifestyle.Transient);
        }
    }
}
