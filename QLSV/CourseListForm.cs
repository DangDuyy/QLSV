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
    public partial class CourseListForm : Form
    {
        COURSE course = new COURSE();
        MY_DB mydb = new MY_DB();
        public CourseListForm(string label)
        {
            InitializeComponent();
            txt_Course.Text = label;

            dataGridView1.DataSource = course.getCourseByLabel(label);
            printDoc.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);


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

        private void CourseListForm_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select std.id, std.fname, std.lname, std.bdate " +
                        "from std INNER JOIN score on score.student_id = std.id inner join course on score.course_id = CAST(course.id AS INT) " +
                        "where course.label=@cid", mydb.getConnection);
            SqlCommand sem = new SqlCommand("select semester from course where label=@cid", mydb.getConnection);
            cmd.Parameters.Add("@cid", SqlDbType.VarChar).Value = txt_Course.Text;
            sem.Parameters.Add("@cid", SqlDbType.VarChar).Value = txt_Course.Text;
            mydb.openConnection();
            SqlDataReader reader = sem.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                int semester = reader.GetInt32(0);
                txt_Semester.Text = semester.ToString();
            }
            else
            {
                // Handle the case where the query returned no results
            }

            reader.Close();
            mydb.closeConnection();
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDoc;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from course where label=@cid", mydb.getConnection);
            cmd.Parameters.Add("@cid", SqlDbType.VarChar).Value = txt_Course.Text;
            mydb.openConnection();
            SqlDataReader reader = cmd.ExecuteReader();

            string courseid = "";
            int period = 0;
            string descriptionn = "";

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
            mydb.closeConnection();
            // Set the font and color
            Font font = new Font("Arial", 12, FontStyle.Bold);
            SolidBrush brush = new SolidBrush(Color.Black);

            // Set the text to print
            string university = "TRƯỜNG ĐẠI HỌC SƯ PHẠM KỸ THUẬT TP HCM";
            string courseList = "DANH SÁCH SINH VIÊN ĐĂNG KÍ MÔN HỌC";

            // Calculate the center of the page
            float pageCenter = e.PageBounds.Width / 2;

            // Calculate the starting point of the strings
            float universityStart = pageCenter - (e.Graphics.MeasureString(university, font).Width / 2);
            float courseListStart = pageCenter - (e.Graphics.MeasureString(courseList, font).Width / 2);

            // Load the logo
            string filePath = @"C:\fit23-hk2\WINDOW\bt\QLSV\QLSV\QLSV\Resources\FIT2.jpg";
            Image logo = Image.FromFile(filePath);

            // Draw the logo at the top left
            e.Graphics.DrawImage(logo, 10, 10, logo.Width, logo.Height);

            // Set the signature text
            string signature = "Signature: ____________________";

            // Calculate the starting point of the signature
            float signatureStartX = e.PageBounds.Width - e.Graphics.MeasureString(signature, font).Width;
            float signatureStartY = e.PageBounds.Height - font.Height;

            // Draw the signature at the bottom right
            e.Graphics.DrawString(signature, font, brush, signatureStartX, signatureStartY);
        

            // Draw the text onto the Graphics object
            e.Graphics.DrawString(university, font, brush, universityStart, 20);
            e.Graphics.DrawString(courseList, font, brush, courseListStart, 120);

            // Set the additional text to print
            string courseCode = "Mã Học Phần: " + courseid;
            string courseName = "Tên Môn Học: " + txt_Course.Text;
            string semester = "Học kỳ: " + txt_Semester.Text;
            string periods = "Số Tiết: " + period.ToString();
            string description = "Mô Tả: " + descriptionn;

            // Set the font for the additional text
            Font additionalTextFont = new Font("Arial", 8, FontStyle.Regular);

            // Calculate the starting point of the additional text
            float additionalTextStartX = pageCenter - (e.Graphics.MeasureString(courseCode + " " + semester + " " + periods + " " + courseName + " " + description, additionalTextFont).Width / 4) - 80;
            float additionalTextStartY = 160;

            // Draw the additional text onto the Graphics object
            e.Graphics.DrawString(courseCode, additionalTextFont, brush, additionalTextStartX, additionalTextStartY);
            e.Graphics.DrawString(courseName, additionalTextFont, brush, additionalTextStartX, additionalTextStartY + 20);
            e.Graphics.DrawString(semester, additionalTextFont, brush, additionalTextStartX, additionalTextStartY + 40);

            additionalTextStartX = pageCenter + (e.Graphics.MeasureString(courseCode + " " + semester + " " + periods + " " + courseName + " " + description, additionalTextFont).Width / 4) - 80;

            e.Graphics.DrawString(periods, additionalTextFont, brush, additionalTextStartX, additionalTextStartY);
            e.Graphics.DrawString(description, additionalTextFont, brush, additionalTextStartX, additionalTextStartY + 20);


            // Starting point for the DataGridView
            int startX = (e.PageBounds.Width - dataGridView1.Width) / 2;
            int startY = 220;

            // Draw column headers
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                e.Graphics.FillRectangle(Brushes.LightGray, startX, startY, dataGridView1.Columns[i].Width, dataGridView1.Rows[0].Height);
                e.Graphics.DrawRectangle(Pens.Black, startX, startY, dataGridView1.Columns[i].Width, dataGridView1.Rows[0].Height);
                e.Graphics.DrawString(dataGridView1.Columns[i].HeaderText, dataGridView1.Font, Brushes.Black, startX, startY);
                startX += dataGridView1.Columns[i].Width;
            }

            // Draw rows
            startY += dataGridView1.Rows[0].Height;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                startX = (e.PageBounds.Width - dataGridView1.Width) / 2;
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    e.Graphics.FillRectangle(Brushes.White, startX, startY, dataGridView1.Columns[j].Width, dataGridView1.Rows[i].Height);
                    e.Graphics.DrawRectangle(Pens.Black, startX, startY, dataGridView1.Columns[j].Width, dataGridView1.Rows[i].Height);
                    e.Graphics.DrawString(Convert.ToString(dataGridView1.Rows[i].Cells[j].Value), dataGridView1.Font, Brushes.Black, startX, startY);
                    startX += dataGridView1.Columns[j].Width;
                }
                startY += dataGridView1.Rows[i].Height;
            }

        }


    }
}
