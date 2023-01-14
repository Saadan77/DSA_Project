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
    public partial class Form2 : Form
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

        public Form2()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            pnlNav.Height = btnScholarship.Height;
            pnlNav.Top = btnScholarship.Top;
            pnlNav.Left = btnScholarship.Left;
            btnScholarship.BackColor = Color.FromArgb(46, 51, 73);

            string connstring = "server = localhost; uid = root; pwd = 1234 ; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "select * from student_info";
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

                string query = "INSERT INTO student_info (student_id, student_name, student_schoarship, gpa, salary) VALUES ('" +studentID.Text+ "', '" + studentName.Text+ "','" + studentscholarship.Text+ "','" + gpaText.Text+ "' ,'" + salaryText.Text+ "');";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                string sql = "select * from student_info";
                MySqlCommand cmd_1 = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd_1.ExecuteReader();

                DataTable data = new DataTable();
                data.Load(reader);

                dataGridView1.DataSource = data;
                dataGridView1.Update();
                dataGridView1.Refresh();

                studentID.Text = "";
                studentscholarship.Text = "";
                studentName.Text = "";
                salary.Text = "";
                gpaText.Text = "";

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
                string connstring = "server = localhost; uid = root; pwd = 1234; database = student";
                MySqlConnection con = new MySqlConnection
                {
                    ConnectionString = connstring
                };
                con.Open();

                string query = "select * from student_info";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                MySqlDataReader Reader = cmd.ExecuteReader();

                List<int> studentscholarship = new List<int>();
                List<int> studentgpa = new List<int>();
                List<string> studentName = new List<string>();
                List<int> studentid= new List<int>();

                while (Reader.Read())
                {
                    int scholarship = Convert.ToInt32(Reader["student_scholarship"]);
                    studentscholarship.Add(scholarship);

                    string name = Convert.ToString(Reader["student_name"]);
                    studentName.Add(name);

                    int gpa = Convert.ToInt32(Reader["gpa"]);
                    studentgpa.Add(gpa);

                    int id = Convert.ToInt32(Reader["student_id"]);
                    studentid.Add(id);
                }

                var array_scholarship= studentscholarship.ToArray();
                var array_gpa= studentgpa.ToArray();
                var array_name = studentName.ToArray();
                var array_id = studentid.ToArray();
                int n = array_id.Length - 1;

                int[,] data = new int[25, 500001];

                const int maxscholarship= 500000;

                int result = Getmaxscholarship(n, maxscholarship, array_scholarship, array_gpa, data);
                OutputNames(n, maxscholarship, array_name, array_scholarship, array_gpa, data);

                con.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static int Getmaxscholarship(int n, int maxscholarship, int[] scholarship, int[] gpa, int[,] data)
        {
            for (int studentNum = 0; studentNum <= n; studentNum++)
            {
                for (int currentscholarship = 0; currentscholarship<= maxscholarship; currentscholarship++)
                {
                    if (studentNum == 0 || currentscholarship== 0)
                    {
                        data[studentNum, currentscholarship] = 0;
                    }
                    else if (scholarship[studentNum] <= currentscholarship) // Loan of student is less than or equal to current loan
                    {
                        data[studentNum, currentscholarship] = Math.Max(gpa[studentNum] + data[studentNum - 1, currentscholarship - scholarship[studentNum]],
                                                                                data[studentNum - 1, currentscholarship]);
                    }
                    else
                    {
                        data[studentNum, currentscholarship] = data[studentNum - 1, currentscholarship];
                    }
                }
            }

            MessageBox.Show("\nMax Scholarship: " + Convert.ToString(data[n, maxscholarship]));
            return data[n, maxscholarship];
        }

     private static void OutputNames(int n, int maxscholarship, string[] studentNames, int[] studentscholarships, int[] studentgpa, int[,] data)
        {
            int i = n;
            int j = maxscholarship;
          

            while (i > 0 && j > 0)
            {
                if (data[i, j] != data[i - 1, j])
                {
                    MessageBox.Show($"Student Name: " + studentNames[i] + "\n\n Scholarship: " + studentscholarships[i]);
                    maxscholarship += studentscholarships[i];
                    j = j - studentgpa[i];
                }
                i--;
            }
        }

       
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "delete from student_info where customer_id = ( " + studentID.Text + " );";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();

            string sql = "select * from student_info";
            MySqlCommand cmd_1 = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd_1.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(reader);

            dataGridView1.DataSource = data;
            dataGridView1.Update();
            dataGridView1.Refresh();

            studentID.Text = "";
            studentscholarship.Text = "";
            studentName.Text = "";
            gpaText.Text = "";
            salaryText.Text = "";

            MessageBox.Show("Successfully Deleted");
            con.Close();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "UPDATE student_info SET student_scholarship = '" + studentscholarship.Text + "' , gpa= '" + gpaText.Text + "' , father_salary = '" + salary.Text + "'  where student_id = ( " + studentID.Text + " );";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();

            string sql = "select * from student_info";
            MySqlCommand cmd_1 = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd_1.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(reader);

            dataGridView1.DataSource = data;
            dataGridView1.Update();
            dataGridView1.Refresh();

            studentID.Text = "";
            studentscholarship.Text = "";
            studentName.Text = "";
            gpaText.Text = "";
            salaryText.Text = "";

            MessageBox.Show("Successfully Updated");
            con.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            string connstring = "server = localhost; uid = root; pwd = 1234; database = customer";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connstring;
            con.Open();

            string query = "select * from student_info";
            MySqlCommand cmd_1 = new MySqlCommand(query, con);
            MySqlDataReader reader = cmd_1.ExecuteReader();

            studentID.Text = "";
            studentscholarship.Text = "";
            studentName.Text = "";
            gpaText.Text = "";
            salaryText.Text = "";

            con.Close();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int n = dataGridView1.SelectedRows[0].Index;
            studentID.Text = dataGridView1.Rows[n].Cells[0].Value.ToString();
            studentName.Text = dataGridView1.Rows[n].Cells[1].Value.ToString();
            studentscholarship.Text = dataGridView1.Rows[n].Cells[2].Value.ToString();  
            gpaText.Text = dataGridView1.Rows[n].Cells[3].Value.ToString();
            salaryText.Text = dataGridView1.Rows[n].Cells[4].Value.ToString();
        }

        private void studentscholarship_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void salaryText_TextChanged(object sender, EventArgs e)
        {
            double gpa = Convert.ToDouble(gpaText.Text);
            int salary = Convert.ToInt32(salaryText.Text);
            int scholarship;

            if (salary >= 20000 && salary <= 30000 && gpa >= 3.8)
            {
                scholarship = 200000;
                studentscholarship.Text = Convert.ToString(scholarship);
            }

            else if (salary >= 30000 && salary <= 40000 && gpa >= 3.7)
            {
                scholarship = 150000;
                studentscholarship.Text = Convert.ToString(scholarship);
            }

            else if (salary >= 40000 && salary <= 50000 && gpa >= 3.6)
            {
                scholarship = 100000;
                studentscholarship.Text = Convert.ToString(scholarship);
            }
        }
    }
}