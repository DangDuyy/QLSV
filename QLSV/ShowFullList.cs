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
    public partial class ShowFullList : Form
    {
        public ShowFullList()
        {
            InitializeComponent();
        }
        MY_DB mydb = new MY_DB();
        public DataTable getData(SqlCommand cmd)
        {
            cmd.Connection = mydb.getConnection;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                return;
            }
            int userid = Login_Form.userID;
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [group] where userid = @userid");
            sqlCommand.Parameters.Add("@userid", SqlDbType.Int).Value = userid;

            int groupId = int.Parse(getData(sqlCommand).Rows[listBox1.SelectedIndex]["id"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT contact.id as Teacher_ID, fname as HoSV, lname as TenSV, [group].name as Faculty, contact.phone as SDT, contact.email as Email, contact.address as DiaChi, contact.picture as 'Ảnh đại diện' FROM contact INNER JOIN [group] ON contact.group_id = [group].id WHERE contact.userid = @userid AND contact.group_id = @group_id", mydb.getConnection);

            cmd.Parameters.Add("@group_id", SqlDbType.Int).Value = groupId;
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;

            dataGridView1.DataSource = getData(cmd);
        }


        private void ShowFullList_Load(object sender, EventArgs e)
        {
            int x = Login_Form.userID;
            // Sửa lỗi ở đây: loại bỏ dấu nháy đơn xung quanh tên bảng 'group'
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [group] where userid = @userid");
            sqlCommand.Parameters.Add("@userid", SqlDbType.Int).Value = x;
            listBox1.DataSource = getData(sqlCommand);
            listBox1.ValueMember = "id"; // Giả sử rằng bảng 'group' có cột 'id'
            listBox1.DisplayMember = "name";
            listBox1.SelectedItem = null;

            int userid = Login_Form.userID;

            // Sửa lỗi ở đây: sử dụng dấu ngoặc vuông cho từ khóa 'group'
            SqlCommand cmd = new SqlCommand("SELECT contact.id as Teacher_ID,fname as HoSV, lname as TenSV, [group].name as Faculty, contact.phone as SDT, contact.email as Email, contact.address as DiaChi, contact.picture as 'Ảnh đại diện' FROM contact INNER JOIN [group] ON contact.group_id = [group].id WHERE contact.userid = @userid", mydb.getConnection);

            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;

            dataGridView1.DataSource = getData(cmd);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DetailCourseListByTeacher detailCourseListByTeacher = new DetailCourseListByTeacher(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            detailCourseListByTeacher.Show();
        }


    }
}
