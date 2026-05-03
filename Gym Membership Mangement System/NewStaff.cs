using System;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Gym_Membership_Mangement_System
{
    public partial class NewStaff : Form
    {
        public NewStaff()
        {
            InitializeComponent();
        }

        private void NewStaff_Load_1(object sender, EventArgs e) { }

        // SAVE BUTTON
        private async void button1_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtFname.Text) ||
                string.IsNullOrWhiteSpace(txtLname.Text) ||
                string.IsNullOrWhiteSpace(txtMobile.Text))
            {
                MessageBox.Show(
                    "Please fill in First Name, Last Name, and Mobile.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Build data matching your users table columns
            var staffData = new
            {
                fullname = txtFname.Text.Trim() + " " + txtLname.Text.Trim(),
                username = (txtFname.Text.Trim() + txtLname.Text.Trim()).ToLower().Replace(" ", ""),
                phone = txtMobile.Text.Trim(),
                address = txtCity.Text.Trim() + ", " + txtState.Text.Trim(),
                password = "Staff@123"
            };

            try
            {
                button1.Enabled = false;
                button1.Text = "Saving...";

                string response = await ApiHelper.AddStaff(staffData);
                dynamic result = JsonConvert.DeserializeObject(response);

                if (result.success == true)
                {
                    MessageBox.Show(
                        "Staff added successfully!\n\nUsername: " + staffData.username +
                        "\nDefault Password: Staff@123",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Open ViewStaff — loads fresh data automatically
                    ViewStaff vs = new ViewStaff();
                    vs.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Error: " + result.message,
                        "Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot connect. Make sure XAMPP is running.\n\n" + ex.Message,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
                button1.Text = "Save";
            }
        }

        // RESET BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            txtFname.Clear();
            txtLname.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            txtMobile.Clear();
            txtEmail.Clear();
            txtState.Clear();
            txtCity.Clear();
            dateTimePickerDOB.Value = DateTime.Now;
            dateTimePickerJOINDate.Value = DateTime.Now;
        }

        // VIEW STAFF BUTTON
        private void button3_Click(object sender, EventArgs e)
        {
            ViewStaff vs = new ViewStaff();
            vs.Show();
        }

        private void txtFname_TextChanged(object sender, EventArgs e) { }
        private void txtLname_TextChanged(object sender, EventArgs e) { }
        private void txtMobile_TextChanged(object sender, EventArgs e) { }
        private void radioButton1_CheckedChanged(object sender, EventArgs e) { }
        private void radioButton2_CheckedChanged(object sender, EventArgs e) { }
        private void dateTimePickerDOB_ValueChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
    }
}