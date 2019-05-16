using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerAPI2.Model
{
    public class Userhistory
    {
        public int Id { get; set; }

        public Users Users { get; set; }

        public Boolean Status { get; set; }

        public Boolean CreateNewPlan { get; set; }

        public DateTime Date { get; set; }
    }
}
