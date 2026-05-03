using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Gym_Membership_Mangement_System
{
    public partial class DeleteMember : Form
    {
        private System.Windows.Forms.Timer refreshTimer;

        public DeleteMember()
        {
            InitializeComponent();
        }

        private async void DeleteMember_Load(object sender, EventArgs e)
        {
            await LoadMembers();

            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 5000;
            refreshTimer.Tick += async (s, ev) => await LoadMembers();
            refreshTimer.Start();
        }

        private async System.Threading.Tasks.Task LoadMembers()
        {
            try
            {
                string result = await ApiHelper.GetMembers();
                var members = JsonConvert.DeserializeObject<List<Member>>(result);

                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() => {
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = members;
                    }));
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = members;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Load error: " + ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDelete.Text))
            {
                MessageBox.Show("Please enter a Member ID to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("This will delete the member. Confirm?", "Delete Data",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string result = await ApiHelper.DeleteMember(int.Parse(txtDelete.Text));
                    var response = JsonConvert.DeserializeObject<dynamic>(result);

                    if (response["success"] == true)
                    {
                        MessageBox.Show("Member deleted successfully! ✅", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtDelete.Clear();
                        await LoadMembers();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete member.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteMember_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }
        }
    }
}