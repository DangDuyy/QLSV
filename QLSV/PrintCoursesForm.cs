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
            // Lấy kích thước của DataGridView
            int width = dataGridView1.Width;
            int height = dataGridView1.Height;

            // Tạo một bitmap với kích thước của DataGridView
            Bitmap bm = new Bitmap(width, height);
            dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, width, height));

            // In ảnh của DataGridView
            e.Graphics.DrawImage(bm, 0, 0);
        }
    }
}
