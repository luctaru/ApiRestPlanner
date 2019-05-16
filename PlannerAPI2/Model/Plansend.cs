using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerAPI2.Model
{
    public class Plansend
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public int Users { get; set; }

        public int Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public Decimal Cost { get; set; }
    }
}
