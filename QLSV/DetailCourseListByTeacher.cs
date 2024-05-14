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
using System.Windows.Input;

namespace QLSV
{
    public partial class DetailCourseListByTeacher : Form
    {
        public DetailCourseListByTeacher()
        {
            InitializeComponent();
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

        }
        public DetailCourseListByTeacher(string selectedData)
        {
            InitializeComponent();
            SelectedData = selectedData;
            txt_TeacherID.Text = SelectedData;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

        }
        COURSE course = new COURSE();
        MY_DB mydb = new MY_DB();   
        public string SelectedData { get; set; }

        private void DetailCourseListByTeacher_Load(object sender, EventArgs e)
        {
            txt_TeacherID.Text = SelectedData;
            comboBox1.DataSource = course.getSemester();
            comboBox1.DisplayMember = "semester";
            comboBox1.ValueMember = "semester";
            comboBox1.SelectedItem = null;
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int teacher_id = Convert.ToInt32(txt_TeacherID.Text);

                int semester = Convert.ToInt32(comboBox1.SelectedValue);

                dataGridView1.DataSource = course.getCourseBySemesterandTeacherID(teacher_id, semester);
            }
            catch (Exception ex) 
            { }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                if (row.DataBoundItem is DataRowView drv)
                {
                    string selectedCourseLabel = drv[1].ToString();
                    CourseListFormByTeacher courseListForm = new CourseListFormByTeacher(selectedCourseLabel);
                    courseListForm.Show(this);
                }
            }
        }
    }
}
