using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using QLSV.QLSVDBDataSet1TableAdapters;

namespace QLSV
{
    internal class SCORE
    {
        MY_DB mydb = new MY_DB();

        public bool insertOnlySIDandCID(int student_id, int course_Id)
        {
            SqlCommand command = new SqlCommand("insert into score (student_id, course_Id)" + " values (@sid,@cid)", mydb.getConnection);
            command.Parameters.Add("@sid", SqlDbType.Int).Value = student_id;
            command.Parameters.Add("@cid", SqlDbType.Int).Value = course_Id;

            mydb.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                mydb.closeConnection();
                return true;
            }
            else
            {
                mydb.closeConnection();
                return false;
            }
        }
        public bool insertScore(int student_id, int course_Id, double student_score, string description)
        {
            SqlCommand command = new SqlCommand("insert into score (student_id, course_Id, student_score, description)" + "values (@sid,@cid,@scr,@dscr)", mydb.getConnection);
            command.Parameters.Add("@sid", SqlDbType.Int).Value = student_id;
            command.Parameters.Add("@cid", SqlDbType.Int).Value = course_Id;
            command.Parameters.Add("@scr", SqlDbType.Float).Value = student_score;
            command.Parameters.Add("@dscr", SqlDbType.VarChar).Value = description;

            mydb.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                mydb.closeConnection();
                return true;
            }
            else
            {
                mydb.closeConnection();
                return false;
            }
        }
        public bool ScoreExists(int student_id, int course_Id)
        {
            SqlCommand command = new SqlCommand("select * from score where student_id=@sid and course_Id=@cid", mydb.getConnection);
            command.Parameters.Add("@sid", SqlDbType.Int).Value = student_id;
            command.Parameters.Add("@cid", SqlDbType.Int).Value = course_Id;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count == 0)
            {

                return false;
            }
            else
            {
                return true;
            }
        }
        public bool CheckScoreExists(int student_id, string coursename)
        {
            SqlCommand command = new SqlCommand("select * " +
                "from score inner join course on score.course_Id = CAST(course.Id AS INT) " +
                "where score.student_id=@sid and course.label=@cid", mydb.getConnection);
            command.Parameters.Add("@sid", SqlDbType.Int).Value = student_id;
            command.Parameters.Add("@cid", SqlDbType.VarChar).Value = coursename;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count == 0)
            {

                return false;
            }
            else
            {
                return true;
            }
        }
        public DataTable getScore()
        {
            // Sử dụng CAST để chuyển đổi kiểu dữ liệu của các cột id
            SqlCommand command = new SqlCommand("SELECT score.student_id,  std.fname, std.lname, score.course_Id, course.label, score.student_score, score.description " +
                                     "FROM std INNER JOIN score ON std.id = score.student_id INNER JOIN course ON score.course_Id = CAST(course.Id AS INT)", mydb.getConnection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public bool deleteScore(int student_id, int course_Id)
        {
            SqlCommand command = new SqlCommand("delete from score where student_id = @sid and course_Id = @cid", mydb.getConnection);
            command.Parameters.Add("@sid", SqlDbType.Int).Value = student_id;
            command.Parameters.Add("@cid", SqlDbType.NVarChar).Value = course_Id; // Chỉnh sửa kiểu dữ liệu của course_Id nếu cần
            mydb.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                mydb.closeConnection();
                return true;
            }
            else
            {
                mydb.closeConnection();
                return false;
            }
        }
        public DataTable avgScoreByCourse()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection;
            command.CommandText = ("select course.label, avg(score.student_score) as AverageScore from course, score where score.course_Id = CAST(course.Id AS INT) group by course.label");

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public DataTable getCourseScores(int course_Id)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection;
            command.CommandText = ("select score.student_id, std.fname, std.lname, score.course_Id, course.label, score.student_score from std inner join score on std.id=score.student_id inner join course on score.course_Id = CAST(course.Id AS INT) where score.course_Id =" + course_Id);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public DataTable getScoreBySIDandCourseID(int student_id, string label)
        {
            SqlCommand command = new SqlCommand("select score.student_id, std.fname, std.lname, score.course_Id, course.label, score.student_score, score.description" +
                " from std inner join score on std.id=score.student_id inner join course on score.course_Id = CAST(course.Id AS INT) " +
                "where score.student_id = @sid and course.label = @name", mydb.getConnection);
            command.Parameters.Add("@sid", SqlDbType.Int).Value = student_id;
            command.Parameters.Add("@name", SqlDbType.VarChar).Value = label;


            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

    
    }
}