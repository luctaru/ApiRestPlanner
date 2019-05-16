using Dapper;
using Microsoft.Extensions.Configuration;
using PlannerAPI.Repositories;
using PlannerAPI2.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerAPI2.Repositories
{
    public class StatusRepository : AbstractRepository<Planstatus>
    {
        public StatusRepository(IConfiguration configuration) : base(configuration) { }

        public override IEnumerable<Planstatus> FindAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }
                IEnumerable<Planstatus> status = dbConnection.Query<Planstatus>("SELECT * FROM plan_status");
                return (List<Planstatus>)status;
            }
        }

        public override void Add(Planstatus item)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "INSERT INTO plan_status (name)"
                                + " VALUES(@Name)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, item);
            }
        }
        public override void Remove(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "DELETE FROM plan_status"
                            + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }
        public override void Update(Planstatus item)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "UPDATE plan_status SET name = @Name"
                            + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Query(sQuery, item);
            }
        }
        public override Planstatus FindByID(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "SELECT * FROM plan_status"
                            + " WHERE id = @Id";
                dbConnection.Open();
                return dbConnection.Query<Planstatus>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }
    }
}
