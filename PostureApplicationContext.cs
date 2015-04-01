using System;
using System.Drawing;
using System.Windows.Forms;
using PostureApplication.Properties;

namespace PostureApplication
{
    internal class PostureApplicationContext : ApplicationContext
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly Timer _timer = new Timer();

        public PostureApplicationContext(NotifyIcon notifyIcon)
        {
            _timer.Interval = Settings.Default.SavedInterval;
            _timer.Start();
            _timer.Tick += TimerOnTick;
            _timer.Enabled = Settings.Default.Active;

            _notifyIcon = notifyIcon;
            _notifyIcon.Visible = true;

            SetContextMenu();
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            _notifyIcon.BalloonTipTitle = "Posture Check";
            _notifyIcon.BalloonTipText = "Check all your posture points now";
            _notifyIcon.ShowBalloonTip(1000);
        }

        private void SetContextMenu()
        {
            var configMenuItem = new MenuItem("Active", (sender, args) =>
            {
                _timer.Enabled = !_timer.Enabled;
                SetContextMenu();
            }) { Checked = _timer.Enabled };


            var changeInterval = new MenuItem("Change Interval");
            foreach (var interval in new[] {5, 10, 15, 30, 45, 60, 120})
            {
                var timerInterval = interval*60000;
                changeInterval.MenuItems.Add(new MenuItem(String.Format("{0}", interval), (sender, args) =>
                {
                    _timer.Interval = timerInterval;
                    Settings.Default.SavedInterval = timerInterval;
                    Settings.Default.Save();
                    SetContextMenu();
                })
                {Checked = _timer.Interval == timerInterval});
            }

            var exitMenuItem = new MenuItem("Exit", (sender, args) =>
            {
                Application.Exit();
            });
            
            _notifyIcon.ContextMenu = new ContextMenu();
            _notifyIcon.ContextMenu.MenuItems.Add(configMenuItem);
            _notifyIcon.ContextMenu.MenuItems.Add(changeInterval);
            _notifyIcon.ContextMenu.MenuItems.Add("-");
            _notifyIcon.ContextMenu.MenuItems.Add(exitMenuItem);

            _notifyIcon.Text = String.Format("Every {0} minutes ({1})", _timer.Interval / 60000, !_timer.Enabled ? "Suspended" : "Active");
            _notifyIcon.Icon = (Icon)Properties.Resources.ResourceManager.GetObject(!_timer.Enabled ? "MainIconSuspended" : "MainIcon");
        }
    }
}