namespace Alpha.Captcha.Models
{
    using System.Drawing;

    public class CaptchaResult
    {
        public CaptchaResult(string text, Image image)
        {
            Text = text;
            Image = image;
        }

        public string Text { get; }

        public Image Image { get; }
    }
}