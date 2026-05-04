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
            LoadPaymentHistory(""); // load all payment history on startup

            // Load all members immediately
            await LoadMembers();

            // Auto-refresh every 5 seconds
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

                // Also filter payment history by the search keyword
                LoadPaymentHistory(keyword);
            }
            else
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = allMembers;

                // Show all payment history when search is cleared
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

                    string query = @"
                        SELECT p.id, m.fullname, p.amount, p.type
                        FROM payments p
                        JOIN members m ON p.member = m.id
                        WHERE p.is_notified = 0
                        ORDER BY p.id DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    var newPayments = new List<(int id, string name, decimal amount, string type)>();

                    while (reader.Read())
                    {
                        newPayments.Add((
                            reader.GetInt32("id"),
                            reader.GetString("fullname"),
                            reader.GetDecimal("amount"),
                            reader.GetString("type")
                        ));
                    }
                    reader.Close();

                    foreach (var payment in newPayments)
                    {
                        MessageBox.Show(
                            $"Member: {payment.name}\nPlan: {payment.type}\nAmount: ₱{payment.amount:N2}\n\nPayment has been received!",
                            "✅ New Payment",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        string updateQuery = "UPDATE payments SET is_notified = 1 WHERE id = @id";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@id", payment.id);
                        updateCmd.ExecuteNonQuery();
                    }

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
                        // Show all payments
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
                        // Filter payments by member name or member id
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
    }
}