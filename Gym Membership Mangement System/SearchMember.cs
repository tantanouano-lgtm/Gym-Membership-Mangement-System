using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Gym_Membership_Mangement_System
{
    public partial class SearchMember : Form
    {
        private System.Windows.Forms.Timer refreshTimer;
        private List<Member> allMembers = new List<Member>();

        public SearchMember()
        {
            InitializeComponent();
        }

        private async void SearchMember_Load(object sender, EventArgs e)
        {
            // Load all members immediately
            await LoadMembers();

            // Auto-refresh every 5 seconds
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 5000;
            refreshTimer.Tick += async (s, ev) => await LoadMembers();
            refreshTimer.Start();
        }

        private async Task LoadMembers()
        {
            try
            {
                string result = await ApiHelper.GetMembers();
                allMembers = JsonConvert.DeserializeObject<List<Member>>(result);

                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() => {
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = allMembers;
                    }));
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = allMembers;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Refresh error: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {
                // Search by name or ID from already loaded data
                string keyword = txtSearch.Text.ToLower();
                var filtered = allMembers.Where(m =>
                    (m.fullname != null && m.fullname.ToLower().Contains(keyword)) ||
                    m.id.ToString().Contains(keyword) ||
                    (m.phone != null && m.phone.Contains(keyword))
                ).ToList();

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = filtered;

                if (filtered.Count == 0)
                {
                    MessageBox.Show("No members found!", "Search Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // If search is empty, show all members
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = allMembers;
            }
        }

        private void SearchMember_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }
        }

        private void SearchMember_FormClosing_1(object sender, FormClosingEventArgs e)
        {

        }
    }
}