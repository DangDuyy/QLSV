using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;

namespace QLSV
{
    public partial class studentsListForm : Form
    {
        public String tmp;
        public studentsListForm()
        {
            InitializeComponent();
        }

        STUDENT student = new STUDENT();
        private void studentsListForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLSVDBDataSet3.std' table. You can move, or remove it, as needed.
            //this.stdTableAdapter.Fill(this.QLSVDBDataSet3.std);
            SqlCommand command = new SqlCommand("SELECT * FROM std");
            DataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            DataGridView1.RowTemplate.Height = 80;
            DataGridView1.DataSource = student.getStudents(command);
            picCol = (DataGridViewImageColumn)DataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            DataGridView1.AllowUserToAddRows = false;
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM std");
            DataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            DataGridView1.RowTemplate.Height = 80;
            DataGridView1.DataSource = student.getStudents(command);
            picCol = (DataGridViewImageColumn)DataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            DataGridView1.AllowUserToAddRows = false;
        }


        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateDeleteStudent updateDeletStdF = new UpdateDeleteStudent();
            updateDeletStdF.TextBoxID.Text = DataGridView1.CurrentRow.Cells[0].Value.ToString();
            updateDeletStdF.TextBoxFname.Text = DataGridView1.CurrentRow.Cells[1].Value.ToString();
            updateDeletStdF.TextBoxLname.Text = DataGridView1.CurrentRow.Cells[2].Value.ToString();
            updateDeletStdF.DateTimePicker1.Value = (DateTime)DataGridView1.CurrentRow.Cells[3].Value;

            // Check for the gender and set the appropriate RadioButton
            if (DataGridView1.CurrentRow.Cells[4].Value.ToString().Trim() == "Female")
            {
                updateDeletStdF.RadioButtonFemale.Checked = true;
            }
            else
            {
                updateDeletStdF.RadioButtonMale.Checked = true;
            }

            updateDeletStdF.TextBoxPhone.Text = DataGridView1.CurrentRow.Cells[5].Value.ToString();
            updateDeletStdF.TextBoxAddress.Text = DataGridView1.CurrentRow.Cells[6].Value.ToString();

            byte[] pic;
            if (!Convert.IsDBNull(DataGridView1.CurrentRow.Cells[7].Value))
            {
                pic = (byte[])DataGridView1.CurrentRow.Cells[7].Value;
                MemoryStream picture = new MemoryStream(pic);
                updateDeletStdF.PictureBoxStudentImage.Image = Image.FromStream(picture);
            }
            else
            {
                // Handle the case when the image is NULL in the database
                // For example, you can set a default image or leave the PictureBox empty
                updateDeletStdF.PictureBoxStudentImage.Image = null; // or your default image
            }


            updateDeletStdF.Show();
            tmp = updateDeletStdF.TextBoxID.Text;
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                DataTable dataTable = new DataTable();
                using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'"))
                {
                    conn.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", conn);
                    adapter.Fill(dataTable);
                    conn.Close();
                }

                DataGridView1.DataSource = dataTable;

                using (SqlConnection sqlConn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QLSVDB;Integrated Security=True"))
                {
                    sqlConn.Open();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        int id;
                        if (int.TryParse(row["Mã SV"].ToString(), out id))
                        {
                            string fname = row["Họ"].ToString();
                            string lname = row["Tên"].ToString();

                            string pattern = @"^[\p{L}\s]+$"; // Matches any Unicode letter or whitespace character

                            if (!Regex.IsMatch(fname, pattern) || !Regex.IsMatch(lname, pattern))
                            {
                                MessageBox.Show("First name and last name must only contain letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue; // Skip the record if the name contains non-letter characters
                            }

                            SqlCommand checkIdCmd = new SqlCommand("SELECT COUNT(*) FROM std WHERE id = @id", sqlConn);
                            checkIdCmd.Parameters.AddWithValue("@id", id);
                            int idExists = (int)checkIdCmd.ExecuteScalar();

                            if (idExists == 0)
                            {
                                string query = "INSERT INTO std (id, fname, lname, bdate, gender, phone, address, email) VALUES (@id, @fname, @lname, @bdate, @gender, @phone, @address, @email)";
                                using (SqlCommand cmd = new SqlCommand(query, sqlConn))
                                {
                                    cmd.Parameters.AddWithValue("@id", id);
                                    cmd.Parameters.AddWithValue("@fname", fname);
                                    cmd.Parameters.AddWithValue("@lname", lname);


                                    DateTime bdate;
                                    if (DateTime.TryParseExact(row["Ngày sinh"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out bdate))
                                    {
                                        cmd.Parameters.AddWithValue("@bdate", bdate);
                                    }
                                    else
                                    {
                                        continue; // Skip the record if the date is not valid
                                    }

                                    cmd.Parameters.AddWithValue("@gender", row["Giới tính"]);
                                    cmd.Parameters.AddWithValue("@phone", row["SDT"]);
                                    cmd.Parameters.AddWithValue("@address", row["Địa chỉ"]);

                                    // Tạo địa chỉ email từ Mã SV và thêm vào câu lệnh SQL
                                    string email = row["Mã SV"].ToString() + "@student.hcmute.edu.vn";
                                    cmd.Parameters.AddWithValue("@email", email);
                                    
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("ID must be a number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue; // Skip the record if the ID is not a number
                        }
                    }
                    sqlConn.Close();
                }
            }

            SqlCommand command = new SqlCommand("SELECT * FROM std");
            DataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            DataGridView1.RowTemplate.Height = 80;
            DataGridView1.DataSource = student.getStudents(command);
            picCol = (DataGridViewImageColumn)DataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            DataGridView1.AllowUserToAddRows = false;
        }


        private void btn_Clear_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu từ DataGridView
            DataGridView1.DataSource = null;

            // Xóa dữ liệu từ cơ sở dữ liệu
            string sqlConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QLSVDB;Integrated Security=True";
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM std", sqlConnection);
                cmd.ExecuteNonQuery();
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
