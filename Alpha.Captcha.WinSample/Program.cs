using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alpha.Captcha.WinSample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            for (int i = 0; i < 20; i++)
            {
                var t = new Thread(() =>
                {
                    Application.Run(new Form1());
                });

                t.Start();
            }
            
        }
    }
}
