using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidation.ConfigurationClasses
{
    public class Processor
    {
       public int ID { get; set; }
       public ProcessorType Type { get; set; }
        public Double Frequency { get; set; }
        public int Ram { get; set; }
        public int Download { get; set; }
        
       public int Upload { get; set; }

        

        public Processor()
        {

        }
        public Processor(int Id, ProcessorType type, Double frequency, int ram, int download, int upload)
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

        public double CalcEnergy(Task task)
        {
            double val = CalcTime(task) * (Type.C2 * Frequency * Frequency + Type.C1 * Frequency + Type.C0);

            return val;
        }

        public bool CheckRam(Task task)
        {
            return Ram >= task.ram ? true : false;
   
        }

        public bool CheckDownloadSpeed(Task task)
        {
            return Download >= task.download ? true : false;
        }

        public bool CheckUploadSpeed(Task task)
        {
            return Upload >= task.upload ? true : false;
        }


    }
}