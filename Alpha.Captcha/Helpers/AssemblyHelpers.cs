namespace Alpha.Captcha.Helpers
{
    using System;
    using System.Reflection;

    public static class AssemblyHelpers
    {
        public static readonly Assembly Current = typeof(AssemblyHelpers).Assembly;
        public static readonly Version Version = Current.GetName().Version;
    }
}