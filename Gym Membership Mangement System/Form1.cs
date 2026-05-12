using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gym_Membership_Mangement_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Boolean b = true;

        private void newMember_Click(object sender, EventArgs e)
        {
            if (b == true)
            {
                menuStrip1.Dock = DockStyle.Left;
                b = false;
                if (System.IO.File.Exists(@"C:\Users\Chamikara Mendis\Downloads\arrow-down-sign-to-navigate.png"))
                {
                    newMember.Image = Image.FromFile(@"C:\Users\Chamikara Mendis\Downloads\arrow-down-sign-to-navigate.png");
                }
            }
            else
            {
                menuStrip1.Dock = DockStyle.Top;
                b = true;
                if (System.IO.File.Exists(@"C:\Users\Chamikara Mendis\Downloads\right-arrow.png"))
                {
                    newMember.Image = Image.FromFile(@"C:\Users\Chamikara Mendis\Downloads\right-arrow.png");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(@"C:\Users\Chamikara Mendis\Downloads\right-arrow.png"))
            {
                newMember.Image = Image.FromFile(@"C:\Users\Chamikara Mendis\Downloads\right-arrow.png");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nothing needed here anymore
        }

        private void newMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Trainer obj = new Trainer();
            obj.Show();
        }

        private void newStaffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewStaff vs = new ViewStaff();
            vs.Show();
        }

        private void equipmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Equipment eq = new Equipment();
            eq.Show();
        }

        private void searchMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchMember sm = new SearchMember();
            sm.Show();
        }

        private void deleteMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteMember dm = new DeleteMember();
            dm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will close your application Confirm?", "Close",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Welcome Back", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Log out !! Confirm? ", "Log out",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.Close();
                Login lg = new Login();
                lg.Show();
            }
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            // Nothing needed here
        }
    }
}