namespace Alpha.Captcha.Models
{
    using System.Drawing;

    public class CaptchaImageOptions
    {
        public CaptchaImageOptions(
            int width,
            int height,
            Color backColor,
            Color fontColor,
            Color noiseColor,
            Color lineColor,
            FontWarpFactors fontWarp,
            BackgroundNoiseLevels backgroundNoise,
            LineNoiseLevels lineNoise,
            string[] fonts
            )
        {
            Width = width;
            Height = height;
            BackColor = backColor;
            FontColor = fontColor;
            NoiseColor = noiseColor;
            LineColor = lineColor;
            FontWarp = fontWarp;
            BackgroundNoise = backgroundNoise;
            LineNoise = lineNoise;
            Fonts = fonts;
        }

        public int Width { get; }

        public int Height { get; }

        public Color BackColor { get; }

        public Color FontColor { get; }

        public Color NoiseColor { get; }

        public Color LineColor { get; }

        public FontWarpFactors FontWarp { get; }

        public BackgroundNoiseLevels BackgroundNoise { get; }

        public LineNoiseLevels LineNoise { get; }

        public string[] Fonts { get; }
    }
}