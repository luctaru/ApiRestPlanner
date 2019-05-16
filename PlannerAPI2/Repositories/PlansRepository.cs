using Dapper;
using Microsoft.Extensions.Configuration;
using PlannerAPI;
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
    public class PlansRepository : AbstractRepository<Plans>
    {
        public PlansRepository(IConfiguration configuration) : base(configuration) { }

        public override IEnumerable<Plans> FindAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }

                var query = dbConnection.Query<Plans, Planstatus, Plantype, Users, Plans>(@"SELECT p.*, ps.*, pt.*, u.* 
                                        FROM plans p INNER JOIN plan_status ps ON p.id_status = ps.id
                                        INNER JOIN plan_type pt ON p.id_type = pt.id
                                        INNER JOIN users u ON p.id_user = u.id"
                , (p, ps, pt, u) =>
                {
                    p.Users = u;
                    p.Status = ps;
                    p.Type = pt;
                    return p;
                }, null, splitOn: "id, id, id, id").AsList();

                
                return query;
            }
        }

        public override void Add(Plans item)
        {

            throw new Exception("Para este método insira Plansend!");
            
        }

        public void Add(Plansend item)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "INSERT INTO plans (name, id_type, id_user, id_status, start_date, end_date, description, cost)"
                                + " VALUES(@Name, @Type, @Users, @Status, @StartDate, @EndDate, @Description, @Cost)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, item);
            }
        }

        public override void Remove(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "DELETE FROM plans"
                            + " WHERE id = @Id";
                //string sQuery = "UPDATE users SET removed = 1"
                //            + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }
        public override void Update(Plans item)
        {

            throw new Exception("Para este método insira Plansend!");

        }
        public void Update(Plansend item)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "UPDATE plans SET name = @Name, id_type = @Type, id_user = @Users, id_status = @Status, start_date = @StartDate, end_date = @EndDate, description = @Description, cost = @Cost"
                            + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Query(sQuery, item);
            }
        }
        public override Plans FindByID(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }

                var query = dbConnection.Query<Plans, Planstatus, Plantype, Users, Plans>(@"SELECT p.*, ps.*, pt.*, u.* 
                                        FROM plans p INNER JOIN plan_status ps ON p.id_status = ps.id
                                        INNER JOIN plan_type pt ON p.id_type = pt.id
                                        INNER JOIN users u ON p.id_user = u.id 
                                        WHERE p.id = @Id"
                , (p, ps, pt, u) =>
                {
                    p.Users = u;
                    p.Status = ps;
                    p.Type = pt;
                    return p;
                }, new { Id = id}, splitOn: "id, id, id, id").AsList();


                return query.FirstOrDefault();

            }
        }
    }
}
