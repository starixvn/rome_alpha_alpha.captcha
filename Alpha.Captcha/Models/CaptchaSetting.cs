using System.Collections.Generic;

namespace Alpha.Captcha.Models
{
    public class CaptchaSetting
    {
        public string FunctionName { get; set; }

        public CaptchaLevels CaptchaLevel { get; set; } = CaptchaLevels.Extreme;

        public IDictionary<string, string> HiddenProps { get; set; }

        /// <summary>
        /// render captcha as a form control
        /// </summary>
        public bool IsForm { get; set; } = true;

        /// <summary>
        /// add button in form
        /// </summary>
        public bool IsShowButton { get; set; } = true;

        /// <summary>
        /// add custom class to button
        /// </summary>
        public string ButtonClassname { get; set; }
    }
}