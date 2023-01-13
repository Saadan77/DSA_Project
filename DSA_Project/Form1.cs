using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DSA_Project
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        enum TextStyle
        {
            Normal,
            Success,
            Danger
        }

        public Form1()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            pnlNav.Height = btnBankLoan.Height;
            pnlNav.Top = btnBankLoan.Top;
            pnlNav.Left = btnBankLoan.Left;
            btnBankLoan.BackColor = Color.FromArgb(46, 51, 73);

            string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "select * from customer_info";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(dataReader);
            dataGridView1.DataSource = dt;

            con.Close();
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            pnlNav.Height = BtnDashboard.Height;
            pnlNav.Top = BtnDashboard.Top;
            pnlNav.Left = BtnDashboard.Left;
            BtnDashboard.BackColor = Color.FromArgb(46, 51, 73);
        }

        private void BtnBankLoan_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnBankLoan.Height;
            pnlNav.Top = btnBankLoan.Top;
            btnBankLoan.BackColor = Color.FromArgb(46, 51, 73);
        }

        private void BtnLaptopScheme_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnLaptopScheme.Height;
            pnlNav.Top = btnLaptopScheme.Top;
            btnLaptopScheme.BackColor = Color.FromArgb(46, 51, 73);
        }

        private void BtnScholarship_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnScholarship.Height;
            pnlNav.Top = btnScholarship.Top;
            btnScholarship.BackColor = Color.FromArgb(46, 51, 73);
        }

        private void BtnDashboard_Leave(object sender, EventArgs e)
        {
            BtnDashboard.BackColor = Color.FromArgb(24, 30, 54);
        }

        private void btnBankLoan_Leave(object sender, EventArgs e)
        {
            btnBankLoan.BackColor = Color.FromArgb(24, 30, 54);
        }

        private void btnLaptopScheme_Leave(object sender, EventArgs e)
        {
            btnLaptopScheme.BackColor = Color.FromArgb(24, 30, 54);
        }

        private void btnScholarship_Leave(object sender, EventArgs e)
        {
            btnScholarship.BackColor = Color.FromArgb(24, 30, 54);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
                MySqlConnection con = new MySqlConnection();
                con.ConnectionString = connstring;
                con.Open();

                string query = "INSERT INTO customer_info (customer_id, customer_name, customer_loan, interest, tenure) VALUES ('" +customerID.Text+ "', '" + customerName.Text+ "','" + customerLoan.Text+ "','" + interestText.Text+ "' ,'" + tenureComboBox.Text+ "');";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                string sql = "select * from customer_info";
                MySqlCommand cmd_1 = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd_1.ExecuteReader();

                DataTable data = new DataTable();
                data.Load(reader);

                dataGridView1.DataSource = data;
                dataGridView1.Update();
                dataGridView1.Refresh();

                customerID.Text = "";
                customerLoan.Text = "";
                customerName.Text = "";
                tenureComboBox.Text = "";
                interestText.Text = "";

                MessageBox.Show("Successfully Submitted");
                con.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnClick_Click(object sender, EventArgs e)
        {
            try
            {
                string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
                MySqlConnection con = new MySqlConnection
                {
                    ConnectionString = connstring
                };
                con.Open();

                string query = "select * from customer_info";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                MySqlDataReader Reader = cmd.ExecuteReader();

                List<int> customerLoan = new List<int>();
                List<int> customerInterest = new List<int>();
                List<string> customerName = new List<string>();
                List<int> totalItems = new List<int>();

                while (Reader.Read())
                {
                    int loan = Convert.ToInt32(Reader["customer_loan"]);
                    customerLoan.Add(loan);

                    string name = Convert.ToString(Reader["customer_name"]);
                    customerName.Add(name);

                    int interest = Convert.ToInt32(Reader["interest"]);
                    customerInterest.Add(interest);

                    int items = Convert.ToInt32(Reader["customer_id"]);
                    totalItems.Add(items);
                }

                var array_loan = customerLoan.ToArray();
                var array_interest = customerInterest.ToArray();
                var array_name = customerName.ToArray();
                var total_items = totalItems.ToArray();
                int n = total_items.Length - 1;

                int[,] data = new int[100, 1000001];

                const int maxLoan = 100000;

                int result = GetMaxInterest(n, maxLoan, array_loan, array_interest, data);
                OutputNames(n, maxLoan, array_name, array_interest, data);

                con.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static int GetMaxInterest(int n, int maxLoan, int[] loan, int[] interest, int[,] data)
        {
            for (int customerNum = 0; customerNum <= n; customerNum++)
            {
                for (int currentLoan = 0; currentLoan <= maxLoan; currentLoan++)
                {
                    if (customerNum == 0 || currentLoan == 0)
                    {
                        data[customerNum, currentLoan] = 0;
                    }
                    else if (loan[customerNum] <= currentLoan) // Loan of customer is less than or equal to current loan
                    {
                        data[customerNum, currentLoan] = Math.Max(interest[customerNum] + data[customerNum - 1, currentLoan - loan[customerNum]],
                                                                                data[customerNum - 1, currentLoan]);
                    }
                    else
                    {
                        data[customerNum, currentLoan] = data[customerNum - 1, currentLoan];
                    }
                }
            }

            MessageBox.Show("\nMax Interest: " + Convert.ToString(data[n, maxLoan]));
            return data[n, maxLoan];
        }

        private static void OutputNames(int n, int maxLoan, string[] customerNames, int[] customerInterests, int[,] data)
        {
            int i = n;
            int j = maxLoan;
            int maxInterest = 0;
            List<int> no_interests = new List<int>();
            List<string> no_names = new List<string>();

            while (i > 0)
            {
                if (data[i, j] != data[i - 1, j])
                {
                    MessageBox.Show($"Customer Names: \n" + customerNames[i] + "\n\nInterest: " + customerInterests[i]);
                    maxInterest += customerInterests[i];            
                }
                i--;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int loan = Convert.ToInt32(customerLoan.Text);
            int tenure = Convert.ToInt32(this.tenureComboBox.GetItemText(this.tenureComboBox.SelectedItem));
            int interest;

            if (tenure.Equals(1))
            {
                interest = (loan * 8) / 100;
                interestText.Text = Convert.ToString(interest);
            }

            else if (tenure.Equals(2))
            {
                interest = (loan * 10) / 100;
                interestText.Text = Convert.ToString(interest);
            }

            else if (tenure.Equals(3))
            {
                interest = (loan * 12) / 100;
                interestText.Text = Convert.ToString(interest);
            }

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "delete from customer_info where customer_id = ( " + customerID.Text + " );";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();

            string sql = "select * from customer_info";
            MySqlCommand cmd_1 = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd_1.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(reader);

            dataGridView1.DataSource = data;
            dataGridView1.Update();
            dataGridView1.Refresh();

            customerID.Text = "";
            customerLoan.Text = "";
            customerName.Text = "";
            interestText.Text = "";
            tenureComboBox.Text = "";

            MessageBox.Show("Successfully Deleted");
            con.Close();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "UPDATE customer_info SET customer_loan = '" + customerLoan.Text + "' , interest = '" + interestText.Text + "' , tenure = '" + tenureComboBox.Text + "'  where customer_id = ( " + customerID.Text + " );";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();

            string sql = "select * from customer_info";
            MySqlCommand cmd_1 = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd_1.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(reader);

            dataGridView1.DataSource = data;
            dataGridView1.Update();
            dataGridView1.Refresh();

            customerID.Text = "";
            customerLoan.Text = "";
            customerName.Text = "";
            interestText.Text = "";
            tenureComboBox.Text = "";

            MessageBox.Show("Successfully Updated");
            con.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "select * from customer_info";
            MySqlCommand cmd_1 = new MySqlCommand(query, con);
            MySqlDataReader reader = cmd_1.ExecuteReader();

            customerID.Text = "";
            customerLoan.Text = "";
            customerName.Text = "";
            interestText.Text = "";
            tenureComboBox.Text = "";

            con.Close();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int n = dataGridView1.SelectedRows[0].Index;
            customerID.Text = dataGridView1.Rows[n].Cells[0].Value.ToString();
            customerName.Text = dataGridView1.Rows[n].Cells[1].Value.ToString();
            customerLoan.Text = dataGridView1.Rows[n].Cells[2].Value.ToString();  
            interestText.Text = dataGridView1.Rows[n].Cells[3].Value.ToString();
            tenureComboBox.Text = dataGridView1.Rows[n].Cells[4].Value.ToString();
        }
    }
}