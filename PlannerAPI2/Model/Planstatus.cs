using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerAPI2.Model
{
    public class Planstatus
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
