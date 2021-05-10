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
        public double Runtime { get; set; }
        public double ReferenceFrequency { get; set; }
        public int Ram { get; set; }
        public int Download { get; set; }
        public int Upload { get; set; }


        public Task()
        {

        }
        public Task(int Id, Double runtime, Double referenceFrequency, int ram, int download, int upload)
        {
            this.ID = Id;
            this.Runtime = runtime;
            this.ReferenceFrequency = referenceFrequency;
            this.Ram = ram;
            this.Download = download;
            this.Upload = upload;
        }


    }
}
