namespace Alpha.Captcha
{
    using Alpha.Captcha.Helpers;
    using Alpha.Captcha.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class Configuration
    {
        internal static readonly string CaptchaInputName = "c_v";
        internal static readonly string CaptchaFunctionName = "c_f";
        private static readonly IList<object> services = new List<object>();

        static Configuration()
        {
            var random = RandomProvider.GetThreadRandom();
            Register(random);
            Register<ICaptchaService>();
            Register<ISessionService>();
        }

        public static string CaptchaStylesheet
        {
            get
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                return $"{urlHelper.Action("Stylesheet", "Captcha")}?v={AssemblyHelpers.Version}";
            }
        }

        public static T Resolve<T>()
        {
            var service = Find<T>();
            if (service == null)
            {
                service = Create<T>();
                Register(service);
            }

            return service;
        }

        private static T Find<T>()
        {
            var type = typeof(T);
            return (T)services.FirstOrDefault(s => type == s.GetType() || type.IsAssignableFrom(s.GetType()));
        }

        private static void Register<T>()
        {
            Unregister<T>();
            var service = Create<T>();
            Register(service);
        }

        private static void Unregister<T>()
        {
            var service = Find<T>();
            if (service != null)
            {
                services.Remove(service);
            }
        }

        private static T Create<T>()
        {
            var type = typeof(T);
            if (type.IsInterface)
            {
                type = typeof(Configuration).Assembly.GetTypes().FirstOrDefault(p => p.IsClass && type.IsAssignableFrom(p));
            }

            var parameters = type.GetConstructors().First().GetParameters();
            var bindingServices = parameters.Select(p =>
            {
                return services.FirstOrDefault(s => s.GetType() == p.ParameterType || p.ParameterType.IsAssignableFrom(s.GetType()));
            }).ToArray();
            return (T)Activator.CreateInstance(type, bindingServices);
        }

        private static void Register<T>(T service)
        {
            var type = typeof(T);
            var typeName = type.FullName;
            services.Add(service);
        }
    }
}