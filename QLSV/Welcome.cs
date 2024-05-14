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
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace QLSV
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }
        MY_DB mydb = new MY_DB();
        STUDENT student = new STUDENT();
        private void Welcome_Load(object sender, EventArgs e)
        {
            refresh();
        }
        void refresh()
        {
            SqlCommand cmd = new SqlCommand("Select name from [group] where userid = @userid");
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Login_Form.userID;
            cbb_select_group_remoce.DataSource = getData(cmd);
            cbb_select_group_remoce.DisplayMember = "name";
            cbb_select_group_remoce.ValueMember = "name";
            cbb_select_group_remoce.SelectedItem = null;

            cbb_group.DataSource = getData(cmd);
            cbb_group.DisplayMember = "name";
            cbb_group.ValueMember = "name";
            cbb_group.SelectedItem = null;
        }
        public DataTable getData(SqlCommand cmd)
        {
            cmd.Connection = mydb.getConnection;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
        private bool CheckUserExist(int id)
        {
            MY_DB db = new MY_DB();
            id = Convert.ToInt32(txt_idGroup.Text);
            db.openConnection();
            SqlCommand cmd = new SqlCommand("Select * from [group] where id = @id", db.getConnection);
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
        }
        private void btt_addGroup_Click(object sender, EventArgs e)
        {

        }

        private void btt_edit_Click(object sender, EventArgs e)
        {

        }
        public bool deleteGroup(int userid, string name)
        {
            SqlCommand command = new SqlCommand("DELETE FROM [group] Where userid = @id AND name = @name", mydb.getConnection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = userid;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            mydb.openConnection();
            if ((command.ExecuteNonQuery() == 1))
                return true;
            else
            {
                return false;
            }
        }
        private void btt_removeGroup_Click(object sender, EventArgs e)
        {


        }

        private void btt_addContact_Click(object sender, EventArgs e)
        {

        }

        private void btt_editContact_Click(object sender, EventArgs e)
        {
            EditContact editContact = new EditContact();
            editContact.Show(this);
        }
        public bool deleteContac(int userid)
        {
            SqlCommand command = new SqlCommand("DELETE FROM contact Where id = @id", mydb.getConnection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = userid;
            mydb.openConnection();
            if ((command.ExecuteNonQuery() == 1))
                return true;
            else
            {
                return false;
            }
        }
        private void btt_removeContact_Click(object sender, EventArgs e)
        {

        }

        private void btt_Select_Click(object sender, EventArgs e)
        {

        }

        private void btt_Show_Click(object sender, EventArgs e)
        {

        }

        private void btt_removeContact_Click_1(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txt_contacId.Text);
            if (deleteContac(id))
            {
                MessageBox.Show("Remove contact successful", "Edit Group", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_contacId.Text = null;
            }
            else
            {
                mydb.closeConnection();
                MessageBox.Show("Error", "Remove Group", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AddContact addContact = new AddContact();
            addContact.Show(this);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            EditContact editContact = new EditContact();
            editContact.Show(this);
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (txt_idGroup.Text == null)
            {
                MessageBox.Show("Please choose Group ID");
            }

            SqlCommand sqlCommand = new SqlCommand("INSERT INTO [group] (id, name, userid) VALUES (@id, @name, @userid)", mydb.getConnection);
            sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(txt_idGroup.Text);
            sqlCommand.Parameters.Add("@name", SqlDbType.NVarChar).Value = txt_groupName.Text;
            sqlCommand.Parameters.Add("@userid", SqlDbType.Int).Value = Login_Form.userID;
            mydb.openConnection();
            if (!CheckUserExist(Convert.ToInt32(txt_idGroup.Text)))
            {
                if (sqlCommand.ExecuteNonQuery() == 1)
                {

                    mydb.closeConnection();
                    MessageBox.Show("Add Group Succesful");
                    refresh();
                    txt_groupName.Text = string.Empty;
                    txt_idGroup.Text = string.Empty;


                }
                else
                {
                    mydb.closeConnection();
                    MessageBox.Show("Add Group failed");
                }
            }
            else
            {
                MessageBox.Show("ID Available", "Add", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (cbb_select_group_remoce.Text != null)
            {
                SqlCommand cmd = new SqlCommand("Update [group] SET name = @name1 where name= @name2", mydb.getConnection);
                cmd.Parameters.Add("@name1", SqlDbType.NVarChar).Value = txt_newGroup.Text;
                cmd.Parameters.Add("@name2", SqlDbType.NVarChar).Value = cbb_group.Text;
                mydb.openConnection();
                if ((cmd.ExecuteNonQuery() == 1))
                {
                    mydb.closeConnection();
                    MessageBox.Show("Edit Group successful", "Edit Group", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refresh();
                    txt_newGroup.Text = string.Empty;
                }
                else
                {
                    mydb.closeConnection();
                    MessageBox.Show("Error", "Edit Group", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            int x = Login_Form.userID;
            string name = cbb_select_group_remoce.Text;
            if (deleteGroup(x, name))
            {
                MessageBox.Show("Remove Group successful", "Edit Group", MessageBoxButtons.OK, MessageBoxIcon.Information);
                refresh();
                cbb_select_group_remoce = null;
            }
            else
            {
                mydb.closeConnection();
                MessageBox.Show("Error", "Remove Group", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            SelectContact slct = new SelectContact();


            slct.ShowDialog();
            txt_contacId.Text = SelectContact.id;
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            ShowFullList showFullList = new ShowFullList();
            showFullList.Show(this);
        }
        String tmp;
        private void btn_info_Click(object sender, EventArgs e)
        {
            DetailStudentInformation detailStudent = new DetailStudentInformation();

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QLSVDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string query = "SELECT * FROM hr WHERE uid = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", Login_Form.userID);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    detailStudent.txt_id.Text = reader["uid"].ToString();
                    detailStudent.txt_fname.Text = reader["f_name"].ToString();
                    detailStudent.txt_lname.Text = reader["l_name"].ToString();
                    detailStudent.txt_username.Text = reader["uname"].ToString();
                    detailStudent.txt_pass.Text = reader["pwd"].ToString();

                    if (!Convert.IsDBNull(reader["fig"]))
                    {
                        byte[] pic = (byte[])reader["fig"];
                        MemoryStream picture = new MemoryStream(pic);
                        detailStudent.pictureBox2.Image = System.Drawing.Image.FromStream(picture);
                    }
                    else
                    {
                        detailStudent.pictureBox2.Image = null; // or your default image
                    }
                }

                reader.Close();
            }
            detailStudent.ButtonRemoveStudent.Visible = false;
            detailStudent.Show();
            tmp = detailStudent.txt_id.Text;
        }


    }
}
