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
using System.Windows.Input;

namespace QLSV
{
    public partial class AddCourse : Form
    {
        public AddCourse()
        {
            InitializeComponent();
        }
        
        MY_DB mydb= new MY_DB();
        COURSE course = new COURSE();

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // button insert a new course

        //  chuc nang kiem tra du lieu input

        private void btn_Add_Click(object sender, EventArgs e)
        {
            string id = txt_ID.Text;
            string label = txt_Label.Text;
            int period = Convert.ToInt32(txt_Period.Text);
            string description = txt_Des.Text;
            int sem = Convert.ToInt32(comboBox1.SelectedValue);
            if (period < 10)
            {
                MessageBox.Show("The Period must be > 10", "Invalid Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if  (label.Trim()=="")
                MessageBox.Show("New Course Added", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (course.checkCourseName(label, id))
                {
                    if (course.insertCourse(id, label, period, description,sem))
                    {
                        MessageBox.Show("New Course Added", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void AddCourse_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = course.getSemester();
            comboBox1.DisplayMember = "semester";
            comboBox1.ValueMember = "semester";
            comboBox1.SelectedItem = null;
        }
    }
}
