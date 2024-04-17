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
    public partial class AddCourseFrm : Form
    {
        COURSE course = new COURSE();
        MY_DB mydb = new MY_DB();
        public AddCourseFrm(string txt_ID)
        {
            InitializeComponent();
            this.txt_ID.Text = txt_ID;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

        }

        private void AddCourseFrm_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = course.getSemester();
            comboBox1.DisplayMember = "semester";
            comboBox1.ValueMember = "semester";
            comboBox1.SelectedItem = null;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)

        {
            
            try
            {
                int student_id = Convert.ToInt32(txt_ID.Text);

                int semester = Convert.ToInt32(comboBox1.SelectedValue);

                dataGridView1.DataSource = course.getCourseBySemAndStudent_ID(student_id, semester);
                int id = Convert.ToInt32(comboBox1.SelectedValue);
                if (id != 0)
                {
                    dataGridView2.DataSource = course.getCourseBySemester(id);
                    dataGridView2.Columns["label"].HeaderText = "Tên MH";
                }
            }
            catch
            {
            }
        }
        int count = 0;
        private void btn_Add_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                string courseName = row.Cells[0].Value.ToString();

                if (dt.AsEnumerable().Any(r => r.Field<string>("label") == courseName))
                {
                    MessageBox.Show("Course " + courseName + " already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                DataRow newRow = dt.NewRow();

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    newRow[i] = row.Cells[i].Value;
                }

                dt.Rows.Add(newRow);
            }

            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.Remove(row);
            }
        }



        private void btn_Save_Click(object sender, EventArgs e)
        {
            SCORE score = new SCORE();

            int student_id = Convert.ToInt32(txt_ID.Text);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string course_name = null;

                if (row.Cells[0].Value != null)
                {
                    course_name = row.Cells[0].Value.ToString();
                }
                else
                {
                    continue; 
                }
               
                SqlCommand cmd = new SqlCommand("select id from course where label = @cname", mydb.getConnection);
                cmd.Parameters.Add("@cname", SqlDbType.VarChar).Value = course_name;
                mydb.openConnection();
                SqlDataReader reader = cmd.ExecuteReader();

                int course_id = 0;
                int id;

                if (reader.HasRows)
                {
                    reader.Read();
                    string idString = reader.GetString(0);
                    if (Int32.TryParse(idString, out id))
                    {
                        course_id = id;
                    }
                    else
                    {
                    }
                }
                else
                {
                }

                reader.Close();
                mydb.closeConnection();

                if (!score.CheckScoreExists(student_id, course_name))
                {
                    if (score.insertOnlySIDandCID(student_id, course_id))
                    {
                        MessageBox.Show("Add course " + course_name + "successfull", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Can't add course", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    continue;
                }
            }
        }




    }
}
