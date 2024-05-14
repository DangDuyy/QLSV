using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSV
{
    public partial class RemoveCourseForm : Form
    {
        public RemoveCourseForm()
        {
            InitializeComponent();
        }

        COURSE course = new COURSE();
        private void btnRemovecourse_Click(object sender, EventArgs e)
        {
            string courseid = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            if ((MessageBox.Show("Do you want to delete course", "Delete course", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                if (course.deleteCourse(courseid))
                {
                    MessageBox.Show("course deleted", "Remove course", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.DataSource = course.getAllCourses();
                }
                else
                {
                    MessageBox.Show("course not deleted", "Remove course", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void RemoveCourseForm_Load(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT id as MãMH, label as TênMH, period as Tiết, description as MôTả, semester as HọcKi FROM course");
            dataGridView1.DataSource = course.getCourses(sqlCommand);
        }
    }
}
