using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidation.TaskAllocationClasses
{
    class Allocations
    {
        public int Count { get; set; }
        public int Taks { get; set; }
        public int Processor { get; set; }
        public List<Allocation> allocations { get; set; } = new List<Allocation>(); 
    }
}
