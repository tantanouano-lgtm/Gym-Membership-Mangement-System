using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gym_Membership_Mangement_System
{
    public partial class ViewEquipment : Form
    {
        private System.Windows.Forms.Timer refreshTimer;

        public ViewEquipment()
        {
            InitializeComponent();
        }

        private async void ViewEquipment_Load(object sender, EventArgs e)
        {
            // Set DataGridView properties
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;

            await LoadEquipment();

            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 5000;
            refreshTimer.Tick += async (s, ev) => await LoadEquipment();
            refreshTimer.Start();
        }

        private async Task LoadEquipment()
        {
            try
            {
                string result = await ApiHelper.GetEquipment();
                var wrapper = JsonConvert.DeserializeObject<EquipmentResponse>(result);

                if (wrapper != null && wrapper.success && wrapper.data != null)
                {
                    if (dataGridView1.InvokeRequired)
                    {
                        dataGridView1.Invoke(new Action(() => {
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = wrapper.data;
                        }));
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = wrapper.data;
                    }
                }
                else
                {
                    MessageBox.Show("No equipment data found.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load equipment: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DELETE BUTTON
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an equipment to delete.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int equipmentId = Convert.ToInt32(selectedRow.Cells["id"].Value);
            string equipmentName = selectedRow.Cells["equipment_name"].Value.ToString();

            var confirm = MessageBox.Show(
                "Are you sure you want to delete \"" + equipmentName + "\"?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.No) return;

            try
            {
                string result = await ApiHelper.DeleteEquipment(equipmentId);
                dynamic response = JsonConvert.DeserializeObject(result);

                if (response.success == true)
                {
                    MessageBox.Show("Equipment deleted successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadEquipment();
                }
                else
                {
                    MessageBox.Show("Failed to delete: " + response.message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewEquipment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }
        }
    }
}