using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidation
{
    class FileReader
    {

        public string readData(String path)
        {
            StreamReader streamReader = new StreamReader(path);
            String data = "";

            while (!streamReader.EndOfStream)
            {

                String line = streamReader.ReadLine();
                if (line.Contains(Keywords.config))
                {
                    data += "<p style='color:red'>" + line + "</p>";

                }
                else
                {
                    data += "<p>" + line + "</p>";
                }
               
                
            }

            streamReader.Close();

            return data;
        }
    }
}
