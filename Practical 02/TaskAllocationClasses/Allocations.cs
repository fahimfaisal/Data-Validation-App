using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidation.TaskAllocationClasses
{
    class Allocations
    {
        public int count { get; set; }
        public int taks { get; set; }
        public int processor { get; set; }
        public List<Allocation> allocations = new List<Allocation>(); 
    }
}
