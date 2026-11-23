using SupplyBranch.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch
{
    internal static class Program
    {
        private sealed class ModifierShortcutFilter : IMessageFilter
        {
            private const int WmKeyDown = 0x0100;
            private const int WmSysKeyDown = 0x0104;
            private const int WmSysKeyUp = 0x0105;
            private const int WmSysChar = 0x0106;

            public bool PreFilterMessage(ref Message message)
            {
                if (message.Msg == WmSysKeyDown ||
                    message.Msg == WmSysKeyUp ||
                    message.Msg == WmSysChar)
                {
                    return true;
                }

                return message.Msg == WmKeyDown &&
                    (Control.ModifierKeys & (Keys.Control | Keys.Alt)) != Keys.None;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread] static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.AddMessageFilter(new ModifierShortcutFilter());
            Application.Run(new frmLogin());
        }
    }
}
