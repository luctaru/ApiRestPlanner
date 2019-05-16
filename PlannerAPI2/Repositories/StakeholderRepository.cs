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
    public class StakeholderRepository : AbstractRepository<Stakeholder>
    {
        public StakeholderRepository(IConfiguration configuration) : base(configuration) { }

        public override IEnumerable<Stakeholder> FindAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }

                var query = dbConnection.Query<Stakeholder, Plans, Planstatus, Plantype, Users, Users, Stakeholder>
                    (@"SELECT piu.*, p.*, ps.*, pt.*, u.*, r.* FROM plan_interested_user piu 
                                        INNER JOIN plans p ON piu.id_plan = p.id
                                        INNER JOIN plan_status ps ON p.id_status = ps.id
										INNER JOIN plan_type pt ON p.id_type = pt.id
										INNER JOIN users u ON piu.id_user = u.id
                                        INNER JOIN users r ON r.id = p.id_user"
                , (s, p, ps, pt, u, r) =>
                {
                    s.Plans = p;
                    s.Users = u;
                    p.Users = r;
                    p.Status = ps;
                    p.Type = pt;
                    return s;
                }, null, splitOn: "id, id, id, id, id, id").AsList();


                return query;
            }
        }

        public override void Add(Stakeholder item)
        {

            throw new Exception("Para este método insira Plansend!");

        }

        public void Add(Stakeholdersend item)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "INSERT INTO plan_interested_user (id_plan, id_user) VALUES(@Plans, @Users)";
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
        public override void Update(Stakeholder item)
        {

            throw new Exception("Para este método insira Plansend!");

        }
        public void Update(Stakeholdersend item)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string sQuery = "UPDATE plan_interested_user SET id_plan = @Plans, id_user = @Users"
                            + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Query(sQuery, item);
            }
        }
        public override Stakeholder FindByID(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }

                var query = dbConnection.Query<Stakeholder, Plans, Planstatus, Plantype, Users, Users, Stakeholder>
                    (@"SELECT piu.*, p.*, ps.*, pt.*, u.*, r.* FROM plan_interested_user piu 
                                        INNER JOIN plans p ON piu.id_plan = p.id
                                        INNER JOIN plan_status ps ON p.id_status = ps.id
										INNER JOIN plan_type pt ON p.id_type = pt.id
										INNER JOIN users u ON piu.id_user = u.id
                                        INNER JOIN users r ON r.id = p.id_user
                                        WHERE piu.id = @Id"
                , (s, p, ps, pt, u, r) =>
                {
                    s.Plans = p;
                    s.Users = u;
                    p.Users = r;
                    p.Status = ps;
                    p.Type = pt;
                    return s;
                }, new { Id = id }, splitOn: "id, id, id, id, id, id").AsList();


                return query.FirstOrDefault();

            }
        }
    }
}
