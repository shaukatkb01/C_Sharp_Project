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
    public partial class frmUserGuide : Form
    {
        public frmUserGuide()
        {
            InitializeComponent();

            rtbGuide.Text =
        @"SUPPLY BRANCH MANAGEMENT SYSTEM
USER GUIDE

1. LOGIN
Enter User Name and Password to access the system.

2. OFFICE & ZONE
Office and Zone information can be maintained from the relevant menu.

3. Supply Operations

i. INDENT
Create a new indent by selecting the required office, category and denomination.
Indent details can be reviewed before saving.

ii. INDENT CRECTION
Crection of indents can be done by selecting the required office, category and denomination.

iii. SUPPLY
Supply can be created against an approved indent.
Supply details, quantities, dispatch information and packing information can be entered.

iv. Draft Supply
Draft supply can be created against an approved indent.

4. INVOICE
Invoice numbers are generated according to the supply type and applicable sequence.

5. REPORTS
Use the Reports menu to generate:
• Indent Register
• Category Wise Indent
• Office Wise Indent
• Supply Register
• Category Wise Supply
• Office Wise Supply
• Invoice Register
• Financial Year Report
• Performa

6. TO-DO LIST
Tasks can be added for the logged-in user.
Tasks can be assigned a priority and due date.
Completed tasks can be marked as completed.
Tasks can also be deleted or their priority changed.

7. DATABASE BACKUP
Database backup can be taken from the Backup option.
The backup file is saved for database recovery purposes.

8. LOGOUT
Use Logout to safely exit the current user session.

9. HELP
Use the Help menu to access this User Guide and About information.
";
        }
       

       
    }
}
