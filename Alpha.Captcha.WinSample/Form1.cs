namespace Alpha.Captcha.WinSample
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Services;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateClick(object sender, EventArgs e)
        {
            var randomService = new RandomService();
            var captchaService = new CaptchaService();
            var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                for (var i = 0; i < 5; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        var captchaText = randomService.RandomText(CaptchaProfiles.DefaultTextOptions);
                        var captchaImage = captchaService.Generate(captchaText, CaptchaLevels.Medium);
                        var x = i * (captchaImage.Width + 5);
                        var y = j * (captchaImage.Height + 5);
                        g.DrawImage(captchaImage, x, y);
                    }
                }
            }

            pictureBox1.Image = bitmap;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateClick(sender, e);
        }
    }
}
