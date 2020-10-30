namespace Alpha.Captcha.Models
{
    public class CaptchaTextOptions
    {
        public CaptchaTextOptions(int textLength, string chars)
        {
            TextLength = textLength;
            Chars = chars;
        }

        public int TextLength { get; }

        public string Chars { get; }
    }
}