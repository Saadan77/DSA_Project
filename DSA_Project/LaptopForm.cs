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
    public partial class LaptopForm : Form
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

        public LaptopForm()
        {
            
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            pnlNav.Height = btnBankLoan.Height;
            pnlNav.Top = btnBankLoan.Top;
            pnlNav.Left = btnBankLoan.Left;
            btnLaptopScheme.BackColor = Color.FromArgb(46, 51, 73);

            string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "select * from laptop";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(dataReader);
            StGridView.DataSource = dt;
            con.Close();
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void LaptopForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (double.Parse(STGpaTb.Text) <= 4 && double.Parse((STGpaTb.Text)) >= 3)
            {
                try
                {
                    string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
                    MySqlConnection con = new MySqlConnection();
                    con.ConnectionString = connstring;
                    con.Open();
                    string query = "INSERT INTO laptop (St_Id, St_Name, St_Father, St_Prog, St_Sem,St_Gpa) VALUES ('"+ StIdTb.Text +"','" + StNameTb.Text + "', '" + StFNameTb.Text + "','" + StProgComboBox.Text + "','" + StSemComboBox.Text + "' ,'" + STGpaTb.Text + "');";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();

                    string sql = "select * from laptop";
                    MySqlCommand cmd_1 = new MySqlCommand(sql, con);
                    MySqlDataReader reader = cmd_1.ExecuteReader();

                    DataTable data = new DataTable();
                    data.Load(reader);

                    StGridView.DataSource = data;
                    Update();Clear();
                    MessageBox.Show("Successfully Submitted");
                    con.Close();
                }
                catch (Exception)
                {
                    
                }
            }
            else
            {
                MessageBox.Show("Invalid GPA");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            StNameTb.Text = "";
            StFNameTb.Text = "";
            StProgComboBox.Text = "";
            StSemComboBox.Text = "";
            STGpaTb.Text = "";
        }

        private void btnLaptopScheme_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnLaptopScheme.Height;
            pnlNav.Top = btnLaptopScheme.Top;
            btnLaptopScheme.BackColor = Color.FromArgb(46, 51, 73);
        }

        private void btnBankLoan_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnBankLoan.Height;
            pnlNav.Top = btnBankLoan.Top;
            btnBankLoan.BackColor = Color.FromArgb(46, 51, 73);

            this.Close();
            Form1 f = new Form1();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "UPDATE laptop SET St_Name = '" + StNameTb.Text + "' , St_Father = '" + StFNameTb.Text + "' , St_Prog = '" + StProgComboBox.Text + "'  where St_Id = ( " + StIdTb.Text + " );";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();

            string sql = "select * from laptop";
            MySqlCommand cmd_1 = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd_1.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(reader);
            StGridView.DataSource = data;
            Update();
            Clear();
            MessageBox.Show("Successfully Updated");
            con.Close();
        }
        private new void Update()
        {
            StGridView.Update();
            StGridView.Refresh();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "delete from laptop where St_Id = ( " + StIdTb.Text + " );";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();

            string sql = "select * from laptop";
            MySqlCommand cmd_1 = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd_1.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(reader);

            StGridView.DataSource = data;
            Update();
            Clear();
            MessageBox.Show("Successfully Deleted");
            con.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnScholarship_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnScholarship.Height;
            pnlNav.Top = btnScholarship.Top;
            btnScholarship.BackColor = Color.FromArgb(46, 51, 73);

            this.Close();
            Form2 f = new Form2();
            f.Show();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
                MySqlConnection con = new MySqlConnection
                {
                    ConnectionString = connstring
                };
                con.Open();

                string query = "select * from laptop";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                MySqlDataReader Reader = cmd.ExecuteReader();

                List<int> laptop = new List<int>(); // arr_laptop
                List<int> gpa = new List<int>();//arr_gpa
                List<string> studentname = new List<string>(); // id
                List<int> totalItems = new List<int>();

                while (Reader.Read())
                {
                    int lap = 1;
                    laptop.Add(lap);

                    string name = Convert.ToString(Reader["St_Name"]);
                    studentname.Add(name);

                    int gpa_ = Convert.ToInt32(Reader["St_Gpa"]);
                    gpa.Add(gpa_);

                    int items = Convert.ToInt32(Reader["St_Id"]);
                    totalItems.Add(items);
                }

                var array_laptop = laptop.ToArray();
                var array_gpa = gpa.ToArray();
                var array_name = studentname.ToArray();
                var total_items = totalItems.ToArray();
                int n = total_items.Length - 1;

                int[,] data = new int[50, 6];

                const int maxLaptop = 5;

                int result = GetMaxInterest(n, maxLaptop, array_laptop, array_gpa, data);
                OutputNames(n, maxLaptop, array_name, array_laptop, array_gpa, data);

                con.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private static int GetMaxInterest(int n, int maxlaptop, int[] arr_laptop, int[] arr_gpa, int[,] data)
        {
            for (int studentNum = 0; studentNum <= n; studentNum++)
            {
                for (int currentlaptop = 0; currentlaptop <= maxlaptop; currentlaptop++)
                {
                    if (studentNum == 0 || currentlaptop == 0)
                    {
                        data[studentNum, currentlaptop] = 0;
                    }
                    else if (arr_laptop[studentNum] <= currentlaptop) // Loan of customer is less than or equal to current arr_laptop
                    {
                        data[studentNum, currentlaptop] = Math.Max(arr_gpa[studentNum] + data[studentNum - 1, currentlaptop - arr_laptop[studentNum]],
                                                                                data[studentNum - 1, currentlaptop]);
                    }
                    else
                    {
                        data[studentNum, currentlaptop] = data[studentNum - 1, currentlaptop];
                    }
                }
            }

            MessageBox.Show("\n  selected students: " + Convert.ToString(data[n, maxlaptop]));
            return data[n, maxlaptop];
        }

        private static void OutputNames(int n, int maxlaptop, string[] stdNames, int[] arr_stlaptop, int[] studentgpa, int[,] data)
        {
            int i = n;
            int j = maxlaptop;
            int maxInterest = 0;

            while (i > 0 && j > 0)
            {
                if (data[i, j] != data[i - 1, j])
                {
                    
                    MessageBox.Show($"Student Name: " + stdNames[i] + "\n\nGpa: " + studentgpa[i]);
                    maxInterest += studentgpa[i];
                    j = j - arr_stlaptop[i];
                }
                i--;
            }
        }

        private void StGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void StGridView_DoubleClick(object sender, EventArgs e)
        {
            int n = StGridView.SelectedRows[0].Index;
            StIdTb.Text = StGridView.Rows[n].Cells[0].Value.ToString();
            StNameTb.Text = StGridView.Rows[n].Cells[1].Value.ToString();
            StFNameTb.Text = StGridView.Rows[n].Cells[2].Value.ToString();
            StProgComboBox.Text = StGridView.Rows[n].Cells[3].Value.ToString();
            StSemComboBox.Text = StGridView.Rows[n].Cells[4].Value.ToString();
            STGpaTb.Text = StGridView.Rows[n].Cells[5].Value.ToString();
        }
    }
}
