namespace Alpha.Captcha.Models
{
    public class CaptchaSession
    {
        public CaptchaSession(string functionName, string text, CaptchaLevels level)
        {
            FunctionName = functionName;
            Text = text;
            Level = level;
        }

        public string FunctionName { get; private set; }

        public string Text { get; private set; }

        public CaptchaLevels Level { get; private set; }
    }
}