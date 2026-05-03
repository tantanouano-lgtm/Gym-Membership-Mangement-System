using System;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Gym_Membership_Mangement_System
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            cmbSex.Items.AddRange(new[] { "Male", "Female" });
            cmbSex.SelectedIndex = 0;
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            string fullname = txtFullname.Text.Trim();
            string username = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string sex = cmbSex.SelectedItem?.ToString() ?? "";
            string birthdate = dtpBirthdate.Value.ToString("yyyy-MM-dd");
            string address = txtAddress.Text.Trim();
            string password = txtPassword.Text;
            string confirm = txtConfirm.Text;

            if (string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("All fields are required.", "Missing Fields",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (phone.Length != 11 || !phone.StartsWith("09"))
            {
                MessageBox.Show("Phone must be 11 digits starting with 09.",
                    "Invalid Phone", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters.",
                    "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Passwords do not match.", "Mismatch",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtConfirm.Clear();
                txtPassword.Focus();
                return;
            }

            btnRegister.Enabled = false;
            btnRegister.Text = "Creating...";

            try
            {
                var payload = new
                {
                    username = username,
                    fullname = fullname,
                    phone = phone,
                    address = address,
                    password = password,
                    confirm_password = confirm
                };

                string result = await ApiHelper.RegisterMember(payload);
                dynamic data = JsonConvert.DeserializeObject(result);

                if ((bool)data.success)
                {
                    MessageBox.Show(
                        "Account created!\nYou can now log in with your username and password.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show((string)data.message, "Registration Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot reach server. Make sure XAMPP is running.\n\n" + ex.Message,
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegister.Enabled = true;
                btnRegister.Text = "Create Account";
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}