namespace Alpha.Captcha.Models
{
    using System.Drawing;

    public class CaptchaOptions
    {
        public int Width { get; } = 200;

        public int Height { get; } = 50;

        public Color BackColor { get; } = Color.White;

        public Color FontColor { get; } = Color.Black;

        public Color NoiseColor { get; } = Color.Black;

        public Color LineColor { get; } = Color.Black;

        public int TextLength { get; } = 6;

        public FontWarpFactor FontWarp { get; } = FontWarpFactor.Medium;

        public BackgroundNoiseLevel BackgroundNoise { get; } = BackgroundNoiseLevel.Medium;

        public LineNoiseLevel LineNoise { get; } = LineNoiseLevel.Medium;

        public string Chars { get; } = "ACDEFGHJKLNPQRTUVXYZ2346789";

        public string[] Fonts { get; } = new [] { "arial", "arial black", "comic sans ms", "courier new", "estrangelo edessa", "franklin gothic medium", "georgia", "lucida console", "lucida sans unicode", "mangal", "microsoft sans serif", "palatino linotype", "sylfaen", "tahoma", "times new roman", "trebuchet ms", "verdana" };
    }
}