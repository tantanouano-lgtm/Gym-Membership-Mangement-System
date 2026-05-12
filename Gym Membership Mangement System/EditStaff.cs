using System;
using System.Windows.Forms;

namespace Gym_Membership_Mangement_System
{
    public partial class EditStaff : Form
    {
        private Staff _staff;

        public EditStaff(Staff staff)
        {
            InitializeComponent();
            _staff = staff;

            txtUsername.Text = staff.username;
            txtFullname.Text = staff.fullname;
            txtAddress.Text = staff.address;
            txtPhone.Text = staff.phone;
            cmbLevel.SelectedIndex = staff.level;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullname.Text))
            {
                MessageBox.Show("Username and Full Name cannot be empty.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _staff.username = txtUsername.Text.Trim();
            _staff.fullname = txtFullname.Text.Trim();
            _staff.address = txtAddress.Text.Trim();
            _staff.phone = txtPhone.Text.Trim();
            _staff.level = cmbLevel.SelectedIndex;

            try
            {
                await ApiHelper.UpdateStaff(_staff.id, _staff);
                MessageBox.Show("Staff updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}