using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class DetailStudentInformation : Form
    {
        public DetailStudentInformation()
        {
            InitializeComponent();
        }
        MY_DB mydb = new MY_DB();

        private void txt_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ButtonUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((ofd.ShowDialog() == DialogResult.OK))
            {
                pictureBox2.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void ButtonRemoveStudent_Click(object sender, EventArgs e)
        {
            //delete student 
            try
            {
                int studentId = Convert.ToInt32(txt_id.Text);
                //display a confirmation message before the delete
                if ((MessageBox.Show("Are You Sure You Want To Delete This Account", "Delete Account", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    if (deleteStudent(studentId))
                    {
                        MessageBox.Show("Account Deleted", "Delete Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //clear field
                        txt_id.Text = "";
                        txt_fname.Text = "";
                        txt_lname.Text = "";
                        txt_username.Text = "";
                        txt_pass.Text = "";
                        pictureBox2.Image = null;
                    }
                    else
                    {
                        MessageBox.Show("Account Deleted", "Delete Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please Enter A Valid ID", "Delete Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        // verify data
        bool verif()
        {
            if ((txt_id.Text.Trim() == "")
                || (txt_fname.Text.Trim() == "")
                || (txt_lname.Text.Trim() == "")
                || (txt_username.Text.Trim() == "")
                || (txt_pass.Text.Trim() == "")
                || (pictureBox2.Image == null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        AdminForm admin = new AdminForm();
        //edit student 
        private void ButtonEditStudent_Click(object sender, EventArgs e)
        {
            int id;
            string fname = txt_fname.Text;
            string lname = txt_lname.Text;
            string username = txt_username.Text.Trim();
            string pass = txt_pass.Text;
            string pattern = @"\d";
            if (Regex.IsMatch(fname, pattern) || Regex.IsMatch(lname, pattern))
            {
                MessageBox.Show("First name and last name must be letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MemoryStream pic = new MemoryStream();
            if (verif())
            {
                try
                {
                    id = Convert.ToInt32(txt_id.Text);

                    pictureBox2.Image.Save(pic, pictureBox2.Image.RawFormat);
                    if (updateStudent(id, fname, lname, username, pass, pic))
                    {
                        MessageBox.Show("Account Information Updated", "Edit Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Updated Account not successful", "Edit Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("ID must be a number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Edit Account", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


            }
            else
            {
                MessageBox.Show("Empty Fields", "Edit Account", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        public bool updateStudent(int Id, string fname, string lname, string username, string pass, MemoryStream picture)
        {
            SqlCommand command = new SqlCommand("UPDATE hr SET f_name=@fn, l_name=@ln, uname=@user, pwd=@pass, fig=@pic WHERE uid=@id", mydb.getConnection);

            command.Parameters.Add("@id", SqlDbType.Int).Value = Id;
            command.Parameters.Add("@fn", SqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", SqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@user", SqlDbType.VarChar).Value = username;
            command.Parameters.Add("@pass", SqlDbType.VarChar).Value = pass;
            command.Parameters.Add("@pic", SqlDbType.Image).Value = picture.ToArray();

            mydb.openConnection();

            if ((command.ExecuteNonQuery() == 1))
            {
                mydb.closeConnection();
                return true;
            }
            else
            {
                mydb.closeConnection();
                return false;
            }
        }
        public bool deleteStudent(int id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM hr WHERE uid = @id", mydb.getConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            mydb.openConnection();
            if ((cmd.ExecuteNonQuery() == 1))
            {
                mydb.closeConnection();
                return true;
            }
            else
            {
                mydb.closeConnection();
                return false;
            }
        }
    }
}
