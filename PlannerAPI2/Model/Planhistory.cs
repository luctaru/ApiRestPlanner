using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerAPI2.Model
{
    public class Planhistory
    {
        public int Id { get; set; }

        public Plans Plans { get; set; }

        public Planstatus Status { get; set; }

        public DateTime Date { get; set; }
    }
}
