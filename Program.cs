using System;
using System.Windows.Forms;

namespace PostureApplication
{
    static class Program
    {
        static readonly NotifyIcon NotifyIcon = new NotifyIcon();
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += ApplicationOnApplicationExit;
            Application.Run(new PostureApplicationContext(NotifyIcon));
        }

        private static void ApplicationOnApplicationExit(object sender, EventArgs eventArgs)
        {
            NotifyIcon.Visible = false;
        }
    }
}
