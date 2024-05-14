using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace QLSV
{
    public partial class ManageStudentForm : Form
    {
        public ManageStudentForm()
        {
            InitializeComponent();
        }
        MY_DB mydb = new MY_DB();
        STUDENT student = new STUDENT();
        private void ManageStudentForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLSVDBDataSet5.std' table. You can move, or remove it, as needed.
            //this.stdTableAdapter.Fill(this.qLSVDBDataSet5.std);
            // TODO: This line of code loads data into the 'qLSVDBDataSet4.course' table. You can move, or remove it, as needed.
            //this.courseTableAdapter.Fill(this.qLSVDBDataSet4.course);
            mydb.openConnection();

            // Tạo và thực thi câu lệnh SQL để cập nhật dữ liệu
            SqlCommand command = new SqlCommand("UPDATE std SET course = STUFF((SELECT ' ,' + CAST(score.course_Id AS VARCHAR(MAX)) FROM score WHERE score.student_id = std.id FOR XML PATH('')), 1, 2, '')", mydb.getConnection);

            command.ExecuteNonQuery(); // Thực thi câu lệnh

            // Đóng kết nối cơ sở dữ liệu
            mydb.closeConnection();
            //populate the datagridview with students data

            fillGrid(new SqlCommand("select id as MaSV, fname as 'Họ SV', lname as 'Tên SV', bdate as DOB, gender as 'Giới tính', phone as SDT, address as 'Địa chỉ',picture as 'Ảnh đại diện', email as Email, course as 'Mã Học Phần' from std "));

        }
        public void fillGrid(SqlCommand command)
        {

            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudents(command);
            picCol = (DataGridViewImageColumn)dataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;

            dataGridView1.AllowUserToAddRows = false;

            // show the total students 
            labeTotal.Text = "Total Students: " + dataGridView1.Rows.Count;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_ID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txt_fname.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txt_lname.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            DateTime dateValue;
            if (DateTime.TryParse(dataGridView1.CurrentRow.Cells[3].Value.ToString(), out dateValue))
            {
                DateTimePicker1.Value = dateValue;
            }
            else
            {
                // Xử lý nếu giá trị không hợp lệ
                // Ví dụ: Hiển thị một giá trị mặc định hoặc báo lỗi
                DateTimePicker1.Value = DateTime.Now; // Giá trị mặc định
            }

            if (dataGridView1.CurrentRow.Cells[4].Value.ToString().Trim() == "Female")
            {
                RadioButtonFemale.Checked = true;
            }
            else
            {
                RadioButtonMale.Checked = true;
            }


            TextBoxPhone.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            TextBoxAddress.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            byte[] pic;
            if (dataGridView1.CurrentRow.Cells[7].Value != DBNull.Value)
            {
                pic = (byte[])dataGridView1.CurrentRow.Cells[7].Value;
                try
                {
                    MemoryStream picture = new MemoryStream(pic);
                    PictureBoxStudentImage.Image = Image.FromStream(picture);
                }
                catch (ArgumentException ex)
                {
                    // Xử lý lỗi hoặc thông báo người dùng nếu không thể tạo hình ảnh
                    MessageBox.Show("Không thể tạo hình ảnh từ dữ liệu.");
                    // Xóa hình ảnh hiển thị trên PictureBox
                    PictureBoxStudentImage.Image = null;
                }
            }
            else
            {
                // Nếu giá trị hình ảnh là null, xóa hình ảnh hiển thị trên PictureBox
                PictureBoxStudentImage.Image = null;
            }
            // Kiểm tra xem cell được nhấp có phải là cell ở cột 9 hay không
            if (e.ColumnIndex == 9)
            {
                string courses = dataGridView1.CurrentRow.Cells[9].Value.ToString();

                DetailCourseList detailCourseListForm = new DetailCourseList(courses);
                detailCourseListForm.Show(this);

                // Mở form mới
                
            }
        }


        private void buttonReset_Click(object sender, EventArgs e)
        {
            txt_ID.Text = "";
            txt_lname.Text = "";
            TextBoxPhone.Text = "";
            txt_fname.Text = "";
            TextBoxAddress.Text = "";
            RadioButtonMale.Checked = true;
            DateTimePicker1.Value = DateTime.Now;
            PictureBoxStudentImage.Image = null;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string query = "select * FROM std where CONCAT(fname,lname,address) LIKE'%" + txtSearch.Text + "%'";
            SqlCommand command = new SqlCommand(query);
            fillGrid(command);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            SaveFileDialog svf = new SaveFileDialog();
            svf.FileName = "Student_" + txt_ID.Text;
            if (PictureBoxStudentImage.Image == null)
            {
                MessageBox.Show("No Image In the PictureBox");
            }
            else if (svf.ShowDialog() == DialogResult.OK)
            {
                PictureBoxStudentImage.Image.Save(svf.FileName + ("." + ImageFormat.Jpeg.ToString()));
            }
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

        private void ButtonAddStudent_Click(object sender, EventArgs e)
        {
            DateTimePicker1.CustomFormat = "dd/MM/yyyy";
            STUDENT student = new STUDENT();
            int id = Convert.ToInt32(txt_ID.Text);
            string fname = txt_fname.Text;
            string lname = txt_lname.Text;
            DateTime bdate = DateTimePicker1.Value.Date;
            string phone = TextBoxPhone.Text.Trim();
            string adrs = TextBoxAddress.Text;
            string gender = "Male";
            string pattern = @"\d";
            string email = txtEmail.Text;
            if (!phone.All(char.IsDigit))
            {
                MessageBox.Show("Phone number must contain only digits", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Regex.IsMatch(fname, pattern) || Regex.IsMatch(lname, pattern))
            {
                MessageBox.Show("First name and last name must be letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (RadioButtonFemale.Checked)
            {
                gender = "Female";
            }

            MemoryStream pic = new MemoryStream();
            int born_year = DateTimePicker1.Value.Year;
            int this_year = DateTime.Now.Year;
            //  sv tu 10-100,  co the thay doi
            if (((this_year - born_year) < 10) || ((this_year - born_year) > 100))
            {
                MessageBox.Show("The Student Age Must Be Between 10 and 100 year", "Invalid Birth Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verif())
            {
                PictureBoxStudentImage.Image.Save(pic, PictureBoxStudentImage.Image.RawFormat);
                if (student.insertStudentEmail(id, fname, lname, bdate, gender, phone, adrs, pic, email))
                {
                    MessageBox.Show("New Student Added", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fillGrid(new SqlCommand("select * from std "));
                }
                else
                {
                    MessageBox.Show("Error", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Empty Fields", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            DateTimePicker1.CustomFormat = "dd/MM/yyyy";
            STUDENT student = new STUDENT();
            int id;
            string fname = txt_fname.Text;
            string lname = txt_lname.Text;
            DateTime bdate = DateTimePicker1.Value.Date;
            string phone = TextBoxPhone.Text.Trim();
            string adrs = TextBoxAddress.Text;
            string gender = "Male";
            string pattern = @"\d";
            if (!phone.All(char.IsDigit))
            {
                MessageBox.Show("Phone number must contain only digits", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Regex.IsMatch(fname, pattern) || Regex.IsMatch(lname, pattern))
            {
                MessageBox.Show("First name and last name must be letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (RadioButtonFemale.Checked)
            {
                gender = "Female";
            }
            else if (RadioButtonMale.Checked)
                gender = "Male";

            MemoryStream pic = new MemoryStream();
            int born_year = DateTimePicker1.Value.Year;
            int this_year = DateTime.Now.Year;
            //  sv tu 10-100,  co the thay doi
            if (((this_year - born_year) < 10) || ((this_year - born_year) > 100))
            {
                MessageBox.Show("The Student Age Must Be Between 10 and 100 year", "Invalid Birth Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verif())
            {
                try
                {
                    id = Convert.ToInt32(txt_ID.Text);
                    PictureBoxStudentImage.Image.Save(pic, PictureBoxStudentImage.Image.RawFormat);
                    if (student.updateStudent(id, fname, lname, bdate, gender, phone, adrs, pic))
                    {
                        MessageBox.Show("Student information updated", "Edit Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        fillGrid(new SqlCommand("select * from std "));
                    }
                    else
                    {
                        MessageBox.Show("Error", "Edit Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Edit Student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Empty Fields", "Edit Student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            STUDENT student = new STUDENT();
            try
            {
                int studentID = Convert.ToInt32(txt_ID.Text);
                if ((MessageBox.Show("Are You Sure You Want To Delete This Student", "Delete Student", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    if (student.deleteStudent(studentID))
                    {
                        MessageBox.Show("Student Deleted", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        fillGrid(new SqlCommand("select * from std "));
                        txt_ID.Text = "";
                        txt_fname.Text = "";
                        txt_lname.Text = "";
                        TextBoxAddress.Text = "";
                        TextBoxPhone.Text = "";
                        DateTimePicker1.Value = DateTime.Now;
                        PictureBoxStudentImage.Image = null;
                    }
                    else
                    {
                        MessageBox.Show("Student Not Deleted", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please Enter A Valid ID", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        bool verif()
        {
            if ((txt_fname.Text.Trim() == "")
                        || (txt_lname.Text.Trim() == "")
                        || (TextBoxAddress.Text.Trim() == "")
                        || (TextBoxPhone.Text.Trim() == "")
                        || (PictureBoxStudentImage.Image == null))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

    }
}