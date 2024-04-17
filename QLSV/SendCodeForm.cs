using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class SendCodeForm : Form
    {
        public SendCodeForm()
        {
            InitializeComponent();
        }

       
        string randomCode;
        public static String to;



        private void SendCodeForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            if (txt_Email.Text.Trim() == "")
            {
                MessageBox.Show("Please enter your email address", "Add Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btn_Verify_Click(object sender, EventArgs e)
        {
            if (txt_Code.Text.Trim() =="")
            {
                MessageBox.Show("Please enter your code", "Forget Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (randomCode == txt_Code.Text.ToString()) 
            {
                to = txt_Email.Text;
                ResetPassword resetPassword = new ResetPassword();
                this.Hide();
                //resetPassword.typeAccount = 1;
                if (resetPassword.ShowDialog() == DialogResult.Cancel)
                {
                    this.Show();
                }
                
            }
            else
                MessageBox.Show("Wrong code");
        }
        private void labelReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
