using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
    public partial class EditContact : Form
    {
        public EditContact()
        {
            InitializeComponent();
        }
        MY_DB mydb = new MY_DB();

        public DataTable getData(SqlCommand cmd)
        {
            cmd.Connection = mydb.getConnection;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
        private void EditContact_Load(object sender, EventArgs e)
        {
            int userid = Login_Form.userID;
            SqlCommand cbb = new SqlCommand("select * from [group] where userid = @userid");
            cbb.Parameters.Add("@userid", SqlDbType.Int).Value = userid;
            cbb_group.DataSource = getData(cbb);
            cbb_group.DisplayMember = "Name";
            cbb_group.ValueMember = "id";
            cbb_group.SelectedItem = null;

        }
        /*private bool CheckContact(int id)
        {
            MY_DB db = new MY_DB();
            id =
            db.openConnection();
            SqlCommand cmd = new SqlCommand("Select * from contact where id = @id", db.getConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                return true; // Course with the given name and ID exists
            }
            else
            {
                return false; // Course with the given name and ID does not exist
            }
        }*/
        private void button1_Click(object sender, EventArgs e)
        {
            SelectContact select = new SelectContact();
            select.ShowDialog(this);
            int id = int.Parse(SelectContact.id);
            txt_id.Text = id.ToString();
            int userid = Login_Form.userID;
            //MessageBox.Show(id.ToString());
            SqlCommand cmd = new SqlCommand("Select * from contact where id = @id");
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            DataTable data = new DataTable();
            data = getData(cmd);


            txt_Fname.Text = data.Rows[0]["fname"].ToString();
            txt_lname.Text = data.Rows[0]["lname"].ToString();

            //cbb_group.Text = data.Rows[0]["group_id"].ToString();
            SqlCommand cbb = new SqlCommand("select * from [group] where userid = @userid and id = @id");
            cbb.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(data.Rows[0]["group_id"].ToString());
            cbb.Parameters.Add("@userid", SqlDbType.Int).Value = userid;
            DataTable dt = new DataTable();
            dt = getData(cbb);

            cbb_group.Text = dt.Rows[0]["name"].ToString();



            txt_phone.Text = data.Rows[0]["phone"].ToString();
            txt_email.Text = data.Rows[0]["email"].ToString();
            txt_address.Text = data.Rows[0]["address"].ToString();
            byte[] pic;
            pic = (byte[])data.Rows[0]["picture"];
            MemoryStream picture = new MemoryStream(pic);
            PictureBoxStudentImage.Image = Image.FromStream(picture);

        }
        private bool ContainsNumber(string text)
        {
            return text.Any(char.IsDigit);
        }

        private bool ContainsLetter(string text)
        {
            return text.Any(char.IsLetter);
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }
        private bool IsEmpty(string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }


        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (IsEmpty(txt_Fname.Text.Trim()) || IsEmpty(txt_lname.Text.Trim()) || IsEmpty(txt_phone.Text.Trim()) || IsEmpty(txt_email.Text.Trim()) || IsEmpty(txt_address.Text.Trim()) || IsEmpty(txt_id.Text.Trim()))
            {
                MessageBox.Show("All fields must be filled.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ContainsNumber(txt_Fname.Text.Trim()) || ContainsNumber(txt_lname.Text.Trim()))
            {
                MessageBox.Show("First name and last name should not contain numbers.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ContainsLetter(txt_phone.Text.Trim()))
            {
                MessageBox.Show("Phone number should not contain letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsNumeric(txt_id.Text.Trim()))
            {
                MessageBox.Show("ID should be a number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MemoryStream pic = new MemoryStream();
            PictureBoxStudentImage.Image.Save(pic, PictureBoxStudentImage.Image.RawFormat);

            SqlCommand cmd = new SqlCommand("UPDATE contact SET fname=@fname, lname=@lname, group_id=@group_id, phone=@phone, email=@email, address=@address, picture=@picture WHERE id=@id", mydb.getConnection);

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(txt_id.Text);
            cmd.Parameters.Add("@fname", SqlDbType.NVarChar, 50).Value = txt_Fname.Text.Trim(); // Giả sử fname có độ dài tối đa là 50 ký tự
            cmd.Parameters.Add("@lname", SqlDbType.NVarChar, 50).Value = txt_lname.Text.Trim(); // Giả sử lname có độ dài tối đa là 50 ký tự
            cmd.Parameters.Add("@group_id", SqlDbType.NVarChar).Value = cbb_group.SelectedValue; // Giả sử group_id là kiểu Int
            cmd.Parameters.Add("@phone", SqlDbType.VarChar, 20).Value = txt_phone.Text.Trim(); // Giả sử phone có độ dài tối đa là 20 ký tự
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = txt_email.Text.Trim(); // Giả sử email có độ dài tối đa là 100 ký tự
            cmd.Parameters.Add("@address", SqlDbType.NVarChar, 200).Value = txt_address.Text.Trim(); // Giả sử address có độ dài tối đa là 200 ký tự
            cmd.Parameters.Add("@picture", SqlDbType.Image).Value = pic.ToArray();

            mydb.openConnection();
            if (cmd.ExecuteNonQuery() == 1)
            {
                mydb.closeConnection();
                MessageBox.Show("Edit successful", "Edit Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                mydb.closeConnection();
                MessageBox.Show("Error", "Edit Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
