using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Providers.Auth;
using Dapper;
using System.Data.SqlClient;

namespace WebApplication.Web.DAL
{
    public class WorkoutDAL : IWorkoutDAL
    {
        private readonly string connectionString;

        public WorkoutDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void CreateWorkout(Workout workout)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO workout " +
                        "(visitId, name, type, reps, weight, startTime, endTime, equipmentId, isActive, userid)" +
                        "VALUES (@visitId, @name, @type, @reps, @weight, @startTime, @endTime, @equipmentId, 'true', @userid);", conn);
                    cmd.Parameters.AddWithValue("@visitId", workout.VisitID);
                    cmd.Parameters.AddWithValue("@name", workout.Name);
                    cmd.Parameters.AddWithValue("@type", workout.Type);
                    cmd.Parameters.AddWithValue("@reps", workout.Reps);
                    cmd.Parameters.AddWithValue("@startTime", workout.startTime);
                    cmd.Parameters.AddWithValue("@endTime", workout.endTime);
                    cmd.Parameters.AddWithValue("@weight", workout.Weight);
                    cmd.Parameters.AddWithValue("@equipmentId", workout.EquipmentID);
                    cmd.Parameters.AddWithValue("@userid", workout.UserId);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public void DeleteWorkout(Workout workout)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE workout SET IsActive = 'false' WHERE workoutId = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", workout.WorkoutID);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public List<Workout> GetWorkout(string username)
        {
            List<Workout> workout = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string CmdText = "SELECT * FROM USERS WHERE name = @name;";
                    // SqlCommand cmd = new SqlCommand(CmdText, conn);
                    //cmd.Parameters.AddWithValue("@username", username);
                    Dictionary<string, object> dynamicParamterArgs = new Dictionary<string, object>();
                    dynamicParamterArgs.Add("@name", username);

                    workout = conn.Query<Workout>(CmdText, new DynamicParameters(dynamicParamterArgs)).ToList();
                    // SqlDataReader reader = cmd.ExecuteReader();

                    //if (reader.Read())
                    //{
                    //    user = MapRowToUser(reader);
                    //}

                }

                return workout;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public List<Workout> GetWorkoutPerEquipment(int id)
        {
            List<Workout> workout = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string CmdText = "SELECT * FROM workout WHERE equipmentId = @id;";
                    Dictionary<string, object> dynamicParamterArgs = new Dictionary<string, object>();
                    dynamicParamterArgs.Add("@id", id);

                    workout = conn.Query<Workout>(CmdText, new DynamicParameters(dynamicParamterArgs)).ToList();


                }

                return workout;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public List<Workout> GetWorkoutPerVist(int id)
        {
            List<Workout> workout = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string CmdText = "SELECT * FROM workout WHERE visitId = @id;";
                    Dictionary<string, object> dynamicParamterArgs = new Dictionary<string, object>();
                    dynamicParamterArgs.Add("@id", id);

                    workout = conn.Query<Workout>(CmdText, new DynamicParameters(dynamicParamterArgs)).ToList();


                }

                return workout;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public List<Workout> GetWorkoutPerUser(int id)
        {
            List<Workout> workout = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string CmdText = "SELECT * FROM workout WHERE userId = @id;";
                    Dictionary<string, object> dynamicParamterArgs = new Dictionary<string, object>();
                    dynamicParamterArgs.Add("@id", id);

                    workout = conn.Query<Workout>(CmdText, new DynamicParameters(dynamicParamterArgs)).ToList();


                }

                return workout;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void UpdateWorkout(Workout workout)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE workout SET " +
                        "visitId = @visitId, name = @name, type = @type, reps = @reps" +
                        "startTime = @startTime, endTime = @endTime, equipmentId = @equipmentId" +
                        "userId = @userId WHERE id = @id;", conn);
                    cmd.Parameters.AddWithValue("@visitId", workout.VisitID);
                    cmd.Parameters.AddWithValue("@name", workout.Name);
                    cmd.Parameters.AddWithValue("@type", workout.Type);
                    cmd.Parameters.AddWithValue("@reps", workout.Reps);
                    cmd.Parameters.AddWithValue("@startTime", workout.startTime);
                    cmd.Parameters.AddWithValue("@endTime", workout.endTime);
                    cmd.Parameters.AddWithValue("@equipmentId", workout.EquipmentID);
                    cmd.Parameters.AddWithValue("@userId", workout.UserId);
                    cmd.Parameters.AddWithValue("@id", workout.WorkoutID);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Workout GetWorkoutTotals(List<Workout> totals, string type, User user)
        {
            Workout totalWorkouts = new Workout();
            foreach (Workout workouts in totals)
            {
                if (type == workouts.Name)
                {
                    totalWorkouts.TotalReps += workouts.Reps;
                }
            }
            if (totalWorkouts.TotalReps > user.GoalReps)
            {
                do
                {
                    totalWorkouts.TotalReps = totalWorkouts.TotalReps - user.GoalReps;
                } while (totalWorkouts.TotalReps > user.GoalReps);
            }
            totalWorkouts.Type = type;
            return totalWorkouts;
        }
        public Workout GetWorkoutTotals(List<Workout> totals, string name)
        {
            Workout totalWorkouts = new Workout();
            foreach (Workout workouts in totals)
            {
                totalWorkouts.TotalReps += workouts.Reps;
            }
            return totalWorkouts;
        }

    }
}
