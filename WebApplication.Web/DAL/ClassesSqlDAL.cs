using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Providers.Auth;
using Dapper;

namespace WebApplication.Web.DAL
{
    public class ClassesSqlDAL : IClassesDAL
    {
        private readonly string connectionString;

        public ClassesSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        
        public void CreateClass(WorkoutClass workoutClass)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO workoutClass " +
                        "(Name, InstructorName, AvailableSpots, Description, StartTime, EndTime, isActive)" +
                        "VALUES (@Name, @InstructorName, @AvailableSpots, @Description, @ClassStart, @ClassEnd, 'true');", conn);
                    cmd.Parameters.AddWithValue("@Name", workoutClass.Name);
                    cmd.Parameters.AddWithValue("@InstructorName", workoutClass.InstructorName);
                    cmd.Parameters.AddWithValue("@AvailableSpots", workoutClass.AvailableSpots);
                    cmd.Parameters.AddWithValue("@Description", workoutClass.Description);
                    cmd.Parameters.AddWithValue("@ClassStart", workoutClass.StartTime);
                    cmd.Parameters.AddWithValue("@ClassEnd", workoutClass.EndTime);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
        public void DeleteClass(WorkoutClass workoutClass)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE workoutClass SET IsActive = 'false' WHERE classId = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", workoutClass.classId);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
        public WorkoutClass GetClass(string userName)
        {
            WorkoutClass output = null;

            string SQL_String = @"Select TOP 1 * from workoutClass WHERE name = @name";
            Dictionary<string, object> dynamicParameterArgs = new Dictionary<string, object>();
            dynamicParameterArgs.Add("@name", userName);
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();
                output = myConnection.Query<WorkoutClass>(SQL_String, new DynamicParameters(dynamicParameterArgs)).ToList().FirstOrDefault();

            }
            return output;
        }
        public List<WorkoutClass> GetAllClasses()
        {
            List<WorkoutClass> output = null;

            string SQL_String = @"Select * from workoutClass";
            Dictionary<string, object> dynamicParameterArgs = new Dictionary<string, object>();
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();
                output = myConnection.Query<WorkoutClass>(SQL_String, new DynamicParameters(dynamicParameterArgs)).ToList();

            }
            return output;
        }
        
        public void UpdateClass(WorkoutClass workoutClass)
        {
            workoutClass.InstructorName = workoutClass.InstructorName == null ? "" : workoutClass.InstructorName;
            workoutClass.Description = workoutClass.Description == null ? "" : workoutClass.Description;
            workoutClass.InstructorName = workoutClass.InstructorName == null ? "" : workoutClass.InstructorName;
            workoutClass.InstructorName = workoutClass.InstructorName == null ? "" : workoutClass.InstructorName;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE workoutClass SET Name = @Name, InstructorName = @InstructorName, " +
                        "AvailableSpots = @AvailableSpots, Description = @Description, StartTime = @ClassStart, EndTime = @ClassEnd," +
                        " isActive = 1 WHERE classId = @id;", conn);
                    cmd.Parameters.AddWithValue("@Name", workoutClass.Name);
                    cmd.Parameters.AddWithValue("@InstructorName", workoutClass.InstructorName);
                    cmd.Parameters.AddWithValue("@AvailableSpots", workoutClass.AvailableSpots);
                    cmd.Parameters.AddWithValue("@Description", workoutClass.Description);
                    cmd.Parameters.AddWithValue("@ClassStart", workoutClass.StartTime);
                    cmd.Parameters.AddWithValue("@ClassEnd", workoutClass.EndTime);
                    cmd.Parameters.AddWithValue("@id", workoutClass.classId);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
