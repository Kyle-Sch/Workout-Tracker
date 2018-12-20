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
    public class VisitDAL : IVisitDAL
    {
        private readonly string connectionString;

        public VisitDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool CheckIn(int memberId)
        {
            Visit trip = new Visit();
            trip.Arrival = DateTime.Now;
            trip.MemberId = memberId;
            trip.IsActive = true;

            string SQL_INSERT_STRING = "INSERT INTO visit " +
                        "(memberId, arrival, isActive)" +
                        "VALUES (@memberId, @arrival, @isActive);";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Dictionary<string, object> dynamicParamterArgs =
                conn.Open();

                int rowsAffected = conn.Execute(SQL_INSERT_STRING, trip);


                return rowsAffected > 0;
            }

        }
        public bool HasOpenVisit(int memberId)
        {
            string SQL_SELECT_STRING = "SELECT count(*) from visit WHERE memberId = @memberId AND departure IS NULL;";

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand(SQL_SELECT_STRING, myConnection);
                cmd.Parameters.AddWithValue("@memberId", memberId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }

        }
        public bool CheckOut(int memberId)
        {
            if (HasOpenVisit(memberId))
            {
                string SQL_UPDATE_STRING = "UPDATE visit SET departure = @departure WHERE memberId = @memberId" +
                    " AND departure IS NULL;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(SQL_UPDATE_STRING, conn);
                    cmd.Parameters.AddWithValue("@memberId", memberId);
                    cmd.Parameters.AddWithValue("@departure", DateTime.Now);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected == 1;
                }
            }
            else
            {
                return false;
            }

        }


        public void DeleteVisit(Visit trip)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE visit SET IsActive = 'false' WHERE visitId = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", trip.VisitID);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public Visit GetVisit(int id)
        {
            Visit visit = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string CmdText = "SELECT * FROM visit WHERE memberId = @memberId;";
                    Dictionary<string, object> dynamicParamterArgs = new Dictionary<string, object>();
                    dynamicParamterArgs.Add("@memberId", id);

                    visit = conn.Query<Visit>(CmdText, new DynamicParameters(dynamicParamterArgs)).ToList().FirstOrDefault();
                }

                return visit;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public List<Visit> GetVisits(int memberId)
        {
            List<Visit> output = new List<Visit>();
            string SQL_GET_VISITS = "SELECT * FROM visit where memberId = @memberId AND departure IS NOT NULL" +
                " ORDER BY arrival desc";
            Dictionary<string, object> dynamicParameterArgs = new Dictionary<string, object>();
            dynamicParameterArgs.Add("@memberId", memberId);

            using(SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();
                output = myConnection.Query<Visit>(SQL_GET_VISITS, dynamicParameterArgs).ToList();
            }
                       
            return output;
        }       
    }
}