namespace Alpha.Captcha.Models
{
    public class CaptchaSetting
    {
        public CaptchaSetting(CaptchaLevels level)
        {
            Level = level;
        }

        public CaptchaLevels Level { get; private set; }

        public string InputName { get; } = Configuration.CaptchaInputName;

        public bool IsForm { get; set; } = true;

        public bool IsShowButton { get; set; } = true;

        public string ButtonClassname { get; set; }
    }
}