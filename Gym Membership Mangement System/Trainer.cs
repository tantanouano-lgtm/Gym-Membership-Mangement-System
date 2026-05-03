using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Gym_Membership_Mangement_System
{
    public partial class Trainer : Form
    {
        public Trainer()
        {
            InitializeComponent();
        }

        private void New_Member_Load(object sender, EventArgs e) { }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(inputFirstName.Text))
            {
                MessageBox.Show("Please enter Fullname!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(inputMobile.Text))
            {
                MessageBox.Show("Please enter Mobile!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string gender = radioButton1.Checked ? radioButton1.Text : radioButton2.Text;

            // Calculate age from DOB
            DateTime dob = dateTimePickerDOB.Value;
            int age = DateTime.Now.Year - dob.Year;
            if (DateTime.Now.DayOfYear < dob.DayOfYear) age--;

            var trainerData = new
            {
                fullname = inputFirstName.Text + " " + inputLastName.Text,
                age = age,
                gender = gender,
                mobile = inputMobile.Text,
                email = inputEmail.Text,
                address = inputAddress.Text,
                hire_date = dateTimePickerJoinDate.Value.ToString("yyyy-MM-dd"),
                salary = string.IsNullOrWhiteSpace(textBox1.Text) ? "0" : textBox1.Text
            };

            try
            {
                this.Cursor = Cursors.WaitCursor;
                string result = await ApiHelper.AddTrainer(trainerData);
                var response = JsonConvert.DeserializeObject<dynamic>(result);
                this.Cursor = Cursors.Default;

                if (response["success"] == true)
                {
                    MessageBox.Show("Trainer saved successfully! ✅\nNow visible on the web app.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to save. Please try again.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("Error: " + ex.Message, "API Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            inputFirstName.Clear();
            inputLastName.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            inputMobile.Clear();
            inputEmail.Clear();
            inputAddress.Clear();
            textBox1.Clear();
            dateTimePickerDOB.Value = DateTime.Now;
            dateTimePickerJoinDate.Value = DateTime.Now;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void dateTimePickerDOB_ValueChanged(object sender, EventArgs e) { }
    }
}