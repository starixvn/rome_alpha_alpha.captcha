namespace Alpha.Captcha.App_Start
{
    using Alpha.Captcha.Services;
    using SimpleInjector;

    public static class CaptchaServicesRegistration
    {
        public static void RegisterAlphaCaptchaMvc(this Container container)
        {
            container.Register<IRandomService, RandomService>(Lifestyle.Transient);
            container.Register<ICaptchaService, CaptchaService>(Lifestyle.Transient);
            container.Register<ISessionService, SessionService>(Lifestyle.Transient);
        }
    }
}
