using PlannerAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerAPI2.Model
{
    public class Plans
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Plantype Type { get; set; }

        public Users Users { get; set; }

        public Planstatus Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public Decimal Cost { get; set; }
    }
}
