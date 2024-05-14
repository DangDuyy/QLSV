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
    public partial class AddContact : Form
    {
        public AddContact()
        {
            InitializeComponent();
        }
        MY_DB db = new MY_DB();
        int userid = Login_Form.userID;
        private void AddContact_Load(object sender, EventArgs e)
        {

            SqlCommand cmd = new SqlCommand("select * from [group] where userid = @userid");
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;
            cbb_group.DataSource = getData(cmd);
            cbb_group.DisplayMember = "Name";
            cbb_group.ValueMember = "id";
            cbb_group.SelectedItem = null;
        }
        public DataTable getData(SqlCommand cmd)
        {
            cmd.Connection = db.getConnection;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
        private bool IsValidInput()
        {
            // Check if fname and lname contain only letters
            if (!txt_Fname.Text.All(char.IsLetter) || !txt_lname.Text.All(char.IsLetter))
            {
                MessageBox.Show("First name and last name should only contain letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if phone contains only digits
            if (!txt_phone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Phone number should only contain digits.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ButtonAddStudent_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Insert into contact (id,fname,lname, group_id, phone,email,address,picture,userid) " +
                "values (@id,@fname,@lname, @group_id, @phone,@email,@address,@picture,@userid)", db.getConnection);

            MemoryStream pic = new MemoryStream();
            PictureBoxStudentImage.Image.Save(pic, PictureBoxStudentImage.Image.RawFormat);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(txt_id.Text);
            cmd.Parameters.Add("@fname", SqlDbType.NVarChar).Value = txt_Fname.Text;
            cmd.Parameters.Add("@lname", SqlDbType.NVarChar).Value = txt_lname.Text;
            cmd.Parameters.Add("@group_id", SqlDbType.NVarChar).Value = cbb_group.SelectedValue;
            cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = txt_phone.Text;
            cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = txt_email.Text;
            cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = txt_address.Text;
            cmd.Parameters.Add("@picture", SqlDbType.Image).Value = pic.ToArray();
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;

            if (checkInfor() && IsValidInput())
            {
                db.openConnection();
                if (!CheckID(Convert.ToInt32(txt_id.Text)))
                {
                    MessageBox.Show("This username or ID has already existed", "Create Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Account successfully created", "Create Account,", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_id.Text = "";
                    txt_Fname.Text = "";
                    txt_lname.Text = string.Empty;
                    txt_email.Text = string.Empty;
                    txt_phone.Text = string.Empty;
                    txt_address.Text = string.Empty;
                    cbb_group.Text = string.Empty;

                    PictureBoxStudentImage.Image = null;
                }
                else
                {
                    MessageBox.Show("Registration error", "Create Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    db.closeConnection();
                }
            }
            else
            {
                //MessageBox.Show("Please do not leave information blank", "Create Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private bool CheckID(int usn)
        {
            MY_DB db = new MY_DB();
            db.openConnection();
            SqlCommand cmd = new SqlCommand("Select * from login where id= @id", db.getConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = usn;
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                db.closeConnection();
                return false;
            }
            db.closeConnection();
            return true;
        }
        private bool CheckUserExist(string usn)
        {
            MY_DB db = new MY_DB();
            db.openConnection();
            SqlCommand cmd = new SqlCommand("Select * from log_in where username = @username", db.getConnection);
            cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = usn;
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                db.closeConnection();
                return false;
            }
            db.closeConnection();
            return true;
        }

        private bool checkInfor()
        {
            if (txt_id.Text.Trim() == "")
            {
                return false;
            }
            return true;
        }

        private void ButtonUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                PictureBoxStudentImage.Image = Image.FromFile(opf.FileName);
            }
        }
    }
}
