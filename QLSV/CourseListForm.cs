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
            cmd.Parameters.Add("@cid", SqlDbType.VarChar).Value = txt_Course.Text;
            mydb.openConnection();
            SqlDataReader reader = cmd.ExecuteReader();

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
