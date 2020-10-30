namespace Alpha.Captcha.Services
{
    using System;
    using System.Text;
    using Alpha.Captcha.Models;

    public interface IRandomService
    {
        int Next(int maxValue);

        int Next(int minValue, int maxValue);

        string RandomText(CaptchaTextOptions captchaTextOptions);
    }

    public class RandomService: IRandomService
    {
        private readonly Random random = new Random();

        public int Next(int maxValue)
        {
            return random.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public string RandomText(CaptchaTextOptions captchaTextOptions)
        {
            var sb = new StringBuilder(captchaTextOptions.TextLength);
            var maxLength = captchaTextOptions.Chars.Length;
            for (var i = 0; i < captchaTextOptions.TextLength; i++)
            {
                sb.Append(captchaTextOptions.Chars[random.Next(maxLength)]);
            }

            return sb.ToString();
        }
    }
}
