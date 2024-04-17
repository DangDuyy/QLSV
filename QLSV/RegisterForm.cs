using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSV
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();

        }
        MY_DB myDB = new MY_DB();
        int time = 60;
        string randomCode;
        public static String to;

        

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            if (txt_Email.Text.Trim() == "")
            {
                MessageBox.Show("Please enter your email address", "Add Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (existEmail())
            {
                MessageBox.Show("Email already used, please enter another email", "Add Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string from, pass, messageBody;
            Random rand = new Random();
            randomCode = (rand.Next(999999)).ToString();
            MailMessage message = new MailMessage();
            to = txt_Email.Text.Trim();
            from = "duyprovn987@gmail.com"; //email cua ban
            pass = "jjci xjzr oyzg uwoo";  //pass cua ban
            messageBody = "Code: " + randomCode;
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = "Account creation code";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);
            try
            {
                smtp.Send(message);
                MessageBox.Show("Code send successfully", "Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private bool checkCode()
        {
            if (txt_Code.Text.Trim() == "")
            {
                MessageBox.Show("Please enter your code", "Forget Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (randomCode == txt_Code.Text.ToString()) {
                to = txt_Email.Text;
                return true;
            }
            else
            {
                MessageBox.Show("Wrong Code");
                return false;
            }
        }
      

        private void btn_Reg_Click(object sender, EventArgs e)
        {
            MY_DB mY = new MY_DB();
            SqlCommand cmd = new SqlCommand("Insert into Login(username, password, email) values (@us, @pass,@email)", myDB.getConnection);
            cmd.Parameters.Add("@us", SqlDbType.Char).Value = txt_Username.Text;
            cmd.Parameters.Add("@pass", SqlDbType.Char).Value = txt_Pass.Text;
            cmd.Parameters.Add("@email", SqlDbType.Char).Value = txt_Email.Text;

            if (checkInfor())
            {
                if (checkCode() == false) {
                    return;
                }
                if (txt_Pass.Text != txt_PrePass.Text)
                {
                    MessageBox.Show("Password authentication is wrong, please check again", "Create Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_PrePass.Text = "";
                    return;
                }
                myDB.openConnection();
                if (checkUserExist(txt_Username.Text.ToString().Trim()) == false)
                {
                    MessageBox.Show("This usename has already exist", "Create Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Account successfully created", "Create Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_Username.Text = "";
                    txt_Pass.Text = "";
                    txt_Email.Text = "";
                }
                else
                    MessageBox.Show("Registration error", "Create Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                myDB.closeConnection();
            }
            else
            {
                MessageBox.Show("Please don't leave information blank", "Create Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        private bool checkUserExist(string usn)
        {
            MY_DB db = new MY_DB(); 
            db.openConnection();

            SqlCommand cmd = new SqlCommand("Select * from Login where username = @username", db.getConnection);
            cmd.Parameters.Add("@username", SqlDbType.NChar).Value = usn;

            var result = cmd.ExecuteReader(); 
            if (result.HasRows) {
                db.closeConnection();
                return false;
            }
            db.closeConnection();
            return true;
        }

        private bool checkInfor()
        {
            if (txt_Username.Text.Trim() == "" ||
                txt_Pass.Text.Trim() == "" ||
                txt_PrePass.Text.Trim() == "" ||
                txt_Code.Text.Trim() == "")
            {
                return false;
            }
            return true;
        }

        private bool existEmail()
        {
            MY_DB db = new MY_DB();
            db.openConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Login where email = '" + txt_Email.Text.Trim() + "'", db.getConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable tb = new DataTable();
                adapter.Fill(tb);
                if (tb.Rows.Count > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR"+ ex.Message, "Add Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
