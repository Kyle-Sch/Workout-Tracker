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
    public class UserSqlDAL : IUserDAL
    {
        private readonly string connectionString;

        public UserSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        
        public void CreateUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO users " +
                        "(username, password, salt, role, isActive)" +
                        "VALUES (@username, @password, @salt, @role, 'true');", conn);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@salt", user.Salt);
                    cmd.Parameters.AddWithValue("@role", user.Role);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    
                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public User GetUser(LoginViewModel loginInfo)
        {
            User output = null;
            string userName = loginInfo.Email;
            HashProvider hashProvider = new HashProvider();

            string SQL_LoginString = @"Select TOP 1 * from users WHERE username = @username AND isActive = 'true'";
            Dictionary<string, object> dynamicParameterArgs = new Dictionary<string, object>();
            dynamicParameterArgs.Add("@username", userName);
            //dynamicParameterArgs.Add("@password", hashedPassword.Password);
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();
                output = myConnection.Query<User>(SQL_LoginString, new DynamicParameters(dynamicParameterArgs)).ToList().FirstOrDefault();
                //SqlCommand myCommand = new SqlCommand(SQL_LoginString, myConnection);

            }
            if (output != null && hashProvider.VerifyPasswordMatch(output.Password, loginInfo.Password, output.Salt))
            {
                return output;
            }
            else
            {
                return null;
            }

        }
        
        public void DeleteUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE users SET IsActive = 'false' WHERE id = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", user.Id);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
        public User GetUser(string username)
        {
            User user = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string CmdText = "SELECT * FROM USERS WHERE username = @username;";
                    // SqlCommand cmd = new SqlCommand(CmdText, conn);
                    //cmd.Parameters.AddWithValue("@username", username);
                    Dictionary<string, object> dynamicParamterArgs = new Dictionary<string, object>();
                    dynamicParamterArgs.Add("@username", username);

                    user = conn.Query<User>(CmdText, new DynamicParameters(dynamicParamterArgs)).ToList().FirstOrDefault();
                   // SqlDataReader reader = cmd.ExecuteReader();

                    //if (reader.Read())
                    //{
                    //    user = MapRowToUser(reader);
                    //}

                }

                return user;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public User GetUserWithDapper(string userName)
        {
            User output = null;
            HashProvider hashProvider = new HashProvider();

            string SQL_String = @"Select TOP 1 * from users WHERE username = @username";
            Dictionary<string, object> dynamicParameterArgs = new Dictionary<string, object>();
            dynamicParameterArgs.Add("@username", userName);
            //dynamicParameterArgs.Add("@password", hashedPassword.Password);
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();
                output = myConnection.Query<User>(SQL_String, new DynamicParameters(dynamicParameterArgs)).ToList().FirstOrDefault();

            }
            return output;
        }
        public List<User> GetAllUser()
        {
            List<User> output = null;
            HashProvider hashProvider = new HashProvider();

            string SQL_String = @"Select * from users";
            Dictionary<string, object> dynamicParameterArgs = new Dictionary<string, object>();
            //dynamicParameterArgs.Add("@password", hashedPassword.Password);
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();
                output = myConnection.Query<User>(SQL_String, new DynamicParameters(dynamicParameterArgs)).ToList();

            }
            return output;
        }
        
        public void UpdateUser(User user)
        {
            user.name = user.name == null ? "" : user.name;
            user.GoalType = user.GoalType == null ? "" : user.GoalType;
            user.workoutProfile = user.workoutProfile == null ? "" : user.workoutProfile;
            user.email = user.email == null ? "" : user.email;
            user.Photo = user.Photo == null ? "default-user.jpg" : user.Photo;
            user.FirstName = user.FirstName == null ? "" : user.FirstName;
            user.LastName = user.LastName == null ? "" : user.LastName;
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE users SET password = @password, salt = @salt, " +
                        "role = @role, email = @email, name = @name, photo = @photo, username = @username" +
                        ", GoalType = @workoutgoals, goalreps = @goalreps, " +
                        " isActive = 1, firstname = @firstname, lastname = @lastname WHERE id = @id;", conn);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@salt", user.Salt);
                    cmd.Parameters.AddWithValue("@role", user.Role);
                    cmd.Parameters.AddWithValue("@email", user.email);
                    cmd.Parameters.AddWithValue("@name", user.name);
                    cmd.Parameters.AddWithValue("@photo", user.Photo);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@workoutgoals", user.GoalType);
                    cmd.Parameters.AddWithValue("@goalreps", user.GoalReps);
                    cmd.Parameters.AddWithValue("@workoutProfile", user.workoutProfile);
                    cmd.Parameters.AddWithValue("@id", user.Id);
                    cmd.Parameters.AddWithValue("@firstname", user.FirstName);
                    cmd.Parameters.AddWithValue("@lastname", user.LastName);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public List<User> GetUsersNotCheckedIn()
        {
            List<User> users = new List<User>();
            string SQL_SELECT_STRING = $"SELECT * FROM users WHERE id NOT IN (SELECT memberId FROM visit WHERE departure IS NULL);";

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                users = conn.Query<User>(SQL_SELECT_STRING).ToList();
            }

            return users;
        }
        public List<User> GetUsersCheckedIn()
        {
            List<User> users = new List<User>();
            string SQL_SELECT_STRING = $"SELECT * FROM users WHERE id IN (SELECT memberId FROM visit WHERE departure IS NULL);";
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                users = conn.Query<User>(SQL_SELECT_STRING).ToList();
            }
            return users;
        }

    }
}
