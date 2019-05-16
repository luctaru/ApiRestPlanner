using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerAPI.Repositories
{
    public class TypeRepository : AbstractRepository<Plantype>
    {
        public TypeRepository(IConfiguration configuration) : base(configuration) { }

        public override IEnumerable<Plantype> FindAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                if(dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }
                IEnumerable<Plantype> types = dbConnection.Query<Plantype>("SELECT * FROM plan_type");
                return (List<Plantype>)types;
            }
        }

        public override void Add(Plantype item)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "INSERT INTO plan_type (name)"
                                + " VALUES(@Name)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, item);
            }
        }
        public override void Remove(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "DELETE FROM plan_type"
                            + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }
        public override void Update(Plantype item)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "UPDATE plan_type SET name = @Name"
                            + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Query(sQuery, item);
            }
        }
        public override Plantype FindByID(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "SELECT * FROM plan_type"
                            + " WHERE id = @Id";
                dbConnection.Open();
                return dbConnection.Query<Plantype>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }
    }
}