using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Input;
namespace QLSV
{
    public partial class ManageScoreForm : Form
    {
        public ManageScoreForm()
        {
            InitializeComponent();
        }
        public ManageScoreForm(string selectedData)
        {
            InitializeComponent();
            SelectedData = selectedData;
            txt_TeacherID.Text = SelectedData;
        }

        SCORE score = new SCORE();
        STUDENT student = new STUDENT();
        COURSE course = new COURSE();
        string data = "score";
        private void ManageScoreForm_Load(object sender, EventArgs e)
        {
            txt_TeacherID.Text = SelectedData;

            dataGridView1.DataSource = score.getScore();
            ComboBoxScore.DataSource = course.getAllCourses();
            ComboBoxScore.DisplayMember = "label";
            ComboBoxScore.ValueMember = "label";

            ComboBoxScore.SelectedItem = null;
        }
        public string SelectedData { get; set; }


        private void buttonShowStudent_Click(object sender, EventArgs e)
        {
            data = "student";
            SqlCommand command = new SqlCommand("select id as MaSV, fname as HoSV, lname as TenSV, bdate as DOB from std ");
            dataGridView1.DataSource = student.getStudents(command);
        }
         public void fillCombo(int index)
        {
            ComboBoxScore.DataSource = course.getAllCourses();
            ComboBoxScore.DisplayMember = "label";
            if (flag ==0 ) ComboBoxScore.ValueMember = "id";
            else ComboBoxScore.ValueMember = "label";

            ComboBoxScore.SelectedIndex = index;
        }
        private void buttonShowScore_Click(object sender, EventArgs e)
        {
            data = "score";
            if (txt_TeacherID.Text == "")
                dataGridView1.DataSource = score.getScore();
            else
                dataGridView1.DataSource = score.getScoreWithTeacherID(Convert.ToInt32(SelectedData));
        }


        private void dataGridView1_Click(object sender, EventArgs e)
        {
            getDataFromDatagridview();
        }
        public void getDataFromDatagridview()
        {
            if (data == "student")
            {
                textBoxStudentID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            }
            else if (data == "score")
            {
                textBoxStudentID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                fillCombo(ComboBoxScore.SelectedIndex);
                ComboBoxScore.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                TextBoxScore.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString(); 
                TextBoxDiscription.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            }


        }

        private void btnRemoveScore_Click(object sender, EventArgs e)
        {
            int studentid = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            int courseId = int.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());

            if ((MessageBox.Show("Do you want to delete score", "Delete score", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                if (score.deleteScore(studentid, courseId))
                {
                    MessageBox.Show("score deleted", "Remove score", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.DataSource = score.getScore();
                }
                else
                {
                    MessageBox.Show("score not deleted", "Remove score", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void ButtonAvgScore_Click(object sender, EventArgs e)
        {
            avgScoreByCourseForm avg = new avgScoreByCourseForm();
            avg.Show(this);
        }

        private void ManageScoreForm_Load_1(object sender, EventArgs e)
        {

        }
        int flag = 0;

        private void ComboBoxScore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Check if SelectedValue is not null before calling ToString
                int studentid = Convert.ToInt32(textBoxStudentID.Text);
                int courseId = Convert.ToInt32(ComboBoxScore.SelectedValue);
                Console.WriteLine("Course ID: " + courseId);
                string label = ComboBoxScore.SelectedValue?.ToString().Trim();
                TextBoxScore.Text = "";
                TextBoxDiscription.Text = "";
                if (label != null)
                {
                    DataTable table = score.getScoreBySIDandCourse_ID(studentid, courseId);
                    TextBoxScore.Text = table.Rows[0]["student_score"].ToString();
                    TextBoxDiscription.Text = table.Rows[0]["description"].ToString();
                }
            }
            catch
            {
                // Handle exceptions if necessary
            }
           
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                int studentid = Convert.ToInt32(textBoxStudentID.Text);
                int courseId = Convert.ToInt32(ComboBoxScore.SelectedValue);
                double scoreValue = Convert.ToDouble(TextBoxScore.Text);
                string description = TextBoxDiscription.Text;

                if (!score.ScoreExists(studentid, courseId))
                {
                    if (score.insertScore(studentid, courseId, scoreValue, description))
                    {
                        MessageBox.Show("Student Score Insert", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Student Score  not Insert", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("The Score for this course are already set", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}