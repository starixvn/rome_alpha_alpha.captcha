namespace Alpha.Captcha.Models
{
    using System.Drawing;

    public class CaptchaOptions
    {
        public CaptchaOptions(
            int width,
            int height,
            int textLength,
            Color backColor,
            Color fontColor,
            Color noiseColor,
            Color lineColor,
            FontWarpFactor fontWarp,
            BackgroundNoiseLevel backgroundNoise,
            LineNoiseLevel lineNoise,
            string chars,
            string[] fonts
            )
        {
            Width = width;
            Height = height;
            TextLength = textLength;
            BackColor = backColor;
            FontColor = fontColor;
            NoiseColor = noiseColor;
            LineColor = lineColor;
            FontWarp = fontWarp;
            BackgroundNoise = backgroundNoise;
            LineNoise = lineNoise;
            Chars = chars;
            Fonts = fonts;
        }

        public int Width { get; }

        public int Height { get; }

        public int TextLength { get; }

        public Color BackColor { get; }

        public Color FontColor { get; }

        public Color NoiseColor { get; }

        public Color LineColor { get; }

        public FontWarpFactor FontWarp { get; }

        public BackgroundNoiseLevel BackgroundNoise { get; }

        public LineNoiseLevel LineNoise { get; }

        public string Chars { get; }

        public string[] Fonts { get; }
    }
}