using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV
{
    internal class COURSE
    {

        MY_DB myDB = new MY_DB();

        public bool insertCourse(string Id,string label, int period, string description, int sem)
        {
            SqlCommand sqlCommand = new SqlCommand("insert into course (id,label,period,description,semester)" +
                "values (@id,@label,@period,@description,@sem)" , myDB.getConnection);
            sqlCommand.Parameters.Add("@id", SqlDbType.NVarChar).Value = Id;
            sqlCommand.Parameters.Add("@label", SqlDbType.VarChar).Value = label;
            sqlCommand.Parameters.Add("@period", SqlDbType.Int).Value = period;
            sqlCommand.Parameters.Add("@description", SqlDbType.Text).Value = description;
            sqlCommand.Parameters.Add("@sem", SqlDbType.Int).Value = sem;


            myDB.openConnection();
            if ((sqlCommand.ExecuteNonQuery() == 1))
            {
                myDB.closeConnection();
                return true;
            }
            else
            {
                myDB.closeConnection();
                return false;
            }
        }

        public DataTable getCourses(SqlCommand sqlCommand)
        {
            sqlCommand.Connection = myDB.getConnection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = sqlCommand;
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public string totalCourse()
        {
            return execCount("SELECT COUNT(*) FROM course");
        }
        public string execCount(string query)
        {
            SqlCommand command = new SqlCommand(query, myDB.getConnection);
            myDB.openConnection();
            string count = command.ExecuteScalar().ToString();

            myDB.closeConnection();
            return count;
        }


        public DataTable getAllCourses()
        {
            SqlCommand command = new SqlCommand("select * from course", myDB.getConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();

            adapter.Fill(table);
            return table;
        }
        public DataTable getAllCoursesByID(string[] ids)
        {
            DataTable table = new DataTable();
            myDB.openConnection();

            foreach (string id in ids)
            {
                SqlCommand command = new SqlCommand("select * from course where id = @id", myDB.getConnection);
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable tempTable = new DataTable();

                adapter.Fill(tempTable);
                table.Merge(tempTable);
            }

            myDB.closeConnection();
            return table;
        }


        public DataTable getCourseById(string courseID)
        {
            SqlCommand command = new SqlCommand("select * from course where Id=@cid", myDB.getConnection);
            command.Parameters.Add("@cid", SqlDbType.NVarChar).Value = courseID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();

            adapter.Fill(table);
            return table;
        }
       public DataTable getCourseByLabel(string label)
        {
        SqlCommand command = new SqlCommand("select std.id, std.fname, std.lname, std.bdate " +
            "from std INNER JOIN score on score.student_id = std.id inner join course on score.course_id = CAST(course.id AS INT) " +
            "where course.label=@cid", myDB.getConnection);
        command.Parameters.Add("@cid", SqlDbType.NVarChar).Value = label;
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataTable table = new DataTable();

        adapter.Fill(table);
        return table;
        }

        public DataTable getCourseBySemAndStudent_ID(int studentid, int sem)
        {
            SqlCommand command = new SqlCommand("select course.label " +
                "from course inner join score on score.course_id = CAST(course.id AS INT) inner join std on std.id = score.student_id " +
                "where score.student_id = @sid and course.semester = @sem", myDB.getConnection);
            command.Parameters.Add("@sid", SqlDbType.Int).Value = studentid;
            command.Parameters.Add("@sem", SqlDbType.Int).Value = sem;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();

            adapter.Fill(table);
           

            return table;
        }


        //delete student
        public bool deleteCourse(string id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM course WHERE id = @id", myDB.getConnection);
            cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
            myDB.openConnection();
            if ((cmd.ExecuteNonQuery() == 1))
            {
                myDB.closeConnection();
                return true;
            }
            else
            {
                myDB.closeConnection();
                return false;
            }
        }
        //update student
        public bool updateCourse(string id, string label, int period, string description, int sem)
        {
            SqlCommand cmd = new SqlCommand("UPDATE course SET label=@label, period=@period, description=@description semester=@semester WHERE id=@ID", myDB.getConnection);

            cmd.Parameters.Add("@ID", SqlDbType.NVarChar).Value = id;
            cmd.Parameters.Add("@label", SqlDbType.VarChar).Value = label;
            cmd.Parameters.Add("@period", SqlDbType.Int).Value = period;
            cmd.Parameters.Add("@description", SqlDbType.Text).Value = description;
            cmd.Parameters.Add("@semester", SqlDbType.Int).Value = sem; 

            myDB.openConnection();

            if ((cmd.ExecuteNonQuery() == 1))
            {
                myDB.closeConnection();
                return true;
            }
            else
            {
                myDB.closeConnection();
                return false;
            }
        }

        public bool checkCourseName(string courseName, string courseId)
        {
            SqlCommand cmd = new SqlCommand("select * from course where label = @cname and id <> @cid", myDB.getConnection);


            cmd.Parameters.Add("@cname", SqlDbType.VarChar).Value = courseName;
            cmd.Parameters.Add("@cid", SqlDbType.NVarChar).Value = courseId;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable table = new DataTable();

            adapter.Fill(table);

            if ((table.Rows.Count) > 0)
                return false;
            else
                return true;
        }

        public DataTable getSemester()
        {
            SqlCommand command = new SqlCommand("select distinct semester from course", myDB.getConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();

            adapter.Fill(table);
            return table;
        }   

        public DataTable getCourseBySemester(int sem) 
        { 
            SqlCommand command = new SqlCommand("select label from course where semester = @sem", myDB.getConnection);
            command.Parameters.Add("@sem", SqlDbType.Int).Value = sem;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        


    }
}
