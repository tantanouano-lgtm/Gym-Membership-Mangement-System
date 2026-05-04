using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gym_Membership_Mangement_System
{
    public partial class ViewStaff : Form
    {
        private System.Windows.Forms.Timer refreshTimer;

        public ViewStaff()
        {
            InitializeComponent();
        }

        private async void ViewStaff_Load(object sender, EventArgs e)
        {
            await LoadStaff();

            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 5000;
            refreshTimer.Tick += async (s, ev) => await LoadStaff();
            refreshTimer.Start();
        }

        private async Task LoadStaff()
        {
            try
            {
                string result = await ApiHelper.GetStaff();
                var staff = JsonConvert.DeserializeObject<List<Staff>>(result);

                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() => {
                        dataGridView1.DataSource = staff;
                    }));
                }
                else
                {
                    dataGridView1.DataSource = staff;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load staff: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewStaff_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }
        }
    }
}