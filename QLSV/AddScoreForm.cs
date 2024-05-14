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

namespace QLSV
{
    public partial class AddScoreForm : Form
    {
        public AddScoreForm()
        {
            InitializeComponent();
        }
        SCORE score = new SCORE();
        COURSE course = new COURSE();
        STUDENT student = new STUDENT();
        private void AddScoreForm_Load(object sender, EventArgs e)
        {
            ComboBoxScore.DataSource = course.getAllCourses();
            ComboBoxScore.DisplayMember = "label";
            ComboBoxScore.ValueMember = "Id";

            SqlCommand command = new SqlCommand("select id as MaSV, fname as HoSV, lname as TenSV, bdate as DOB from std ");
            dataGridView1.DataSource = student.getStudents(command);

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            txt_ID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int studentid = Convert.ToInt32(txt_ID.Text);
                int courseId = Convert.ToInt32(ComboBoxScore.SelectedValue);
                double scoreValue = Convert.ToDouble(txt_Score.Text);
                string description = txt_Des.Text;

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

        private void ComboBoxScore_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txt_Des_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void txt_Score_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
        private void txt_ID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_ID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                int studentid = Convert.ToInt32(txt_ID.Text);
                int courseId = Convert.ToInt32(ComboBoxScore.SelectedValue);
                double scoreValue = Convert.ToDouble(txt_Score.Text);
                string description = txt_Des.Text;

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