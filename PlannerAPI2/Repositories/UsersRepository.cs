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
    public class UsersRepository : AbstractRepository<Users>
    {
        public UsersRepository(IConfiguration configuration) : base(configuration) { }

        public override IEnumerable<Users> FindAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }
                IEnumerable<Users> users = dbConnection.Query<Users>("SELECT * FROM users");
                return (List<Users>)users;
            }
        }

        public override void Add(Users item)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "INSERT INTO users (name, register_date, last_changed_date, can_create_plan, removed)"
                                + " VALUES(@Name, GETDATE(), GETDATE(), @CanCreatePlan, @Removed)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, item);
            }
        }
        public override void Remove(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                //string sQuery = "DELETE FROM users"
                //            + " WHERE id = @Id";
                string sQuery = "UPDATE users SET removed = 1"
                            + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }
        public override void Update(Users item)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "UPDATE users SET name = @Name,last_changed_date = GETDATE(),can_create_plan = @CanCreatePlan,removed = @Removed"
                            + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Query(sQuery, item);
            }
        }
        public override Users FindByID(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                string sQuery = "SELECT * FROM users"
                            + " WHERE id = @Id";
                dbConnection.Open();
                return dbConnection.Query<Users>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }
    }
}
