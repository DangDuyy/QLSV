using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }
        public static int userID;

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private string GetSelectedUserRole()
        {
            if (radioHR.Checked)
                return radioHR.Text;
            if (radioStudent.Checked)
                return radioStudent.Text;
            if (radioAdmin.Checked)
                return radioAdmin.Text;
            return string.Empty; // Trả về chuỗi rỗng nếu không có lựa chọn nào được chọn
        }

        private void btt_Login_Click(object sender, EventArgs e)
        {
            MY_DB db = new MY_DB();

            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable table = new DataTable();
            string actor = GetSelectedUserRole().Trim();

            SqlCommand command = new SqlCommand("SELECT * FROM login WHERE username = @User AND password = @Pass", db.getConnection);

            command.Parameters.Add("@User", SqlDbType.VarChar).Value = TextBoxUsername.Text;
            command.Parameters.Add("@Pass", SqlDbType.VarChar).Value = TextBoxPassword.Text;
            adapter.SelectCommand = command;

            adapter.Fill(table);

            if ((table.Rows.Count > 0))
            {
                //MessageBox.Show("Ok, next time will be go to Main Menu of App");
                userID = Convert.ToInt32(table.Rows[0]["id"]);
                if (actor == "HR")
                {
                    SqlCommand hrCommand = new SqlCommand("SELECT * FROM hr WHERE uid = @id and uname = @username and pwd = @pass", db.getConnection);
                    hrCommand.Parameters.Add("@id", SqlDbType.Int).Value = userID;
                    hrCommand.Parameters.Add("@username", SqlDbType.VarChar).Value = TextBoxUsername.Text;
                    hrCommand.Parameters.Add("@pass", SqlDbType.VarChar).Value = TextBoxPassword.Text;
                    DataTable hrTable = new DataTable();
                    adapter.SelectCommand = hrCommand;
                    adapter.Fill(hrTable);
                    Welcome welcome = new Welcome();

                    if (hrTable.Rows.Count > 0)
                    {
                        byte[] pic;
                        if (!Convert.IsDBNull(hrTable.Rows[0]["fig"]))
                        {
                            pic = (byte[])hrTable.Rows[0]["fig"];
                            MemoryStream picture = new MemoryStream(pic);
                            welcome.pictureBox1.Image = Image.FromStream(picture);
                        }
                        else
                        {
                            // Handle the case when the image is NULL in the database
                            // For example, you can set a default image or leave the PictureBox empty
                            welcome.pictureBox1.Image = null; // or your default image
                        }
                        welcome.label2.Text = "Welcome HR: " + hrTable.Rows[0]["l_name"].ToString();
                        welcome.Show(this);
                    }
                    else
                    {
                        MessageBox.Show("Invalid HR User", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (actor == "Student")
                {
                    MainForm01 a = new MainForm01();
                    a.Show(this);
                }
                else if (actor == "Admin")
                {
                    SqlCommand adminCommand = new SqlCommand("SELECT * FROM admin WHERE uid = @id and uname = @username and pwd = @pass", db.getConnection);
                    adminCommand.Parameters.Add("@id", SqlDbType.Int).Value = userID;
                    adminCommand.Parameters.Add("@username", SqlDbType.VarChar).Value = TextBoxUsername.Text;
                    adminCommand.Parameters.Add("@pass", SqlDbType.VarChar).Value = TextBoxPassword.Text;
                    DataTable adminTable = new DataTable();
                    adapter.SelectCommand = adminCommand;
                    adapter.Fill(adminTable);

                    if (adminTable.Rows.Count > 0)
                    {
                        AdminForm welcome = new AdminForm();
                        welcome.Show(this);
                    }
                    else
                    {
                        MessageBox.Show("Invalid ADMIN User", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show("Please choose your role", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Invalid Username Or Password", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (TextBoxUsername.Text == string.Empty)
            {
                TextBoxUsername.Focus();
                errorProvider1.SetError(TextBoxUsername, "Please enter your username");
            }
            if (TextBoxPassword.Text == string.Empty)
            {
                TextBoxPassword.Focus();
                errorProvider1.SetError(TextBoxPassword, "Please enter your password");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
      
        }

        private void toolTip2_Popup(object sender, PopupEventArgs e)
        {

        }

        private void btt_Register_Click(object sender, EventArgs e)
        {
            RegisterForm StdRegister = new RegisterForm();
            StdRegister.Show(this);
        }

        private void ForgetPass_Click(object sender, EventArgs e)
        {
            SendCodeForm s = new SendCodeForm();
            s.Show(this);
        }
    }
}
