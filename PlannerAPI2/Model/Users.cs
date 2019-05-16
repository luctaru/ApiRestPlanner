using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerAPI2.Model
{
    public class Users
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime LastChangedDate { get; set; }

        public Boolean CanCreatePlan { get; set; }

        public Boolean Removed { get; set; }
    }
}
