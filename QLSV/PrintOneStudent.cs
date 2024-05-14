using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class PrintOneStudent : Form
    {
        public PrintOneStudent()
        {
            InitializeComponent();
        }
        SCORE score = new SCORE();
        MY_DB mydb = new MY_DB();
        STUDENT STUDENT = new STUDENT();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void PrintOneStudent_Load(object sender, EventArgs e)
        {
        }

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
            e.Graphics.DrawImage(resizedBmpPanel, 0, y - 50); // Draw panel1 first
            e.Graphics.DrawImage(resizedBmpDataGridView, 0 + 100, y + 30 + resizedBmpPanel.Height); // Draw dataGridView1 below panel1
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            printPreviewDialog.ShowDialog();

        }
    }
}
