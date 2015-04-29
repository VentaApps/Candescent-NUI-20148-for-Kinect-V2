using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCT.NUI.StartMenu.Properties;

namespace CCT.NUI.StartMenu
{
    internal class NotifyIconPresenter
    {
        private NotifyIcon notifyIcon;

        internal NotifyIconPresenter()
        { 
            this.notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Resources.organizer;

            var contextMenu = new System.Windows.Forms.ContextMenuStrip();
            contextMenu.ShowImageMargin = false;

            var buttonSettings = new System.Windows.Forms.ToolStripMenuItem("Settings...");
            buttonSettings.Click += new EventHandler(buttonSettings_Click);
            contextMenu.Items.Add(buttonSettings);
            contextMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            var buttonClose = new System.Windows.Forms.ToolStripMenuItem("Close");
            buttonClose.Click += new EventHandler(buttonClose_Click);
            contextMenu.Items.Add(buttonClose);

            notifyIcon.ContextMenuStrip = contextMenu;

            notifyIcon.Text = "Candescent NUI Start Menu";
            notifyIcon.Visible = true;

            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
        }
        
        public void Dispose()
        {
            this.notifyIcon.Dispose();
        }

        protected void OnShowSettings()
        {
            if (this.ShowSettings != null)
            {
                this.ShowSettings(this, EventArgs.Empty);
            }
        }
        public event EventHandler ShowSettings;

        protected void OnClose()
        {
            if (this.Close != null)
            {
                this.Close(this, EventArgs.Empty);
            }
        }
        public event EventHandler Close;

        void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.OnShowSettings();
        }

        void buttonClose_Click(object sender, EventArgs e)
        {
            this.OnClose();
        }

        void buttonSettings_Click(object sender, EventArgs e)
        {
            this.OnShowSettings();
        }
    }
}
