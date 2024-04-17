using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class DetailCourseList : Form
    {
        COURSE course = new COURSE();
        string[] s;
        public DetailCourseList(string label)
        {
            InitializeComponent();
            textBox1.Text = label;
            s = label.Split(',');
            
        }

        private void DetailCourseList_Load(object sender, EventArgs e)
        {
            // You will need to modify getAllCoursesByID to accept an array of strings
            dataGridView1.DataSource = course.getAllCoursesByID(s);
        }
    }
}
