using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSV
{
    public partial class EditCourseForm : Form
    {
        public EditCourseForm()
        {
            InitializeComponent();
        }
        COURSE course = new COURSE();

        private void EditCourseForm_Load(object sender, EventArgs e)
        {
            ComboBoxCourse.DataSource = course.getAllCourses();
            ComboBoxCourse.DisplayMember = "Id";
            ComboBoxCourse.ValueMember = "Id";

            ComboBoxCourse.SelectedItem = null;
        }

        public void fillCombo(int index)
        {
            ComboBoxCourse.DataSource = course.getAllCourses();
            ComboBoxCourse.DisplayMember = "Id";
            ComboBoxCourse.ValueMember = "Id";

            ComboBoxCourse.SelectedIndex = index;
        }
        private void btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txt_CourseName.Text;
                int hrs = (int)NumericUpDownHours.Value;
                string descr = txt_Des.Text;
                // Check if SelectedValue is not null before calling ToString
                string id = ComboBoxCourse.SelectedValue?.ToString();
                int sem = Convert.ToInt32(comboBox1.SelectedValue);

                if (!string.IsNullOrWhiteSpace(name) && id != null)
                {
                    if (!course.checkCourseName(name, id))
                    {
                        MessageBox.Show("This course name already exists", "Edit course", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (course.updateCourse(id, name, hrs, descr,sem))
                    {
                        MessageBox.Show("Course updated", "Edit course", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        fillCombo(ComboBoxCourse.SelectedIndex);
                    }
                }
                else
                {
                    MessageBox.Show("Enter the course name and select a course", "Edit course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("No course selected or an unexpected error occurred", "Edit course", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComboBoxCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string id = ComboBoxCourse.SelectedValue?.ToString().Trim();
                if (id != null)
                {
                    DataTable table = course.getCourseById(id);
                    txt_CourseName.Text = table.Rows[0]["label"].ToString();
                    NumericUpDownHours.Value = Convert.ToInt32(table.Rows[0]["period"]);
                    txt_Des.Text = table.Rows[0]["description"].ToString();

                    comboBox1.DataSource = course.getSemester();
                    comboBox1.DisplayMember = "semester"; // Replace with the column name for the semester name
                    comboBox1.ValueMember = "semester"; // Replace with the column name for the semester id

                    comboBox1.SelectedValue = table.Rows[0]["semester"].ToString();
                }
            }
            catch
            {
                // Handle exceptions if necessary
            }
        }


    }
}