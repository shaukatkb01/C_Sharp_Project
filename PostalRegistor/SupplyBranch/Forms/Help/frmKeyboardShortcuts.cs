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

GENERAL
────────────────────────────────

Esc             Close current window
Ctrl + S        Save current record
Ctrl + F        Search / Find
Ctrl + P        Print / Preview


NAVIGATION
────────────────────────────────

F5              Refresh current data
Alt + F4        Close application


REPORTS
────────────────────────────────

Ctrl + P        Print report / preview


NOTES
────────────────────────────────

Keyboard shortcuts may vary depending
on the active form and available functions.";
        }
    }
}