using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidation.ConfigurationClasses
{
    public class Task
    {
        public int ID { get; set; }
        public double runtime { get; set; }
        public double referenceFrequency { get; set; }
        public int ram { get; set; }
        public int download { get; set; }
        public int upload { get; set; }


        public Task()
        {

        }
        public Task(int Id, Double runtime, Double referenceFrequency, int ram, int download, int upload)
        {
            this.ID = Id;
            this.runtime = runtime;
            this.referenceFrequency = referenceFrequency;
            this.ram = ram;
            this.download = download;
            this.upload = upload;
        }


    }
}
