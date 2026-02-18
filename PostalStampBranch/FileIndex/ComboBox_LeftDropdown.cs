using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FileIndex
{
    // ═══════════════════════════════════════════════════════════════════════════════
    // METHOD 1: CUSTOM COMBOBOX CLASS (Recommended - Reusable)
    // ═══════════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// ComboBox with dropdown aligned to left side
    /// </summary>
    public class ComboBoxLeftAligned : ComboBox
    {
        private const int WM_CTLCOLORLISTBOX = 0x0134;
        private const int CBS_DROPDOWN = 0x2;
        private const int CBS_DROPDOWNLIST = 0x3;

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        private const uint GW_CHILD = 5;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;

        public ComboBoxLeftAligned()
        {
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_CTLCOLORLISTBOX)
            {
                // Dropdown window ka handle get karo
                IntPtr dropdownHandle = GetWindow(this.Handle, GW_CHILD);

                if (dropdownHandle != IntPtr.Zero)
                {
                    // ComboBox ki position
                    Point comboLocation = this.PointToScreen(Point.Empty);

                    // Dropdown ko left side align karo
                    int dropdownWidth = this.DropDownWidth > 0 ? this.DropDownWidth : this.Width;
                    int leftAlignedX = comboLocation.X - dropdownWidth + this.Width;
                    int dropdownY = comboLocation.Y + this.Height;

                    // Position set karo
                    SetWindowPos(dropdownHandle, IntPtr.Zero,
                        leftAlignedX, dropdownY, 0, 0,
                        SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);
                }
            }
        }
    }


    // ═══════════════════════════════════════════════════════════════════════════════
    // METHOD 2: RUNTIME CONVERSION (Existing ComboBox ko convert karo)
    // ═══════════════════════════════════════════════════════════════════════════════

    public static class ComboBoxExtensions
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        private const uint GW_CHILD = 5;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;

        /// <summary>
        /// Existing ComboBox ko left-aligned dropdown mein convert karta hai
        /// </summary>
        public static void EnableLeftDropdown(this ComboBox combo)
        {
            combo.DropDown += (s, e) =>
            {
                // Dropdown window handle
                IntPtr dropdownHandle = GetWindow(combo.Handle, GW_CHILD);

                if (dropdownHandle != IntPtr.Zero)
                {
                    // ComboBox position
                    Point comboLocation = combo.PointToScreen(Point.Empty);

                    // Calculate left-aligned position
                    int dropdownWidth = combo.DropDownWidth > 0 ? combo.DropDownWidth : combo.Width;
                    int leftX = comboLocation.X - dropdownWidth + combo.Width;
                    int dropdownY = comboLocation.Y + combo.Height;

                    // Set position
                    SetWindowPos(dropdownHandle, IntPtr.Zero,
                        leftX, dropdownY, 0, 0,
                        SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);
                }
            };
        }
    }
}


    // ═══════════════════════════════════════════════════════════════════════════════
    // USAGE EXAMPLES
    // ═══════════════════════════════════════════════════════════════════════════════
    /*
    public partial class YourForm : Form
    {
        // ─────────────────────────────────────────────────────────────────────────
        // EXAMPLE 1: Using Custom ComboBox Class
        // ─────────────────────────────────────────────────────────────────────────
        
        private void Example1_UsingCustomClass()
        {
            // Designer mein ya code mein
            ComboBoxLeftAligned cmbLeftAligned = new ComboBoxLeftAligned();
            cmbLeftAligned.Location = new Point(300, 50);
            cmbLeftAligned.Size = new Size(200, 25);
            cmbLeftAligned.DropDownWidth = 300; // Dropdown ki width
            
            // Items add karo
            cmbLeftAligned.Items.Add("Option 1");
            cmbLeftAligned.Items.Add("Option 2");
            cmbLeftAligned.Items.Add("Option 3");
            
            // Form mein add karo
            this.Controls.Add(cmbLeftAligned);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // EXAMPLE 2: Convert Existing ComboBox (Designer se banaya hua)
        // ─────────────────────────────────────────────────────────────────────────
        
        private void Form_Load(object sender, EventArgs e)
        {
            // ✅ Existing ComboBox ko left-aligned bana do
            cmbPriority.EnableLeftDropdown();
            cmbStatus.EnableLeftDropdown();
            cmbCategory.EnableLeftDropdown();
            
            // Agar dropdown width zyada chahiye
            cmbPriority.DropDownWidth = 300;
        }

        // ─────────────────────────────────────────────────────────────────────────
        // EXAMPLE 3: Multiple ComboBoxes at Once
        // ─────────────────────────────────────────────────────────────────────────
        
        private void SetupAllComboBoxes()
        {
            // Form ke saare ComboBoxes ko left-align karo
            foreach (Control control in this.Controls)
            {
                if (control is ComboBox combo)
                {
                    combo.EnableLeftDropdown();
                    combo.DropDownWidth = 250; // Common width
                }
            }
            
            // GroupBox ya Panel ke andar bhi
            foreach (Control control in groupBox1.Controls)
            {
                if (control is ComboBox combo)
                {
                    combo.EnableLeftDropdown();
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────────────
        // EXAMPLE 4: Conditional Left Alignment (Agar form ke right side par hai)
        // ─────────────────────────────────────────────────────────────────────────
        
        private void SmartAlignment()
        {
            foreach (Control control in this.Controls)
            {
                if (control is ComboBox combo)
                {
                    // Agar ComboBox form ke right half mein hai
                    if (combo.Right > this.Width / 2)
                    {
                        // Left-align karo
                        combo.EnableLeftDropdown();
                    }
                    // Warna normal (right-aligned) rahne do
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────────────
        // EXAMPLE 5: With Custom DropDown Width
        // ─────────────────────────────────────────────────────────────────────────
        
        private void SetupComboWithCustomWidth()
        {
            cmbTask.DropDownWidth = 400; // Dropdown 400px wide hogi
            cmbTask.EnableLeftDropdown();
            
            // Items add karo
            cmbTask.Items.Add("Very long task description that needs wide dropdown");
            cmbTask.Items.Add("Another long option");
        }
    }


    // ═══════════════════════════════════════════════════════════════════════════════
    // METHOD 3: SIMPLE MANUAL APPROACH (Without Custom Class)
    // ═══════════════════════════════════════════════════════════════════════════════
    
    public partial class SimpleForm : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            
            // Dropdown handle
            IntPtr dropdown = GetWindow(combo.Handle, 5); // GW_CHILD = 5
            
            if (dropdown != IntPtr.Zero)
            {
                // Current position
                Point location = combo.PointToScreen(Point.Empty);
                
                // Calculate left-aligned position
                int width = combo.DropDownWidth > 0 ? combo.DropDownWidth : combo.Width;
                int x = location.X - width + combo.Width;
                int y = location.Y + combo.Height;
                
                // Move dropdown
                SetWindowPos(dropdown, IntPtr.Zero, x, y, 0, 0, 0x0001 | 0x0004 | 0x0010);
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // ✅ DropDown event attach karo
            comboBox1.DropDown += comboBox1_DropDown;
            comboBox2.DropDown += comboBox1_DropDown;
            comboBox3.DropDown += comboBox1_DropDown;
        }
    }
}


// ═══════════════════════════════════════════════════════════════════════════════
// COMPLETE WORKING EXAMPLE - Copy-Paste Ready
// ═══════════════════════════════════════════════════════════════════════════════

/*
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace YourNamespace
{
    public partial class TestForm : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        private ComboBox cmbTest;

        public TestForm()
        {
            InitializeComponent();
            SetupComboBox();
        }

        private void SetupComboBox()
        {
            // ComboBox create karo
            cmbTest = new ComboBox
            {
                Location = new Point(300, 50),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                DropDownWidth = 300 // Dropdown ki width
            };

            // Items add karo
            cmbTest.Items.AddRange(new object[] {
                "Option 1",
                "Option 2",
                "Option 3",
                "Long option that needs more width"
            });

            // ✅ Left dropdown enable karo
            cmbTest.DropDown += (s, e) =>
            {
                IntPtr dropdown = GetWindow(cmbTest.Handle, 5);
                if (dropdown != IntPtr.Zero)
                {
                    Point loc = cmbTest.PointToScreen(Point.Empty);
                    int width = cmbTest.DropDownWidth;
                    int x = loc.X - width + cmbTest.Width;
                    int y = loc.Y + cmbTest.Height;
                    SetWindowPos(dropdown, IntPtr.Zero, x, y, 0, 0, 0x0015);
                }
            };

            // Form mein add karo
            this.Controls.Add(cmbTest);
        }
    }
}
*/
