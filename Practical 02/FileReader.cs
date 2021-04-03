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
            String line = "";

            while (!streamReader.EndOfStream)
            {
               
                line += "<p>" + streamReader.ReadLine() + "</p>";
                
            }

            streamReader.Close();

            return line;
        }
    }
}
