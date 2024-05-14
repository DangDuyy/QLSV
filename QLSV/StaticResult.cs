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
    public partial class StaticResult : Form
    {
        SCORE score = new SCORE();
        public StaticResult()
        {
            InitializeComponent();
        }

        private void StaticResult_Load(object sender, EventArgs e)
        {
            DataTable dt = score.avgScoreByCourse();
            dataGridView1.DataSource = dt;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label newLabel = new Label();
                newLabel.Name = "label" + i;
                newLabel.Text = dt.Rows[i]["TênMH"].ToString() + " : " + dt.Rows[i]["AverageScore"].ToString();
                newLabel.Font = new Font(newLabel.Font.FontFamily, 12); 
                newLabel.AutoSize = true; 
                if (i == 0)
                {
                    newLabel.Location = new Point(100, 100);
                }
                else
                    newLabel.Location = new Point(100, (i + 2) * 50);
                this.Controls.Add(newLabel);
            }

            DataTable table = score.avgScoreByStudent();
            dataGridView1.DataSource = table;

            int totalStudents = table.Rows.Count;
            int passStudents = 0;

            foreach (DataRow row in table.Rows)
            {
                if (double.Parse(row["AverageScore"].ToString()) >= 5)
                {
                    passStudents++;
                }
            }

            double passRate = (double)passStudents / totalStudents;

            label3.Text = "Pass: " + passRate.ToString("P2"); 
            label4.Text = "Fail: " + (1 - passRate).ToString("P2"); 
        }


    }
}
