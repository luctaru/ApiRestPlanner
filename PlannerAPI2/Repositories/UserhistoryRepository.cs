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

namespace PlannerAPI2.Repositories
{
    public class UserhistoryRepository : AbstractRepository<Userhistory>
    {
        public UserhistoryRepository(IConfiguration configuration) : base(configuration) { }

        public override IEnumerable<Userhistory> FindAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }

                var query = dbConnection.Query<Userhistory, Users, Userhistory>(@"SELECT uh.*, u.* FROM user_history uh INNER JOIN users u ON uh.id_user = u.id"
                , (uh, u) =>
                {
                    uh.Users = u;
                    return uh;
                }, null, splitOn: "id, id").AsList();


                return query;
            }
        }
        public override Userhistory FindByID(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }

                var query = dbConnection.Query<Userhistory, Users, Userhistory>(@"SELECT uh.*, u.* FROM user_history uh INNER JOIN users u ON uh.id_user = u.id
                                        WHERE uh.id = @Id"
                , (uh, u) =>
                {
                    uh.Users = u;
                    return uh;
                }, new { Id = id }, splitOn: "id, id").AsList();


                return query.FirstOrDefault();

            }
        }

        public override void Add(Userhistory item)
        {
            throw new Exception("Não é necessário este método na regra de negócio!");
        }

        public override void Remove(int id)
        {
            throw new Exception("Não é necessário este método na regra de negócio!");
        }

        public override void Update(Userhistory item)
        {
            throw new Exception("Não é necessário este método na regra de negócio!");
        }
    }
}
