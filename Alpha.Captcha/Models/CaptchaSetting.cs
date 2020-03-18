namespace Alpha.Captcha.Models
{
    public class CaptchaSetting
    {
        public CaptchaSetting()
        {
            Level = CaptchaLevels.Extreme;
            IsForm = true;
            IsShowButton = true;
        }

        public string FunctionName { get; set; }

        public CaptchaLevels Level { get; set; }

        /// <summary>
        /// render captcha as a form control
        /// </summary>
        public bool IsForm { get; set; }

        /// <summary>
        /// add button in form
        /// </summary>
        public bool IsShowButton { get; set; }

        /// <summary>
        /// add custom class to button
        /// </summary>
        public string ButtonClassname { get; set; }
    }
}