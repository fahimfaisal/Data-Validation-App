using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidation.ConfigurationClasses
{
    class Processor
    {
       public int ID { get; set; }
       public ProcessorType Type { get; set; }
       public Double Frequency { get; set; }
        public Double Ram { get; set; }
        public Double Download { get; set; }
        
       public Double Upload { get; set; }

        

        public Processor()
        {

        }
        public Processor(int Id, ProcessorType type, Double frequency, Double ram, Double download, Double upload)
        {
            this.ID = Id;
            this.Type = type;
            this.Frequency = frequency;
            this.Ram = ram;
            this.Download = download;
            this.Upload = upload;
        }
        public double CalcTime(Task task)
        {

            double val = task.runtime * (task.referenceFrequency / Frequency);

            return val;

            
        }

        public Boolean CheckRam(Task task)
        {
            if (Ram >= task.ram)
                return true;



            return false;
        }



    }
}