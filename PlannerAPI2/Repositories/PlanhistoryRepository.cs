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
    public class PlanhistoryRepository : AbstractRepository<Planhistory>
    {
        public PlanhistoryRepository(IConfiguration configuration) : base(configuration) { }

        public override IEnumerable<Planhistory> FindAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }

                var query = dbConnection.Query<Planhistory, Plans, Planstatus, Plantype, Users, Planhistory>(@"SELECT ph.*, p.*, ps.*, pt.*, u.* 
                                        FROM plan_history ph INNER JOIN plans p ON ph.id_plan = p.id
                                        INNER JOIN plan_status ps ON ph.id_plan_status = ps.id
										INNER JOIN plan_type pt ON p.id_type = pt.id
										INNER JOIN users u ON p.id_user = u.id"
                , (ph, p, ps, pt, u) =>
                {
                    ph.Plans = p;
                    ph.Status = ps;
                    p.Status = ps;
                    p.Type = pt;
                    p.Users = u;
                    return ph;
                }, null, splitOn: "id, id, id, id, id").AsList();


                return query;
            }
        }
        public override Planhistory FindByID(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }

                var query = dbConnection.Query<Planhistory, Plans, Planstatus, Plantype, Users, Planhistory>(@"SELECT ph.*, p.*, ps.*, pt.*, u.* 
                                        FROM plan_history ph INNER JOIN plans p ON ph.id_plan = p.id
                                        INNER JOIN plan_status ps ON ph.id_plan_status = ps.id
										INNER JOIN plan_type pt ON p.id_type = pt.id
										INNER JOIN users u ON p.id_user = u.id 
                                        WHERE p.id = @Id"
                , (ph, p, ps, pt, u) =>
                {
                    ph.Plans = p;
                    ph.Status = ps;
                    p.Status = ps;
                    p.Type = pt;
                    p.Users = u;
                    return ph;
                }, new { Id = id }, splitOn: "id, id, id, id, id").AsList();


                return query.FirstOrDefault();

            }
        }

        public override void Add(Planhistory item)
        {
            throw new Exception("Não é necessário este método na regra de negócio!");
        }

        public override void Remove(int id)
        {
            throw new Exception("Não é necessário este método na regra de negócio!");
        }

        public override void Update(Planhistory item)
        {
            throw new Exception("Não é necessário este método na regra de negócio!");
        }
    }
}
