using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerAPI2.Model
{
    public class Stakeholder
    {
        public int Id { get; set; }

        public Plans Plans { get; set; }

        public Users Users { get; set; }
    }
}
