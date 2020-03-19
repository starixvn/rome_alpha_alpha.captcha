namespace Alpha.Captcha.Models
{
    using System.Drawing;

    public class CaptchaProfiles
    {
        private static readonly int DefaultWidth = 250;
        private static readonly int DefaultHeight = 50;
        private static readonly int DefaultTextLength = 6;
        private static readonly Color DefaultBackColor = Color.White;
        private static readonly Color DefaultFontColor = Color.Black;
        private static readonly Color DefaultNoiseColor = Color.Black;
        private static readonly Color DefaultLineColor = Color.Black;
        private static readonly string DefaultChars = "ACDEFGHJKLNPQRTUVXYZ2346789";
        private static readonly string[] DefaultFonts = new [] {
            "arial",
            "arial black",
            "comic sans ms",
            "courier new",
            "estrangelo edessa",
            "franklin gothic medium",
            "georgia",
            "lucida console",
            "lucida sans unicode",
            "mangal",
            "microsoft sans serif",
            "palatino linotype",
            "sylfaen",
            "tahoma",
            "times new roman",
            "trebuchet ms",
            "verdana"
        };

        public static CaptchaTextOptions DefaultTextOptions = new CaptchaTextOptions(DefaultTextLength, DefaultChars);

        public static CaptchaImageOptions Low = new CaptchaImageOptions(
            DefaultWidth,
            DefaultHeight,
            DefaultBackColor,
            DefaultFontColor,
            DefaultNoiseColor,
            DefaultLineColor,
            FontWarpFactor.Low,
            BackgroundNoiseLevel.Low,
            LineNoiseLevel.Low,
            DefaultFonts);

        public static CaptchaImageOptions Medium = new CaptchaImageOptions(
            DefaultWidth,
            DefaultHeight,
            DefaultBackColor,
            DefaultFontColor,
            DefaultNoiseColor,
            DefaultLineColor,
            FontWarpFactor.Medium,
            BackgroundNoiseLevel.Medium,
            LineNoiseLevel.Medium,
            DefaultFonts);

        public static CaptchaImageOptions High = new CaptchaImageOptions(
            DefaultWidth,
            DefaultHeight,
            DefaultBackColor,
            DefaultFontColor,
            DefaultNoiseColor,
            DefaultLineColor,
            FontWarpFactor.High,
            BackgroundNoiseLevel.High,
            LineNoiseLevel.High,
            DefaultFonts);

        public static CaptchaImageOptions Extreme = new CaptchaImageOptions(
            DefaultWidth,
            DefaultHeight,
            DefaultBackColor,
            DefaultFontColor,
            DefaultNoiseColor,
            DefaultLineColor,
            FontWarpFactor.Extreme,
            BackgroundNoiseLevel.Extreme,
            LineNoiseLevel.Extreme,
            DefaultFonts);
    }
}