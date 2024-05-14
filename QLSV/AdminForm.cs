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
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }
        private SqlDataAdapter adapter;
        private DataSet dataSet;
        STUDENT student = new STUDENT();
        SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QLSVDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private void RefreshDataGridView(DataGridView dataGridView1)
        {
            try
            {
                connection.Open();
                string query = "SELECT uid as MãHR, f_name as Họ, l_name as Tên, uname as TênĐN, pwd as Password, fig as Avatar FROM UnAprrovestd ORDER BY l_name";
                adapter = new SqlDataAdapter(query, connection);
                dataSet = new DataSet();
                adapter.Fill(dataSet, "UnAprrovestd");
                dataGridView1.DataSource = dataSet.Tables["UnAprrovestd"];
                TotalStudentlbl.Text = (" Total Students: " + (dataGridView1.Rows.Count));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void RefreshDataGridView2(DataGridView dataGridView2)
        {
            try
            {
                connection.Open();
                string query = "SELECT uid as MãHR, f_name as Họ, l_name as Tên, uname as TênĐN, pwd as Password, fig as Avatar FROM hr ORDER BY uid";
                adapter = new SqlDataAdapter(query, connection);
                dataSet = new DataSet();
                adapter.Fill(dataSet, "hr");
                dataGridView2.DataSource = dataSet.Tables["hr"];
                label1.Text = (" Total Account: " + (dataGridView2.Rows.Count));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        MY_DB mydb = new MY_DB();
        private void AdminApproveStudent_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'myDBDataSet5.UnAprrovestd' table. You can move, or remove it, as needed.
            SqlCommand command = new SqlCommand("select uid as MãHR, f_name as Họ, l_name as Tên, uname as TênĐN, pwd as Password, fig as Avatar from UnAprrovestd", mydb.getConnection);
            dataGridView1.DataSource = student.getStudents(command);
            SqlCommand cmd = new SqlCommand("select uid as MãHR, f_name as Họ, l_name as Tên, uname as TênĐN, pwd as Password, fig as Avatar from hr", mydb.getConnection);
            dataGridView2.DataSource = student.getStudents(cmd);

            label1.Text = (" Total Account: " + (dataGridView2.Rows.Count));
            TotalStudentlbl.Text = (" Total Account: " + (dataGridView1.Rows.Count));


            dataGridView2.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView2.RowTemplate.Height = 80;
            picCol = (DataGridViewImageColumn)dataGridView2.Columns[5];
            picCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView2.AllowUserToAddRows = false;

            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol1 = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            picCol1 = (DataGridViewImageColumn)dataGridView1.Columns[5];
            picCol1.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AllowUserToAddRows = false;

        }
        private void ApproveStudent(int studentId)
        {
            try
            {
                connection.Open();
                string updateQuery = "INSERT INTO hr (uid, f_name, l_name, uname, pwd, fig) " +
                     "SELECT uid, f_name, l_name, uname, pwd, fig " +
                     $"FROM UnAprrovestd WHERE uid = {studentId}";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.ExecuteNonQuery();

                string deleteQuery = $"DELETE FROM UnAprrovestd WHERE uid = {studentId}";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    // Show your custom error message
                    MessageBox.Show("Không thể thêm Account với ID đã có, vui lòng đổi để duyệt", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Re-throw the exception if it's not a constraint violation
                    throw;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private void RejectStudent(int studentId)
        {
            try
            {
                connection.Open();
                string deleteQuery = $"DELETE FROM UnAprrovestd WHERE uid = {studentId}";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }


        }
        private void ApproveButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                //int selectedStudentId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                //  ApproveStudent(selectedStudentId);
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    // Lấy ID của sinh viên từ cột "stt" trong hàng
                    if (row.Cells[0].Value != null)
                    {
                        int selectedStudentId = (int)row.Cells[0].Value;
                        ApproveStudent(selectedStudentId);
                    }
                }
                RefreshDataGridView(dataGridView1);
                RefreshDataGridView2(dataGridView2);

            }
            else
            {
                MessageBox.Show("Please select a student to approve.");
            }


        }

        private void RejectButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                //int selectedStudentId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                // RejectStudent(selectedStudentId);
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    // Lấy ID của sinh viên từ cột "stt" trong hàng
                    if (row.Cells[0].Value != null)
                    {
                        int selectedStudentId = (int)row.Cells[0].Value;
                        RejectStudent(selectedStudentId);
                    }
                }
                RefreshDataGridView(dataGridView1);
                RefreshDataGridView2(dataGridView2);
            }
            else
            {
                MessageBox.Show("Please select a account to reject.");
            }
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGridView(dataGridView1);
            RefreshDataGridView2(dataGridView2);
            searchtxt.Text = " ";
            ChoiceBox.Text = " ";
        }

        private void AllAbtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Lấy ID của sinh viên từ cột "stt" trong hàng
                if (row.Cells[0].Value != null)
                {
                    int studentId = (int)row.Cells[0].Value;
                    ApproveStudent(studentId);
                }
            }

            // Cập nhật DataGridView sau khi từ chối tất cả sinh viên
            RefreshDataGridView(dataGridView1);
            RefreshDataGridView2(dataGridView2);

        }

        private void AllRbtn_Click(object sender, EventArgs e)
        {
            // Lặp qua tất cả các hàng trong DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Lấy ID của sinh viên từ cột "stt" trong hàng
                if (row.Cells[0].Value != null)
                {
                    int studentId = (int)row.Cells[0].Value;
                    RejectStudent(studentId);
                }
            }

            // Cập nhật DataGridView sau khi từ chối tất cả sinh viên
            RefreshDataGridView(dataGridView1);
            RefreshDataGridView2(dataGridView2);

        }

        public void fillGrid(SqlCommand command)
        {
            STUDENT student = new STUDENT();
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudents(command);
            picCol = (DataGridViewImageColumn)dataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AllowUserToAddRows = false;

            // dem sinh vien
            TotalStudentlbl.Text = (" Total Students: " + dataGridView1.Rows.Count);
        }
        private void searchButton_Click(object sender, EventArgs e)
        {
            string selectedAttribute = ChoiceBox.SelectedItem.ToString().Trim(); // Lấy thuộc tính được chọn từ ComboBox
            string searchValue = searchtxt.Text;

            string query = ""; // Câu lệnh SQL sẽ được xây dựng dựa trên thuộc tính được chọn

            // Xây dựng câu lệnh SQL dựa trên thuộc tính được chọn
            switch (selectedAttribute)
            {
                case "UID":
                    query = "SELECT * FROM UnAprrovestd WHERE Id LIKE '%" + searchValue + "%'";
                    break;
                case "FName":
                    query = "SELECT * FROM UnAprrovestd WHERE fname LIKE '%" + searchValue + "%'";
                    break;
                case "LName":
                    query = "SELECT * FROM UnAprrovestd WHERE lname LIKE '%" + searchValue + "%'";
                    break;
                default:
                    // Xử lý trường hợp không xác định
                    break;
            }

            SqlCommand command = new SqlCommand(query);
            fillGrid(command);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //DetailStudentInformation detailStudent = new DetailStudentInformation();
            //// Truyền dữ liệu từ DataGridView vào các Label trong DetailStudentInformation
            //detailStudent.mssvlbl.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            //string fullName = dataGridView1.CurrentRow.Cells[1].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[2].Value.ToString();
            //detailStudent.tenlbl.Text = fullName;
            //DateTime ngaySinh = (DateTime)dataGridView1.CurrentRow.Cells[3].Value;
            //detailStudent.ngaysinhlbl.Text = ngaySinh.ToString("dd/MM/yyyy");
            ////detailStudent.ngaysinhlbl.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();

            //string gender = dataGridView1.CurrentRow.Cells[4].Value.ToString().Trim();
            //detailStudent.gioitinhlbl.Text = gender;

            //detailStudent.phonelbl.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            //detailStudent.addresslbl.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            //detailStudent.emaillbl.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            //// Xử lý hình ảnh
            //if (dataGridView1.CurrentRow.Cells[7].Value != DBNull.Value)
            //{
            //    byte[] pic = (byte[])dataGridView1.CurrentRow.Cells[7].Value;
            //    MemoryStream pictureStream = new MemoryStream(pic);
            //    detailStudent.pictureBox2.Image = Image.FromStream(pictureStream);
            //}
            //else
            //{
            //    // Nếu không có hình ảnh, bạn có thể đặt một hình ảnh mặc định hoặc xóa PictureBox
            //    detailStudent.pictureBox2.Image = null; // Hoặc đặt một hình ảnh mặc định
            //}

            //// Hiển thị Form DetailStudentInformation
            //detailStudent.Show();
        }

        private void TotalStudentlbl_Click(object sender, EventArgs e)
        {

        }
        string tmp;
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DetailStudentInformation detailStudent = new DetailStudentInformation();
            detailStudent.txt_id.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            detailStudent.txt_fname.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            detailStudent.txt_lname.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            detailStudent.txt_username.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            detailStudent.txt_pass.Text = dataGridView2.CurrentRow.Cells[4].Value.ToString();

            byte[] pic;
            if (!Convert.IsDBNull(dataGridView2.CurrentRow.Cells[5].Value))
            {
                pic = (byte[])dataGridView2.CurrentRow.Cells[5].Value;
                MemoryStream picture = new MemoryStream(pic);
                detailStudent.pictureBox2.Image = Image.FromStream(picture);
            }
            else
            {
                // Handle the case when the image is NULL in the database
                // For example, you can set a default image or leave the PictureBox empty
                detailStudent.pictureBox2.Image = null; // or your default image
            }
            detailStudent.Show();
            tmp = detailStudent.txt_id.Text;

        }
    }
}
