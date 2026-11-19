using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms.Help
{
    public partial class frmKeyboardShortcuts : Form
    {
        public frmKeyboardShortcuts()
        {
            InitializeComponent();

            rtbShortcuts.Text =
        @"SUPPLY BRANCH MANAGEMENT SYSTEM
KEYBOARD SHORTCUTS

Ctrl and Alt keyboard shortcuts are disabled.
Use the on-screen menus and buttons instead.


NAVIGATION
────────────────────────────────

Esc             Close current window
F5              Refresh current data";
        }
    }
}