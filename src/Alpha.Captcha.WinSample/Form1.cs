using Alpha.Captcha.Models;
using Alpha.Captcha.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Alpha.Captcha.WinSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateClick(object sender, EventArgs e)
        {
            var captchaService = Configuration.Resolve<CaptchaService>();
            var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                for (var i = 0; i < 5; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        var captchaText = captchaService.RandomText(CaptchaProfiles.DefaultTextOptions);
                        var captchaImage = captchaService.Generate(captchaText, CaptchaLevels.Medium);
                        var x = i * (captchaImage.Width + 5);
                        var y = j * (captchaImage.Height + 5);
                        g.DrawImage(captchaImage, x, y);
                    }
                }
            }

            pictureBox1.Image = bitmap;
        }
    }
}
