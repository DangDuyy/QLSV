using OfficeOpenXml;
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
using System.Drawing.Printing;

namespace QLSV
{
    public partial class PrintCoursesForm : Form
    {
        public PrintCoursesForm()
        {
            InitializeComponent();
            printDoc.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
        }
        COURSE course = new COURSE();
        private void PrintCoursesForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLSVDBDataSetCourse.Course' table. You can move, or remove it, as needed.
            this.courseTableAdapter.Fill(this.qLSVDBDataSetCourse.Course);
            SqlCommand command = new SqlCommand("SELECT * FROM course");
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = course.getCourses(command);
            dataGridView1.AllowUserToAddRows = false;

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            MY_DB dB = new MY_DB();
            string query = "SELECT * FROM course";

            SqlCommand command = new SqlCommand(query, dB.getConnection);
            dB.openConnection();

            // Đọc dữ liệu từ cơ sở dữ liệu vào một DataTable
            DataTable dataTable = new DataTable();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            // Tạo một file Excel và ghi dữ liệu vào đó bằng EPPlus
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells.LoadFromDataTable(dataTable, true);

                // Lưu file Excel
                string filePath = @"C:\Users\Admin\Desktop\output.xlsx";
                excelPackage.SaveAs(new System.IO.FileInfo(filePath));
            }

            MessageBox.Show("File Excel đã được tạo thành công.");
        }


        // Khai báo đối tượng PrintDocument
        private PrintDocument printDoc = new PrintDocument();


        // Sự kiện khi nhấn nút In
        private void btn_Print_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDoc;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        // Xử lý sự kiện PrintPage
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {

            // Set the font and color
            Font font = new Font("Arial", 12, FontStyle.Bold);
            Font titlefont = new Font("Arial", 18, FontStyle.Bold);

            SolidBrush brush = new SolidBrush(Color.Black);

            // Set the text to print
            string university = "TRƯỜNG ĐẠI HỌC SƯ PHẠM KỸ THUẬT THÀNH PHỐ HỒ CHÍ MINH";
            string courseList = "DANH SÁCH MÔN HỌC";

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
