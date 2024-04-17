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
    public partial class avgScoreByCourseForm : Form
    {
        SCORE score = new SCORE();
        public avgScoreByCourseForm()
        {
            InitializeComponent();

            dataGridView1.DataSource = score.avgScoreByCourse();
        }


    }
}
