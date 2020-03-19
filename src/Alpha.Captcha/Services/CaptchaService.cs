namespace Alpha.Captcha.Services
{
    using Alpha.Captcha.Models;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Text;

    public interface ICaptchaService
    {
        Image Generate(string captchaText, CaptchaImageOptions captchaImageOptions);

        Image Generate(string captchaText, CaptchaLevels captchaLevel);

        string RandomText(CaptchaTextOptions captchaTextOptions);
    }

    public class CaptchaService : ICaptchaService
    {
        private readonly Random random;

        public CaptchaService(Random random)
        {
            this.random = random;
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

        private static CaptchaImageOptions GetCaptchaImageOptions(CaptchaLevels level)
        {
            switch (level)
            {
                case CaptchaLevels.Low:
                    return CaptchaProfiles.Low;
                case CaptchaLevels.Medium:
                    return CaptchaProfiles.Medium;
                case CaptchaLevels.High:
                    return CaptchaProfiles.High;
                case CaptchaLevels.Extreme:
                    return CaptchaProfiles.Extreme;
                default:
                    throw new NotImplementedException(nameof(level));
            }
        }

        public Image Generate(string captchaText, CaptchaLevels level)
        {
            return Generate(captchaText, GetCaptchaImageOptions(level));
        }

        public Image Generate(string captchaText, CaptchaImageOptions captchaOptions)
        {
            var captchaImage = new Bitmap(captchaOptions.Width, captchaOptions.Height, PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(captchaImage))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, captchaOptions.Width, captchaOptions.Height);
                using (var br = new SolidBrush(captchaOptions.BackColor))
                {
                    gr.FillRectangle(br, rect);
                }

                var charOffset = 0;
                var charWidth = captchaOptions.Width / captchaText.Length;
                using (var br = new SolidBrush(captchaOptions.FontColor))
                {
                    foreach (var c in captchaText)
                    {
                        using (var fnt = GetFont(captchaOptions))
                        {
                            var rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), captchaOptions.Height);
                            using (var graphics = TextPath(c.ToString(), fnt, rectChar))
                            {
                                WarpText(captchaOptions, graphics, rectChar);
                                gr.FillPath(br, graphics);
                            }
                        }
                        charOffset += 1;
                    }
                }

                AddNoise(captchaOptions, gr, rect);
                AddLine(captchaOptions, gr, rect);
            }

            return captchaImage;
        }

        private string RandomFontFamily(CaptchaImageOptions captchaOptions)
        {
            return captchaOptions.Fonts[random.Next(0, captchaOptions.Fonts.Length)];
        }

        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(random.Next(xmin, xmax), random.Next(ymin, ymax));
        }

        private GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            };

            var gp = new GraphicsPath();
            gp.AddString(s, f.FontFamily, (int)f.Style, f.Size, r, sf);
            return gp;
        }

        private Font GetFont(CaptchaImageOptions captchaOptions)
        {
            var density = 0f;
            switch (captchaOptions.FontWarp)
            {
                case FontWarpFactor.None:
                    density = 0.7f;
                    break;
                case FontWarpFactor.Low:
                    density = 0.8f;
                    break;
                case FontWarpFactor.Medium:
                    density = 0.85f;
                    break;
                case FontWarpFactor.High:
                    density = 0.9f;
                    break;
                case FontWarpFactor.Extreme:
                    density = 0.95f;
                    break;
            }

            var fname = RandomFontFamily(captchaOptions);
            var fsize = Convert.ToInt32(captchaOptions.Height * density);
            return new Font(fname, fsize, FontStyle.Bold);
        }

        private void WarpText(CaptchaImageOptions captchaOptions, GraphicsPath textPath, Rectangle rect)
        {
            float WarpDivisor = 1.0f;
            float RangeModifier = 1.0f;
            switch (captchaOptions.FontWarp)
            {
                case FontWarpFactor.None:
                    return;
                case FontWarpFactor.Low:
                    WarpDivisor = 6;
                    RangeModifier = 1;
                    break;
                case FontWarpFactor.Medium:
                    WarpDivisor = 5;
                    RangeModifier = 1.3f;
                    break;
                case FontWarpFactor.High:
                    WarpDivisor = 4.5f;
                    RangeModifier = 1.4f;
                    break;
                case FontWarpFactor.Extreme:
                    WarpDivisor = 4;
                    RangeModifier = 1.5f;
                    break;
            }

            var rectF = new RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);
            var hrange = Convert.ToInt32(rect.Height / WarpDivisor);
            var wrange = Convert.ToInt32(rect.Width / WarpDivisor);
            var left = Math.Max(rect.Left - Convert.ToInt32(wrange * RangeModifier), 0);
            var top = Math.Max(rect.Top - Convert.ToInt32(hrange * RangeModifier), 0);
            var width = Math.Min(rect.Left + rect.Width + Convert.ToInt32(wrange * RangeModifier), captchaOptions.Width);
            var height = Math.Min(rect.Top + rect.Height + Convert.ToInt32(hrange * RangeModifier), captchaOptions.Height);
            var leftTop = RandomPoint(left, left + wrange, top, top + hrange);
            var rightTop = RandomPoint(width - wrange, width, top, top + hrange);
            var leftBottom = RandomPoint(left, left + wrange, height - hrange, height);
            var rightBottom = RandomPoint(width - wrange, width, height - hrange, height);
            var points = new PointF[] { leftTop, rightTop, leftBottom, rightBottom };
            var m = new Matrix();
            m.Translate(0, 0);
            textPath.Warp(points, rectF, m, WarpMode.Perspective, 0);
        }

        private void AddNoise(CaptchaImageOptions captchaOptions, Graphics graphics, Rectangle rect)
        {
            int density = 0;
            int size = 0;
            switch (captchaOptions.BackgroundNoise)
            {
                case BackgroundNoiseLevel.None:
                    return;
                case BackgroundNoiseLevel.Low:
                    density = 30;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.Medium:
                    density = 18;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.High:
                    density = 16;
                    size = 39;
                    break;
                case BackgroundNoiseLevel.Extreme:
                    density = 12;
                    size = 38;
                    break;
            }

            using (var br = new SolidBrush(captchaOptions.NoiseColor))
            {
                var max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);
                for (var i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / density); i++)
                {
                    graphics.FillEllipse(br, random.Next(rect.Width), random.Next(rect.Height), random.Next(max), random.Next(max));
                }
            }
        }

        private void AddLine(CaptchaImageOptions captchaOptions, Graphics graphics, Rectangle rect)
        {
            int length = 0;
            float width = 1.0f;
            int linecount = 0;
            switch (captchaOptions.LineNoise)
            {
                case LineNoiseLevel.None:
                    return;
                case LineNoiseLevel.Low:
                    length = 4;
                    width = Convert.ToSingle(captchaOptions.Height / 31.25);
                    // 1.6
                    linecount = 1;
                    break;
                case LineNoiseLevel.Medium:
                    length = 5;
                    width = Convert.ToSingle(captchaOptions.Height / 27.7777);
                    // 1.8
                    linecount = 1;
                    break;
                case LineNoiseLevel.High:
                    length = 3;
                    width = Convert.ToSingle(captchaOptions.Height / 25);
                    // 2.0
                    linecount = 2;
                    break;
                case LineNoiseLevel.Extreme:
                    length = 3;
                    width = Convert.ToSingle(captchaOptions.Height / 22.7272);
                    // 2.2
                    linecount = 3;
                    break;
            }

            var pf = new PointF[length + 1];
            using (var p = new Pen(captchaOptions.LineColor, width))
            {
                for (var l = 1; l <= linecount; l++)
                {
                    for (var i = 0; i <= length; i++)
                    {
                        pf[i] = RandomPoint(rect);
                    }

                    graphics.DrawCurve(p, pf, 1.75f);
                }
            }
        }
    }
}