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

namespace QLSV
{
    public partial class SelectContact : Form
    {
        public SelectContact()
        {
            InitializeComponent();
        }
        MY_DB mydb = new MY_DB();
        public static string id;
        public DataTable getData(SqlCommand cmd)
        {
            cmd.Connection = mydb.getConnection;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
        private void SelectContact_Load(object sender, EventArgs e)
        {
            int id = Login_Form.userID;
            SqlCommand cmd = new SqlCommand("select id as MãGV, fname as Họ,lname as Tên,userid as MãHR from [contact] where userid = @userid");
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = id;
            dataGridView1.DataSource = getData(cmd);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            this.Close();
        }
    }
}
