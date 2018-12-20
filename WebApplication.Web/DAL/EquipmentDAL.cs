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
    public class EquipmentDAL : IEquipmentDAL
    {
        private readonly string connectionString;

        public EquipmentDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void CreateEquipment(Equipment machine)
        {
            string SQL_INSERT_STRING = "INSERT INTO equipment " + "(name, needsMaintenance, formMedia, instructions, isActive)" +
                        "VALUES (@name, @needsMaintenance, @formMedia, @instructions, 'true')";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    int rowsAffected = conn.Execute(SQL_INSERT_STRING, machine);

                    //SqlCommand cmd = new SqlCommand("INSERT INTO equipment " +
                    //    "(name, needsMaintenance, formVideo, isActive)" +
                    //    "VALUES (@name, @needsMaintenance, @formVideo, 'true');", conn);



                    //cmd.Parameters.AddWithValue("@needsMaintenance", machine.NeedsMaintenance);
                    //cmd.Parameters.AddWithValue("@formVideo", machine.FormMedia);
                    //cmd.Parameters.AddWithValue("@name", machine.Name);

                    //cmd.ExecuteNonQuery();

                    return;// rowsAffected;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public Equipment GetEquipment(int id)
        {
            Equipment output = null;

            string SQL_LoginString = @"Select TOP 1 * from equipment WHERE equipmentId = @id";
            Dictionary<string, object> dynamicParameterArgs = new Dictionary<string, object>();
            dynamicParameterArgs.Add("@id", id);

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();
                output = myConnection.Query<Equipment>(SQL_LoginString, new DynamicParameters(dynamicParameterArgs)).ToList().FirstOrDefault();
                //SqlCommand myCommand = new SqlCommand(SQL_LoginString, myConnection);

            }
            return output;
        }
        public List<Equipment> GetAllEquipment()
        {
            List<Equipment> posts = new List<Equipment>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select * from equipment", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Equipment temp = new Equipment();
                    temp.EquipmentID = Convert.ToInt32(reader["equipmentId"]);
                    temp.Name = Convert.ToString(reader["name"]);
                    temp.NeedsMaintenance = Convert.ToBoolean(reader["needsMaintenance"]);
                    temp.FormMedia = Convert.ToString(reader["formMedia"]);
                    temp.Instructions = Convert.ToString(reader["instructions"]);
                    temp.IsActive = Convert.ToBoolean(reader["IsActive"]);
                    if (temp.IsActive)
                    {
                        posts.Add(temp);
                    }
                }
                return posts;
            }
        }

        public void DeleteEquipment(Equipment machine)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE equipment SET IsActive = 'false' WHERE equipmentId = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", machine.EquipmentID);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public void UpdateEquipment(Equipment machine)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE equipment SET " +
                        "name = @name, needsMaintenance = @needsMaintenance,  " +
                        "instructions = @instruct WHERE equipmentId = @id;", conn);
                    cmd.Parameters.AddWithValue("@needsMaintenance", machine.NeedsMaintenance);
                    cmd.Parameters.AddWithValue("@instruct", machine.Instructions);
                    cmd.Parameters.AddWithValue("@name", machine.Name);
                    cmd.Parameters.AddWithValue("@id", machine.EquipmentID);

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
