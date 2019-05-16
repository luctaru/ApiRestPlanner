using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerAPI.Repositories
{
    public abstract class AbstractRepository<T>
    {
        private string _connectionString;
        protected string ConnectionString => _connectionString;
        public AbstractRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");

        }
        public abstract void Add(T item);
        public abstract void Remove(int id);
        public abstract void Update(T item);
        public abstract T FindByID(int id);
        public abstract IEnumerable<T> FindAll();
    }
}