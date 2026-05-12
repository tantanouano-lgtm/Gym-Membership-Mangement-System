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

            // Disable Edit/Delete until a row is selected
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            // Full row selection, prevent direct cell editing
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            // Enable Edit/Delete buttons only when a row is clicked
            dataGridView1.SelectionChanged += (s, e) =>
            {
                bool hasRow = dataGridView1.SelectedRows.Count > 0;
                btnEdit.Enabled = hasRow;
                btnDelete.Enabled = hasRow;
            };
        }

        // ── FORM LOAD ───────────────────────────
        private async void ViewStaff_Load(object sender, EventArgs e)
        {
            await LoadStaff();

            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 5000;
            refreshTimer.Tick += async (s, ev) => await LoadStaff();
            refreshTimer.Start();
        }

        // ── LOAD STAFF DATA ─────────────────────
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

        // ── DELETE BUTTON ───────────────────────
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a staff to delete.",
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = (Staff)dataGridView1.SelectedRows[0].DataBoundItem;

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete '{selected.username}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    await ApiHelper.DeleteStaff(selected.id);
                    MessageBox.Show("Staff deleted successfully!", "Deleted",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadStaff(); // Refresh grid
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete failed: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ── EDIT BUTTON ─────────────────────────
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var selected = (Staff)dataGridView1.SelectedRows[0].DataBoundItem;

            EditStaff editForm = new EditStaff(selected);
            editForm.ShowDialog();

            await LoadStaff(); // Refresh grid after editing
        }

        // ── BACK BUTTON ─────────────────────────
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }

            this.Hide();
            Form1 mainForm = new Form1();
            mainForm.Show();
        }

        // ── FORM CLOSING — stop the timer ───────
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