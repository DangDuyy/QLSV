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
        SCORE score = new SCORE();
        STUDENT student = new STUDENT();
        COURSE course = new COURSE();
        string data = "score";
        private void ManageScoreForm_Load(object sender, EventArgs e)
        {
           
            dataGridView1.DataSource = score.getScore();
            ComboBoxScore.DataSource = course.getAllCourses();
            ComboBoxScore.DisplayMember = "label";
            ComboBoxScore.ValueMember = "label";

            ComboBoxScore.SelectedItem = null;
        }

        private void buttonShowStudent_Click(object sender, EventArgs e)
        {
            data = "student";
            SqlCommand command = new SqlCommand("select id, fname, lname, bdate from std ");
            dataGridView1.DataSource = student.getStudents(command);
        }
         public void fillCombo(int index)
        {
            ComboBoxScore.DataSource = course.getAllCourses();
            ComboBoxScore.DisplayMember = "label";
            ComboBoxScore.ValueMember = "label";

            ComboBoxScore.SelectedIndex = index;
        }
        private void buttonShowScore_Click(object sender, EventArgs e)
        {
            data = "score";
            dataGridView1.DataSource = score.getScore();
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
                
            }


        }

        private void ButtonEdit_Click(object sender, EventArgs e)
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
                    MessageBox.Show("The Score for thí course are already set", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void ComboBoxScore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Check if SelectedValue is not null before calling ToString
                int studentid = Convert.ToInt32(textBoxStudentID.Text);
           
                string label = ComboBoxScore.SelectedValue?.ToString().Trim();
                TextBoxScore.Text = "";
                TextBoxDiscription.Text = "";
                if (label != null)
                {
                    DataTable table = score.getScoreBySIDandCourseID(studentid, label);
                    TextBoxScore.Text = table.Rows[0]["student_score"].ToString();
                    TextBoxDiscription.Text = table.Rows[0]["description"].ToString();
                }
                
            }
            catch
            {
                // Handle exceptions if necessary
            }
           
        }
    }
}