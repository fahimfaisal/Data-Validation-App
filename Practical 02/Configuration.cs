using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidation
{
    class Configuration
    {

        public string Logfile { get; set; }
        List<string> Errors { get; set; }

        public Boolean Validate(string cffFilename)
        {

            Errors = new List<string>();

            StreamReader sr = new StreamReader(cffFilename);
            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();
                Logfile = null;
                if (line.Length == 0)
                {

                }
                else if (line.StartsWith("//"))
                {

                }
                else if (line.StartsWith("DEFAULT"))
                {
                    String[] data = line.Split('=');

                    if (data.Length == 2)
                    {
                        if (data[1].StartsWith("\"") && data[1].EndsWith("\""))
                        {
                            line = data[1].Trim('"');

                            if (line.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                            {
                                String path = Path.GetDirectoryName(cffFilename);
                                Logfile = path + Path.DirectorySeparatorChar + line;
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

            

            if (Logfile == null)
            {
                Errors.Add("Missing Keyword");
            }

            return (Errors.Count == 0);


            

        }
    }
}
