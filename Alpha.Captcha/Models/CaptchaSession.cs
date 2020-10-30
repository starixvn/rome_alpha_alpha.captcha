namespace Alpha.Captcha.Models
{
    using System;

    [Serializable]
    public class CaptchaSession
    {
        public CaptchaSession(string functionName, string text, CaptchaLevels captchaLevel)
        {
            FunctionName = functionName;
            Text = text;
            CaptchaLevel = captchaLevel;
        }

        public string FunctionName { get; private set; }

        public string Text { get; private set; }

        public CaptchaLevels CaptchaLevel { get; private set; }
    }
}