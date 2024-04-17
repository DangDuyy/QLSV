using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class ManageCoursesForm : Form
    {
        public ManageCoursesForm()
        {
            InitializeComponent();
            ListBoxCourses.DoubleClick += ListBoxCourses_DoubleClick;

        }

        COURSE course = new COURSE();

        int pos;

        private void ManageCoursesForm_Load(object sender, EventArgs e)
        {
            reloadListBoxData();
        }

        void reloadListBoxData() { 
            ListBoxCourses.DataSource = course.getAllCourses();
            ListBoxCourses.ValueMember = "id";
            ListBoxCourses.DisplayMember = "label";

            ListBoxCourses.SelectedItem = null;

            LabelTotalCourses.Text = ("Total courses: " + course.totalCourse());
        }


        void showData(int index)
        {
            DataRow dr = course.getAllCourses().Rows[index];    
            ListBoxCourses.SelectedIndex = index;
            txt_ID.Text = dr.ItemArray[0].ToString();  
            txt_Label.Text = dr.ItemArray[1].ToString();
            numericUpDown1.Value = int.Parse(dr.ItemArray[2].ToString());
            txt_Des.Text = dr.ItemArray[3].ToString();
            comboBox1.DataSource = course.getSemester();
            comboBox1.DisplayMember = "semester"; // Replace with the column name for the semester name
            comboBox1.ValueMember = "semester"; // Replace with the column name for the semester id

            comboBox1.SelectedValue = dr.ItemArray[4].ToString();
        }

        private void ListBoxCourses_Click(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)ListBoxCourses.SelectedItem;
            pos = ListBoxCourses.SelectedIndex;
            showData(pos);

        }

        private void btn_First_Click(object sender, EventArgs e)
        {
            pos = 0;
            showData(pos);
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (pos < (course.getAllCourses().Rows.Count - 1))
            {
                pos++;
                showData(pos);
            }
        }

        private void btn_Previous_Click(object sender, EventArgs e)
        {
            if (pos > 0)
            {
                pos--;
                showData(pos);
            }
        }

        private void btn_Last_Click(object sender, EventArgs e)
        {
            pos = course.getAllCourses().Rows.Count - 1;
            showData(pos);
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {

            COURSE course = new COURSE();
            string id = txt_ID.Text;
            string label = txt_Label.Text;
            int hrs = (int)numericUpDown1.Value;
            string des = txt_Des.Text;
            int sem = Convert.ToInt32(comboBox1.SelectedValue);
            if (label.Trim() == "")
                MessageBox.Show("Add a Course Name", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else if (course.checkCourseName(label, id))
            {
                if (course.insertCourse(id, label, hrs, des, sem))
                {
                    MessageBox.Show("New Course Added", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reloadListBoxData();
                }
                else
                {
                    MessageBox.Show("Error", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("This Course Name Already Exists", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            COURSE course = new COURSE();
            string id = txt_ID.Text;
            string label = txt_Label.Text;
            int hrs = (int)numericUpDown1.Value;
            string des = txt_Des.Text;
            int sem = Convert.ToInt32(comboBox1.SelectedValue);

            if (!course.checkCourseName(label, id))
                MessageBox.Show("This Course Name Already Exists", "Edit Course", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (course.updateCourse(id, label,hrs, des, sem))
            {
                MessageBox.Show("This Course Updated", "Edit Course", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reloadListBoxData();
            }
            else
                MessageBox.Show("This Course Not Updated", "Edit Course", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            pos = 0;
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            try
            {
                string id = txt_ID.Text;
                if (MessageBox.Show("Are u sure when u want delete this course", "Remove Course", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (course.deleteCourse(id))
                    {
                        MessageBox.Show("This Course Deleted", "Remove Course", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //clear
                        txt_Des.Text = "";
                        txt_ID.Text = "";
                        txt_Label.Text = "";
                        numericUpDown1.Value = 10;
                        comboBox1.ValueMember = null;
                        comboBox1.DisplayMember = null;
                        reloadListBoxData();
                    }
                    else
                        MessageBox.Show("This Course Not Deleted", "Remove Course", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch 
            {
                MessageBox.Show("Enter A Valid Numberic ID", "Remove Course", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            pos = 0;
        }

        private void ListBoxCourses_DoubleClick(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)ListBoxCourses.SelectedItem;
            string selectedCourseLabel = drv["label"].ToString();
            CourseListForm courseListForm = new CourseListForm(selectedCourseLabel);
            courseListForm.Show(this);
        }



    }
}
