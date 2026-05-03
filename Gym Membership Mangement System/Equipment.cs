using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Gym_Membership_Mangement_System
{
    public partial class Equipment : Form
    {
        public Equipment()
        {
            InitializeComponent();
        }

        private void Equipment_Load(object sender, EventArgs e)
        {

        }

        // SAVE BUTTON
        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEquipName.Text))
            {
                MessageBox.Show("Please enter Equipment Name.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCost.Text))
            {
                MessageBox.Show("Please enter Cost.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtCost.Text, out decimal cost))
            {
                MessageBox.Show("Please enter a valid number for Cost.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Please enter Quantity.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Please enter a valid whole number for Quantity.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var equipmentData = new
                {
                    equipment_name = txtEquipName.Text,
                    description = txtDescription.Text,
                    muscles_used = txtMuscleUsed.Text,
                    delivery_date = dateTimePickerDeliveryDate.Value.ToString("yyyy-MM-dd"),
                    cost = txtCost.Text,
                    quantity = txtQuantity.Text
                };

                string json = JsonConvert.SerializeObject(equipmentData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    string url = "http://localhost/GMS/api/equipment.php";
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();

                    dynamic responseData = JsonConvert.DeserializeObject(result);

                    if (responseData.success == true)
                    {
                        MessageBox.Show("Equipment saved successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        txtEquipName.Clear();
                        txtDescription.Clear();
                        txtMuscleUsed.Clear();
                        txtCost.Clear();
                        txtQuantity.Clear();
                        dateTimePickerDeliveryDate.Value = DateTime.Now;
                    }
                    else
                    {
                        MessageBox.Show("Failed to save: " + responseData.message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // RESET BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            txtEquipName.Clear();
            txtDescription.Clear();
            txtMuscleUsed.Clear();
            txtCost.Clear();
            txtQuantity.Clear();
            dateTimePickerDeliveryDate.Value = DateTime.Now;
        }

        // VIEW EQUIPMENT BUTTON
        private void button3_Click(object sender, EventArgs e)
        {
            ViewEquipment ve = new ViewEquipment();
            ve.Show();
        }

        private void txtCost_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerDeliveryDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Equipment_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}