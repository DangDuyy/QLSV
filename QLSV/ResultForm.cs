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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSV
{
    public partial class ResultForm : Form
    {
        SCORE score = new SCORE();
        MY_DB mydb = new MY_DB();
        STUDENT student = new STUDENT();
        public ResultForm()
        {
            InitializeComponent();
            printDoc.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = score.avgScoreByStudent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_ID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txt_Fname.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txt_Lname.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            
        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            DataTable allStudents = score.avgScoreByStudent();
            string search = txt_Search.Text;
            DataRow[] foundRows = allStudents.Select("Convert(MSSV, 'System.String') LIKE '%" + search + "%' OR Họ LIKE '%" + search + "%' OR Tên LIKE '%" + search + "%'");
            if (foundRows.Any())
            {
                DataTable table = foundRows.CopyToDataTable();
                dataGridView1.DataSource = table;
                txt_ID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txt_Fname.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txt_Lname.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            }
            else
            {
                txt_ID.Text = "";
                txt_Fname.Text = "";
                txt_Lname.Text = "";
                dataGridView1.DataSource = null;
            }
        }
        private PrintDocument printDoc = new PrintDocument();

        private void btn_Print_Click(object sender, EventArgs e)
        {
            string studentId = dataGridView1.CurrentRow.Cells[0].Value.ToString();


            SqlCommand command = new SqlCommand("select * from std where id = @id ", mydb.getConnection);
            command.Parameters.AddWithValue("@id", studentId);

            dataGridView2.DataSource = student.getStudents(command);

            PrintOneStudent printOneStudent = new PrintOneStudent();
            printOneStudent.title.Text = printOneStudent.title.Text + " " + dataGridView1.CurrentRow.Cells[1].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[2].Value.ToString();
            printOneStudent.label_ID.Text = printOneStudent.label_ID.Text + " " + dataGridView1.CurrentRow.Cells[0].Value.ToString();
            printOneStudent.lb_Bdate.Text += " " + Convert.ToDateTime(dataGridView2.CurrentRow.Cells[3].Value).ToShortDateString();
            printOneStudent.lb_gender.Text += " " + dataGridView2.CurrentRow.Cells[4].Value.ToString();
            printOneStudent.lb_phone.Text += " " + dataGridView2.CurrentRow.Cells[5].Value.ToString();
            printOneStudent.lb_address.Text += " " + dataGridView2.CurrentRow.Cells[6].Value.ToString();
            printOneStudent.label1.Text += " " + dataGridView2.CurrentRow.Cells[8].Value.ToString();
            printOneStudent.lb_average.Text += " " + dataGridView1.CurrentRow.Cells[dataGridView1.ColumnCount - 2].Value.ToString();
            printOneStudent.lb_rank.Text += " " + dataGridView1.CurrentRow.Cells[dataGridView1.ColumnCount - 1].Value.ToString();

            // Create a new DataTable
            DataTable dt = new DataTable();

            // Add columns to the DataTable
            for (int i = 3; i < dataGridView1.ColumnCount - 2; i++)
            {
                dt.Columns.Add(dataGridView1.Columns[i].Name);
            }

            // Add the data from the selected row to the DataTable
            DataRow row = dt.NewRow();
            for (int i = 3; i < dataGridView1.ColumnCount - 2; i++)
            {
                row[i - 3] = dataGridView1.CurrentRow.Cells[i].Value;
            }
            dt.Rows.Add(row);

            // Set the DataSource of printOneStudent.dataGridView1
            printOneStudent.dataGridView1.DataSource = dt;

            printOneStudent.Show();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Set the font and color
            Font font = new Font("Arial", 12, FontStyle.Bold);

            SolidBrush brush = new SolidBrush(Color.Black);

            // Set the text to print
            string university = "TRƯỜNG ĐẠI HỌC SƯ PHẠM KỸ THUẬT THÀNH PHỐ HỒ CHÍ MINH";
            string courseList = "BẢNG ĐIỂM SINH VIÊN";

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

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                // Set the font and color
                Font childfont = new Font("Arial", 10);
                SolidBrush childbrush = new SolidBrush(Color.Black);

                // Set the text to print
                string studentInfo = "MSSV: " + selectedRow.Cells[0].Value.ToString() +
                                     "\nHọ SV: " + selectedRow.Cells[1].Value.ToString() +
                                     "\nTên SV: " + selectedRow.Cells[2].Value.ToString() +
                                     "\nĐiểm trung bình: " + selectedRow.Cells["AverageScore"].Value.ToString() +
                                     "\nXếp loại: " + selectedRow.Cells["Result"].Value.ToString();

                // Draw the text onto the Graphics object
                e.Graphics.DrawString(studentInfo, childfont, childbrush, pageCenter - (e.Graphics.MeasureString(studentInfo, childfont).Width / 2), 160); // Adjust the Y coordinate to position it below the courseList

                // New code: print course names and scores in two rows
                float currentY = 300; // Adjust this value to position the rows below the student info
                float cellHeight = 20; // Adjust this value to change the height of the cells
                float cellWidth = e.PageBounds.Width / (selectedRow.Cells.Count - 5); // Adjust this value to change the width of the cells
                for (int i = 3; i < selectedRow.Cells.Count - 2; i++) // Start from 3 to skip the student ID, first name, and last name
                {
                    string courseName = dataGridView1.Columns[i].HeaderText; // Changed from .Name to .HeaderText
                    string courseScore = selectedRow.Cells[i].Value.ToString();


                    // Calculate the X coordinate for the current course
                    float currentX = pageCenter - cellWidth * (selectedRow.Cells.Count - 5) / 2 + cellWidth * (i - 3);

                    // Draw the course name in the first row
                    e.Graphics.DrawString(courseName, childfont, Brushes.Black, currentX, currentY); // Adjust the X and Y coordinates to position the course name inside the cell
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)Math.Round(currentX), (int)Math.Round(currentY), (int)Math.Round(cellWidth), (int)Math.Round(cellHeight))); // Draw a border around the course name

                    // Draw the course score in the second row
                    e.Graphics.DrawString(courseScore, childfont, childbrush, currentX, currentY + cellHeight); // Adjust the X and Y coordinates to position the course score inside the cell
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)Math.Round(currentX), (int)Math.Round(currentY + cellHeight), (int)Math.Round(cellWidth), (int)Math.Round(cellHeight))); // Draw a border around the course score

                }
            }
        }
    }
}
