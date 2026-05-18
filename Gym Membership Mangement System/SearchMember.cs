using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gym_Membership_Mangement_System
{
    public partial class SearchMember : Form
    {
        private System.Windows.Forms.Timer refreshTimer;
        private List<Member> allMembers = new List<Member>();
        private System.Windows.Forms.Timer paymentTimer;
        private string connStr = "server=localhost;database=gms_db;uid=root;pwd=;";

        public SearchMember()
        {
            InitializeComponent();
        }

        private async void SearchMember_Load(object sender, EventArgs e)
        {
            StartPaymentListener();
            LoadPaymentHistory("");

            await LoadMembers();

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

                LoadPaymentHistory(keyword);
            }
            else
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = allMembers;

                LoadPaymentHistory("");
            }
        }

        private void SearchMember_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }
            if (paymentTimer != null)
            {
                paymentTimer.Stop();
                paymentTimer.Dispose();
            }
        }

        private void SearchMember_FormClosing_1(object sender, FormClosingEventArgs e)
        {

        }

        private void StartPaymentListener()
        {
            paymentTimer = new System.Windows.Forms.Timer();
            paymentTimer.Interval = 10000;
            paymentTimer.Tick += CheckNewPayments;
            paymentTimer.Start();
        }

        private void CheckNewPayments(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Just mark all unnotified payments as notified silently
                    string updateQuery = "UPDATE payments SET is_notified = 1 WHERE is_notified = 0";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.ExecuteNonQuery();

                    // Refresh payment history after checking
                    LoadPaymentHistory(txtSearch.Text.ToLower());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void LoadPaymentHistory(string keyword)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query;
                    MySqlCommand cmd;

                    if (keyword == "")
                    {
                        query = @"
                            SELECT 
                                m.fullname AS Member,
                                p.type AS Plan,
                                p.amount AS Amount,
                                p.total AS Total,
                                p.created_at AS Date
                            FROM payments p
                            JOIN members m ON p.member = m.id
                            ORDER BY p.created_at DESC";

                        cmd = new MySqlCommand(query, conn);
                    }
                    else
                    {
                        query = @"
                            SELECT 
                                m.fullname AS Member,
                                p.type AS Plan,
                                p.amount AS Amount,
                                p.total AS Total,
                                p.created_at AS Date
                            FROM payments p
                            JOIN members m ON p.member = m.id
                            WHERE m.fullname LIKE @keyword
                            OR m.id LIKE @keyword
                            ORDER BY p.created_at DESC";

                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    }

                    MySqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    if (dgvPayments.InvokeRequired)
                    {
                        dgvPayments.Invoke(new Action(() =>
                        {
                            dgvPayments.DataSource = dt;
                        }));
                    }
                    else
                    {
                        dgvPayments.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Payment history error: " + ex.Message);
            }
        }

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
    }
}