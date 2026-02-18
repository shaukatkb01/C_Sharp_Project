using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FileIndex
{
    /// <summary>
    /// ComboBox Helper - Dropdown ko left side align karne ke liye
    /// Usage: ComboBoxHelper.SetLeftDropdown(yourComboBox);
    /// </summary>
    public static class ComboBoxHelper
    {
        // ═══════════════════════════════════════════════════════════════
        // Windows API Functions
        // ═══════════════════════════════════════════════════════════════
        
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        private const uint GW_CHILD = 5;
        private const uint SWP_FLAGS = 0x0015; // SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE

        // ═══════════════════════════════════════════════════════════════
        // PUBLIC METHOD - Yeh call karo
        // ═══════════════════════════════════════════════════════════════
        
        /// <summary>
        /// ComboBox ki dropdown ko left side align karta hai
        /// </summary>
        /// <param name="comboBox">ComboBox jisko left-align karna hai</param>
        /// <param name="dropdownWidth">Dropdown ki width (optional, default = ComboBox width)</param>
        public static void SetLeftDropdown(ComboBox comboBox, int dropdownWidth = 0)
        {
            if (comboBox == null)
                return;

            // Dropdown width set karo (agar specified hai)
            if (dropdownWidth > 0)
            {
                comboBox.DropDownWidth = dropdownWidth;
            }

            // Remove existing event (duplicate prevent karne ke liye)
            comboBox.DropDown -= ComboBox_DropDown;
            
            // Event attach karo
            comboBox.DropDown += ComboBox_DropDown;
        }

        /// <summary>
        /// Multiple ComboBoxes ko ek saath left-align karta hai
        /// </summary>
        /// <param name="comboBoxes">ComboBox array</param>
        /// <param name="dropdownWidth">Common dropdown width (optional)</param>
        public static void SetLeftDropdown(ComboBox[] comboBoxes, int dropdownWidth = 0)
        {
            if (comboBoxes == null)
                return;

            foreach (ComboBox combo in comboBoxes)
            {
                SetLeftDropdown(combo, dropdownWidth);
            }
        }

        /// <summary>
        /// Form ke saare ComboBoxes ko left-align karta hai
        /// </summary>
        /// <param name="form">Form jiske ComboBoxes align karne hain</param>
        /// <param name="dropdownWidth">Common dropdown width (optional)</param>
        public static void SetLeftDropdownForAll(Form form, int dropdownWidth = 0)
        {
            if (form == null)
                return;

            SetLeftDropdownForContainer(form, dropdownWidth);
        }

        /// <summary>
        /// Kisi bhi container (Form, Panel, GroupBox) ke saare ComboBoxes ko left-align karta hai
        /// </summary>
        private static void SetLeftDropdownForContainer(Control container, int dropdownWidth)
        {
            foreach (Control control in container.Controls)
            {
                if (control is ComboBox combo)
                {
                    SetLeftDropdown(combo, dropdownWidth);
                }

                // Nested containers (Panel, GroupBox, etc.)
                if (control.HasChildren)
                {
                    SetLeftDropdownForContainer(control, dropdownWidth);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // PRIVATE EVENT HANDLER
        // ═══════════════════════════════════════════════════════════════
        
        private static void ComboBox_DropDown(object sender, EventArgs e)
        {
            if (sender is ComboBox combo)
            {
                AlignDropdownToLeft(combo);
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // CORE ALIGNMENT LOGIC
        // ═══════════════════════════════════════════════════════════════
        
        private static void AlignDropdownToLeft(ComboBox comboBox)
        {
            try
            {
                // Dropdown window handle
                IntPtr dropdownHandle = GetWindow(comboBox.Handle, GW_CHILD);

                if (dropdownHandle != IntPtr.Zero)
                {
                    // ComboBox ki screen position
                    Point comboLocation = comboBox.PointToScreen(Point.Empty);

                    // Dropdown width
                    int width = comboBox.DropDownWidth > 0 
                        ? comboBox.DropDownWidth 
                        : comboBox.Width;

                    // Left-aligned position calculate karo
                    int x = comboLocation.X - width + comboBox.Width;
                    int y = comboLocation.Y + comboBox.Height;

                    // Dropdown ko move karo
                    SetWindowPos(dropdownHandle, IntPtr.Zero, x, y, 0, 0, SWP_FLAGS);
                }
            }
            catch
            {
                // Silently ignore errors
            }
        }
    }
}
