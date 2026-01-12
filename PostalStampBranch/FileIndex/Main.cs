using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileIndex
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void issuePrintingWorkToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void OpenChildForm(Form child)
        {
            // 1. Isay Main Form ka bacha (Child) banayen
            child.MdiParent = this;

            // 2. Kholne se pehle Theme apply kar dein
            UIHelper.SetFormTheme(child);

            // 3. Form ko show karein
            child.Show();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addIssueDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Check karein kya Form1 pehle se khula hai?
            Form frm = Application.OpenForms["Form1"];

            if (frm == null)
            {
                // Agar nahi khula to naya khol dein
                Form1 newFrm = new Form1();
                newFrm.MdiParent = this; // Isay parent ke andar rakhein

                // Agar aap chahte hain ke form maximize khule (full screen):
                newFrm.WindowState = FormWindowState.Maximized;

                newFrm.Show();
            }
            else
            {
                // 2. Agar khula hai to usay samnay le aayen
                if (frm.WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal; // Agar minimize tha to wapis normal kar dein
                }
                frm.Activate();
            }
        }

        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addFileNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Check karein kya Form1 pehle se khula hai?
            Form frm = Application.OpenForms["AddFileNo"];

            if (frm == null)
            {
                // Agar nahi khula to naya khol dein
                AddFileNo newFrm = new AddFileNo();
                newFrm.MdiParent = this; // Isay parent ke andar rakhein

                // Agar aap chahte hain ke form maximize khule (full screen):
                newFrm.WindowState = FormWindowState.Maximized;

                newFrm.Show();
            }
            else
            {
                // 2. Agar khula hai to usay samnay le aayen
                if (frm.WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal; // Agar minimize tha to wapis normal kar dein
                }
                frm.Activate();
            }
        }

        private void registorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close Btn ky visible ko false karna 
            //Register childForm = new Register();
            //childForm.MdiParent = this;
            //childForm.CloseBtn.Visible = false;
            //childForm.Show();

            // 1. Check karein kya Form1 pehle se khula hai?
            Form frm = Application.OpenForms["Register"];

            if (frm == null)
            {
                // Agar nahi khula to naya khol dein
                Register newFrm = new Register();
                newFrm.MdiParent = this; // Isay parent ke andar rakhein

                // Agar aap chahte hain ke form maximize khule (full screen):
                newFrm.WindowState = FormWindowState.Maximized;

                newFrm.Show();
            }
            else
            {
                // 2. Agar khula hai to usay samnay le aayen
                if (frm.WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal; // Agar minimize tha to wapis normal kar dein
                }
                frm.Activate();
            }
        }

        private void editIssueDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Check karein kya Form1 pehle se khula hai?
            Form frm = Application.OpenForms["IssueCorrection"];

            if (frm == null)
            {
                // Agar nahi khula to naya khol dein
                IssueCorrection newFrm = new IssueCorrection();
                newFrm.MdiParent = this; // Isay parent ke andar rakhein

                // Agar aap chahte hain ke form maximize khule (full screen):
                newFrm.WindowState = FormWindowState.Maximized;

                newFrm.Show();
            }
            else
            {
                // 2. Agar khula hai to usay samnay le aayen
                if (frm.WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal; // Agar minimize tha to wapis normal kar dein
                }
                frm.Activate();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void editFileNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["FileCorrection"];

            if (frm == null)
            {
                // Agar nahi khula to naya khol dein
                FileCorrection newFrm = new FileCorrection();
                newFrm.MdiParent = this; // Isay parent ke andar rakhein

                // Agar aap chahte hain ke form maximize khule (full screen):
                newFrm.WindowState = FormWindowState.Maximized;

                newFrm.Show();
            }
            else
            {
                // 2. Agar khula hai to usay samnay le aayen
                if (frm.WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal; // Agar minimize tha to wapis normal kar dein
                }
                frm.Activate();
            }
        }

        private void searchFileNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                searchFile childForm = new searchFile();

                // 1. Batayein ke iska Parent ye Form hai
                childForm.MdiParent = this;

                // 2. IS LINE KO ADD KAREIN: 
                // Is se form parent ke andar poori screen par khulay ga
                childForm.WindowState = FormWindowState.Maximized;

                childForm.Show();
            }
        }

        private void printFileReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addDistributionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["DistributionList"];

            if (frm == null)

            {
                DistributionList newFRm = new DistributionList();
                newFRm.MdiParent = this;
                newFRm.WindowState = FormWindowState.Maximized;
                newFRm.Show();
            }
            else
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal;
                }
                frm.Activate();
            }
        }
    }
}

