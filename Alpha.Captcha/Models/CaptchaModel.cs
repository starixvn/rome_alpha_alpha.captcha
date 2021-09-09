namespace Alpha.Captcha.Models
{
    public class CaptchaModel
    {
        public CaptchaModel(CaptchaSetting setting, string text)
        {
            Setting = setting;
            Text = text;
        }

        public string Text { get; }

        public CaptchaSetting Setting { get; }
    }
}