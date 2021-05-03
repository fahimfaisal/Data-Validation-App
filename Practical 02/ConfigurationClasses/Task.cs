using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidation.ConfigurationClasses
{
    class Task
    {
        public int ID { get; set; }
        public double runtime { get; set; }
        public double referenceFrequency { get; set; }
        public double ram { get; set; }
        public double download { get; set; }
        public double upload { get; set; }


        public Task()
        {

        }
        public Task(int Id, Double runtime, Double referenceFrequency, Double ram, Double download, Double upload)
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
