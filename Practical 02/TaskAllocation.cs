using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidation
{
    class TaskAllocation
    {
        public string cffFilename { get; set; }

        List<string> Errors { get; set; }


        public Boolean getCffFilename(string taffFilename)
        {
            cffFilename = null;
            Errors = new List<string>();
            //open the TAff file and extract the css

            StreamReader sr = new StreamReader(taffFilename);

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();

                if (line.StartsWith("FILENAME"))
                {
                    String[] data = line.Split('=');

                    if (data.Length == 2)
                    {
                        if (data[1].StartsWith("\"") && data[1].EndsWith("\""))
                        {
                            line = data[1].Trim('"');

                            if (line.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                            {
                                String path = Path.GetDirectoryName(taffFilename);
                                cffFilename = path + line;
                            }
                            else
                            {
                                Errors.Add("Invalid name");
                            }
                        }
                        else
                        {
                            Errors.Add("Invalid filename");
                        }
                    }
                    else
                    {
                        Errors.Add("Not supplied");
                    }
                }
            }
            sr.Close();
            return (Errors.Count == 0);

        }
        public Boolean Validate(string taffFilename)
        {
            
            Errors = new List<string>();

            return (Errors.Count == 0);
        }




    }
}
