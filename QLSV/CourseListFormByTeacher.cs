using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class CourseListFormByTeacher : Form
    {
        COURSE course = new COURSE();
        SCORE score = new SCORE();
        MY_DB mydb = new MY_DB();
        public CourseListFormByTeacher(string label)
        {
            InitializeComponent();
            txt_Course.Text = label;

            dataGridView1.DataSource = score.getScoreWithLabel(label);


        }
        private PrintDocument printDoc = new PrintDocument();


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txt_Sem_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CourseListFormByTeacher_Load(object sender, EventArgs e)
        {
            SqlCommand cmd1 = new SqlCommand("select std.id, std.fname, std.lname, std.bdate " +
                        "from std INNER JOIN score on score.student_id = std.id inner join course on score.course_id = CAST(course.id AS INT) " +
                        "where course.label=@cid", mydb.getConnection);
            SqlCommand sem = new SqlCommand("select semester,teacher_id from course where label=@cid", mydb.getConnection);
            cmd1.Parameters.Add("@cid", SqlDbType.VarChar).Value = txt_Course.Text;
            sem.Parameters.Add("@cid", SqlDbType.VarChar).Value = txt_Course.Text;
            mydb.openConnection();
            SqlDataReader reader1 = sem.ExecuteReader();

            if (reader1.HasRows)
            {
                reader1.Read();
                int semester = reader1.GetInt32(0);
                int teacherid = reader1.GetInt32(1);
                txt_Semester.Text = semester.ToString();
                txt_TeacherID.Text = teacherid.ToString();
            }
            else
            {
                // Handle the case where the query returned no results
            }
            reader1.Close();
            mydb.closeConnection();



            SqlCommand cmd = new SqlCommand("select * from course where label=@cid", mydb.getConnection);
            SqlCommand command = new SqlCommand("select fname,lname,phone from contact where id=@id", mydb.getConnection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(txt_TeacherID.Text);
            cmd.Parameters.Add("@cid", SqlDbType.VarChar).Value = txt_Course.Text;
            mydb.openConnection();
            SqlDataReader reader = cmd.ExecuteReader();
            string courseid = "";
            int period = 0;
            string descriptionn = "";
            string fname = "";
            string lname = "";
            string phone = "";
            if (reader.HasRows)
            {
                reader.Read();
                courseid = reader.GetString(0);
                period = reader.GetInt32(2);
                descriptionn = reader.GetString(3);

            }
            else
            {
                // Handle the case where the query returned no results
            }
            reader.Close();
            SqlDataReader rider = command.ExecuteReader();
            if (rider.HasRows)
            {
                rider.Read();
                fname = rider.GetString(0);
                lname = rider.GetString(1);
                phone = rider.GetString(2);
            }
            rider.Close();
            mydb.closeConnection();
            title.Text = "Bảng điểm sinh viên môn " + txt_Course.Text;
            label_ID.Text = "Mã học phần: " + courseid;
            lb_sem.Text = "Học kỳ: " + txt_Semester.Text;
            lb_Period.Text = "Số tiết: " + period.ToString();
            lb_des.Text = "Mô tả: " + descriptionn;
            lb_teacher.Text = "Giáo viên hướng dẫn: " + fname + " " + lname;
            lb_phone.Text = "SĐT: " + phone;
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            printPreviewDialog.ShowDialog();
            //PrintDialog printDialog = new PrintDialog();
            //printDialog.Document = printDoc;
            //if (printDialog.ShowDialog() == DialogResult.OK)
            //{
            //    printDoc.Print();
            //}
        }
        //private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        //{
        //    SqlCommand cmd = new SqlCommand("select * from course where label=@cid", mydb.getConnection);
        //    SqlCommand command = new SqlCommand("select fname,lname from contact where id=@id", mydb.getConnection);
        //    command.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(txt_TeacherID.Text);
        //    cmd.Parameters.Add("@cid", SqlDbType.VarChar).Value = txt_Course.Text;
        //    mydb.openConnection();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    string courseid = "";
        //    int period = 0;
        //    string descriptionn = "";
        //    string fname= "";
        //    string lname = "";
        //    if (reader.HasRows)
        //    {
        //        reader.Read();
        //        courseid = reader.GetString(0);
        //        period = reader.GetInt32(2);
        //        descriptionn = reader.GetString(3);
        //    }
        //    else
        //    {
        //        // Handle the case where the query returned no results
        //    }
        //    reader.Close();
        //    SqlDataReader rider = command.ExecuteReader();
        //    if (rider.HasRows)
        //    {
        //        rider.Read();
        //        fname = rider.GetString(0);
        //        lname = rider.GetString(1);
        //    }
        //    rider.Close();
        //    mydb.closeConnection();
        //    // Set the font and color
        //    Font font = new Font("Arial", 12, FontStyle.Bold);
        //    SolidBrush brush = new SolidBrush(Color.Black);

        //    // Set the text to print
        //    string university = "TRƯỜNG ĐẠI HỌC SƯ PHẠM KỸ THUẬT TP HCM";
        //    string courseList = "BẢNG ĐIỂN SINH VIÊN MÔN " + txt_Course.Text;

        //    // Calculate the center of the page
        //    float pageCenter = e.PageBounds.Width / 2;

        //    // Calculate the starting point of the strings
        //    float universityStart = pageCenter - (e.Graphics.MeasureString(university, font).Width / 2);
        //    float courseListStart = pageCenter - (e.Graphics.MeasureString(courseList, font).Width / 2);

        //    // Load the logo
        //    string filePath = @"C:\fit23-hk2\WINDOW\bt\QLSV\QLSV\QLSV\Resources\FIT2.jpg";
        //    Image logo = Image.FromFile(filePath);

        //    // Draw the logo at the top left
        //    e.Graphics.DrawImage(logo, 10, 10, logo.Width, logo.Height);

        //    // Set the signature text
        //    string signature = "Signature: ____________________";

        //    // Calculate the starting point of the signature
        //    float signatureStartX = e.PageBounds.Width - e.Graphics.MeasureString(signature, font).Width;
        //    float signatureStartY = e.PageBounds.Height - font.Height;

        //    // Draw the signature at the bottom right
        //    e.Graphics.DrawString(signature, font, brush, signatureStartX, signatureStartY);


        //    // Draw the text onto the Graphics object
        //    e.Graphics.DrawString(university, font, brush, universityStart, 20);
        //    e.Graphics.DrawString(courseList, font, brush, courseListStart, 120);

        //    // Set the additional text to print
        //    string courseCode = "Mã Học Phần: " + courseid;
        //    string semester = "Học kỳ: " + txt_Semester.Text;
        //    string periods = "Số Tiết: " + period.ToString();
        //    string description = "Mô Tả: " + descriptionn;
        //    string fullname = "Giáo viên hướng dẫn: " + fname + " " + lname;


        //    // Set the font for the additional text
        //    Font additionalTextFont = new Font("Arial", 8, FontStyle.Regular);

        //    // Calculate the starting point of the additional text
        //    float additionalTextStartX = pageCenter - (e.Graphics.MeasureString(courseCode + " " + semester + " " + periods + " " + description + " " + fullname, additionalTextFont).Width / 4) - 80;
        //    float additionalTextStartY = 160;

        //    // Draw the additional text onto the Graphics object
        //    e.Graphics.DrawString(courseCode, additionalTextFont, brush, additionalTextStartX, additionalTextStartY);
        //    e.Graphics.DrawString(semester, additionalTextFont, brush, additionalTextStartX, additionalTextStartY + 20);

        //    additionalTextStartX = pageCenter + (e.Graphics.MeasureString(courseCode + " " + semester + " " + periods +  " " + description + " " + fullname, additionalTextFont).Width / 4) - 80;

        //    e.Graphics.DrawString(periods, additionalTextFont, brush, additionalTextStartX, additionalTextStartY);
        //    e.Graphics.DrawString(description, additionalTextFont, brush, additionalTextStartX, additionalTextStartY + 20);
        //    e.Graphics.DrawString(fullname, additionalTextFont, brush, additionalTextStartX, additionalTextStartY + 40);


        //    // Starting point for the DataGridView
        //    int startX = (e.PageBounds.Width - dataGridView1.Width) / 2;
        //    int startY = 220;

        //    // Draw column headers
        //    for (int i = 0; i < dataGridView1.Columns.Count; i++)
        //    {
        //        e.Graphics.FillRectangle(Brushes.LightGray, startX, startY, dataGridView1.Columns[i].Width, dataGridView1.Rows[0].Height);
        //        e.Graphics.DrawRectangle(Pens.Black, startX, startY, dataGridView1.Columns[i].Width, dataGridView1.Rows[0].Height);
        //        e.Graphics.DrawString(dataGridView1.Columns[i].HeaderText, dataGridView1.Font, Brushes.Black, startX, startY);
        //        startX += dataGridView1.Columns[i].Width;
        //    }

        //    // Draw rows
        //    startY += dataGridView1.Rows[0].Height;
        //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
        //    {
        //        startX = (e.PageBounds.Width - dataGridView1.Width) / 2;
        //        for (int j = 0; j < dataGridView1.Columns.Count; j++)
        //        {
        //            e.Graphics.FillRectangle(Brushes.White, startX, startY, dataGridView1.Columns[j].Width, dataGridView1.Rows[i].Height);
        //            e.Graphics.DrawRectangle(Pens.Black, startX, startY, dataGridView1.Columns[j].Width, dataGridView1.Rows[i].Height);
        //            e.Graphics.DrawString(Convert.ToString(dataGridView1.Rows[i].Cells[j].Value), dataGridView1.Font, Brushes.Black, startX, startY);
        //            startX += dataGridView1.Columns[j].Width;
        //        }
        //        startY += dataGridView1.Rows[i].Height;
        //    }

        //}
        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Tăng độ phân giải DPI
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Create separate bitmaps for dataGridView1 and panel1
            Bitmap bmpDataGridView = new Bitmap(dataGridView1.Width, dataGridView1.Height);
            Bitmap bmpPanel = new Bitmap(panel1.Width, panel1.Height);

            // Draw dataGridView1 onto bmpDataGridView
            dataGridView1.DrawToBitmap(bmpDataGridView, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));

            // Draw panel1 onto bmpPanel
            panel1.DrawToBitmap(bmpPanel, new Rectangle(0, 0, panel1.Width, panel1.Height));

            // Tính toạ độ y để dời hình ảnh xuống 1/4 trang
            float y = e.MarginBounds.Height * 1 / 4;

            // Tính chiều rộng mới để vừa với chiều ngang của trang
            float newWidthDataGridView = e.PageBounds.Width * 0.8f; // Reduced scaling factor for dataGridView1
            float newWidthPanel = bmpPanel.Width; // Keep original width for panel1

            // Maintain aspect ratio
            float ratioDataGridView = (float)bmpDataGridView.Width / bmpDataGridView.Height;
            float ratioPanel = (float)bmpPanel.Width / bmpPanel.Height;

            // Tính chiều cao mới để giữ nguyên tỷ lệ khung hình
            float newHeightDataGridView = newWidthDataGridView / ratioDataGridView;
            float newHeightPanel = newWidthPanel / ratioPanel;

            // Tạo một bitmap mới với kích thước mới
            Bitmap resizedBmpDataGridView = new Bitmap(bmpDataGridView, new Size((int)newWidthDataGridView, (int)newHeightDataGridView));
            Bitmap resizedBmpPanel = new Bitmap(bmpPanel, new Size((int)newWidthPanel, (int)newHeightPanel));

            // Vẽ bitmap đã thay đổi kích thước lên trang tại vị trí mới
            e.Graphics.DrawImage(resizedBmpPanel, 0, y - 50 ); // Draw panel1 first
            e.Graphics.DrawImage(resizedBmpDataGridView, 0 + 100, y + 30 + resizedBmpPanel.Height); // Draw dataGridView1 below panel1
        }






    }
}
