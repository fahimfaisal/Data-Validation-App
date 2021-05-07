using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataValidation
{
    class FileReader
    {

        //public bool readTaff(String path, out List<string> Errors)
        //{
        //    StreamReader streamReader = new StreamReader(path);
        //    String data = "";

        //    while (!streamReader.EndOfStream)
        //    {

        //        String line = streamReader.ReadLine();
        //        if (line.Contains(Keywords.config))
        //        {
        //            data += "<p style='color:red'>" + line + "</p>";

        //        }
        //        else
        //        {
        //            data += "<p>" + line + "</p>";
        //        }
               
                
        //    }

        //    streamReader.Close();

            
        //}



        public string readTaff(String path, out List<string> Errors )
        {
            StreamReader streamReader = new StreamReader(path);
            String data = "";
            Errors = new List<string>();
            while (!streamReader.EndOfStream)
            {
                try
                {
                    String line = streamReader.ReadLine();
                    line = line.Trim();

                    
                    
                    if (line.Contains("//") && !line.StartsWith("//"))
                    {
                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                        Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                    }
                    else if (line.StartsWith("CONFIGURATION-DATA"))
                    {

                        data += "<p>" + line + "</p>";

                        while (!line.StartsWith("END-CONFIGURATION-DATA"))
                        {
                            line = streamReader.ReadLine();

                            line = line.Trim();


                            if (line.StartsWith("//"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.Length == 0)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.StartsWith("FILENAME"))
                            {

                                string orginalLine = line;
                                string[] texts = line.Split('=');

                                if (texts.Length.Equals(2))
                                {
                                    texts[1] = texts[1].Trim();

                                    if (texts[1].StartsWith("\"") && texts[1].EndsWith("\""))
                                    {
                                        line = texts[1].Trim('"');
                                        line = line.Trim();



                                        if (line.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                                        {
                                            data += "<p>" + orginalLine + "</p>";
                                        }
                                        else
                                        {
                                            Errors.Add("Invalid name");

                                            data += "<p>"+"<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : NO ATTRIBUTE PROVIDED" + "</span>"+"</p>";

                                        }
                                    }

                                    else
                                    {
                                        Errors.Add("FILENAME is not a string");

                                        data += "<p>"+"<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : NO ATTRIBUTE PROVIDED" + "</span>" + "</p>";
                                    }

                                }
                                else
                                {
                                    Errors.Add("FILENAME NOT PROVIDED");
                                }
                                
                            }
                            else if (line.StartsWith("END-CONFIGURATION-DATA"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else
                            {
                                data += "<p>"+"<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END-CONFIGURATION-DATA" + "</span>" + "</p>"; 
                                Errors.Add("INVALID LINE");
                            }

                        }

                    }
                    else if (line.StartsWith("ALLOCATIONS"))
                    {

                        data += "<p>" + line + "</p>";

                        while (!line.StartsWith("END-ALLOCATIONS"))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();


                            if (line.StartsWith("//"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.Length == 0)
                            {
                                
                            }
                            else if (line.StartsWith("COUNT"))
                            {
                                

                                string[] lineError = ValidateInt(line, "COUNT");

                                data += lineError[0];
                                if(!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }


                            }
                            else if (line.StartsWith("TASKS"))
                            {


                               string[] lineError = ValidateInt(line, "TASKS");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }


                            }
                            else if (line.StartsWith("PROCESSORS"))
                            {
                                string[] lineError = ValidateInt(line, "PROCESSORS");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }

                            }
                            else if (line.StartsWith("END-ALLOCATIONS"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.StartsWith("ALLOCATION"))
                            {
                                data += "<p>" + line + "</p>";
                                bool isId = false;
                                bool isMap = false;
                                while (!line.StartsWith("END-ALLOCATION"))
                                {
                                    line = streamReader.ReadLine();
                                    line = line.Trim();
                                    
                                    if (line.StartsWith("\\"))
                                    {
                                        data += "<p>" + line + "</p>";
                                    }

                                    else if (line.Length == 0)
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else if (line.StartsWith("ID"))
                                    {
                                        if(isId)
                                        {
                                            data += "<p>"+"<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END ALLOCATION FOUND" + "</span>"+"</p>";
                                        }
                                        else
                                        {
                                            string[] lineError = ValidateInt(line, "ID");

                                            data += lineError[0];
                                            if (!lineError[1].Equals("empty"))
                                            {
                                                Errors.Add(lineError[1]);
                                            }

                                            isId = true;
                                        }
                                       

                                    }
                                    else if (line.StartsWith("MAP"))
                                    {
                                       

                                        if (isMap)
                                        {
                                            data += "</p>"+"<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END ALLOCATION FOUND" + "</span>"+"<p>";
                                        }
                                        else
                                        {
                                            int id = 0;
                                            String orginalLine = line;
                                            String[] texts = line.Split('=');
                                            
                                            if (texts.Length == 2)
                                            {
                                                string[] lineError = ValidateMap(line, "MAP");

                                                data += lineError[0];
                                                if (!lineError[1].Equals("empty"))
                                                {
                                                    Errors.Add(lineError[1]);
                                                }

                                                isMap = true;
                                            }
                                            
                                           
                                        }
                                       

                                    }
                                    else if (line.StartsWith("END-ALLOCATION"))
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else if (line.StartsWith("ALLOCATION"))
                                    {
                                        data += "</p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END ALLOCATION FOUND" + "</span>" + "<p>";
                                    }
                                    else
                                    {
                                        Errors.Add("Invalid line");
                                        data += "<p>"+"<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO ATTRIBUTE PROVIDED" + "</span>"+"</p>";
                                    }
                                }




                            }
                            else
                            {
                                Errors.Add("Inavlid line");
                                data += "<p>"+"<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                            }

                        }



                    }
                    else
                    {
                        data += "<p>" + line + "</p>";
                    }

                }


                catch (Exception e)
                {
                    return data;
                }

            }
            streamReader.Close();

            return data;
        }

        public string Readcff(String path, out List<String> Errors)
        {
            StreamReader streamReader = new StreamReader(path);
            String data = "";
            Errors = new List<string>();
            while (!streamReader.EndOfStream)
            {
                try
                {
                    String line = streamReader.ReadLine();
                    line = line.Trim();


                    if (line.StartsWith("//"))
                    {
                        data += "<p>" + line + "</p>";
                    }
                    else if (line.Length == 0)
                    {

                    }
                    if (line.Contains("//") && !line.StartsWith("//"))
                    {
                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                        Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                    }

                    if (line.StartsWith("LOGFILE"))
                    {
                        data += "<p>" + line + "</p>";

                        while (!line.StartsWith("END-LOGFILE"))                           
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();
                            
                            if (line.StartsWith("//"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.Length == 0)
                            {

                            }
                            if (line.StartsWith("DEFAULT"))
                            {
                                string orginalLine = line;
                                string[] texts = line.Split('=');

                                if (texts.Length.Equals(2))
                                {
                                    texts[1] = texts[1].Trim();

                                    if (texts[1].StartsWith("\"") && texts[1].EndsWith("\""))
                                    {
                                        line = texts[1].Trim('"');
                                        line = line.Trim();



                                        if (line.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                                        {
                                            data += "<p>" + orginalLine + "</p>";
                                        }
                                        else
                                        {
                                            Errors.Add("Invalid name");

                                            data += "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : NO ATTRIBUTE PROVIDED" + "</span>" + "</p>";

                                        }
                                    }

                                    else
                                    {
                                        Errors.Add("FILENAME is not a string");

                                        data += "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : NO ATTRIBUTE PROVIDED" + "</span>" + "</p>";
                                    }

                                }
                                else
                                {
                                    Errors.Add("FILENAME NOT PROVIDED");
                                }
                            }
                            else if (line.StartsWith("END-LOGFILE"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else
                            {
                                Errors.Add("Inavlid line");
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                            }
                        }
                    }

                    if (line.StartsWith("LIMITS"))
                    {
                        data += "<p>" + line + "</p>";
                        while (!line.StartsWith("END-LIMITS"))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();

                            if (line.StartsWith("//"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.Length == 0)
                            {

                            }
                            if (line.StartsWith("MINIMUM-TASKS"))
                            {
                                string[] lineError = ValidateInt(line, "MINIMUM-TASKS");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MAXIMUM-TASKS"))
                            {
                                string[] lineError = ValidateInt(line,"MAXIMUM-TASKS");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MAXIMUM-TASKS"))
                            {
                                string[] lineError = ValidateInt(line,"MAXIMUM-TASKS");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MINIMUM-PROCESSORS"))
                            {
                                string[] lineError = ValidateInt(line, "MINIMUM-PROCESSORS");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MAXIMUM-PROCESSORS"))
                            {
                                string[] lineError = ValidateInt(line, "MAXIMUM-PROCESSORS");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MINIMUM-PROCESSOR-FREQUENCIES"))
                            {
                                string[] lineError = ValidateDouble(line,"MINIMUM-PROCESSOR-FREQUENCIES");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MAXIMUM-PROCESSOR-FREQUENCIES"))
                            {
                                string[] lineError = ValidateDouble(line,"MAXIMUM-PROCESSOR-FREQUENCIES");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MINIMUM-RAM"))
                            {
                                string[] lineError = ValidateInt(line, "MINIMUM-RAM");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MAXIMUM-RAM"))
                            {
                                string[] lineError = ValidateInt(line, "MAXIMUM-RAM");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MINIMUM-DOWNLOAD"))
                            {
                                string[] lineError = ValidateInt(line,"MINIMUM-DOWNLOAD");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MAXIMUM-DOWNLOAD"))
                            {
                                string[] lineError = ValidateInt(line, "MAXIMUM-DOWNLOAD");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MINIMUM-UPLOAD"))
                            {
                                string[] lineError = ValidateInt(line, "MINIMUM-UPLOAD");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("MAXIMUM-UPLOAD"))
                            {
                                string[] lineError = ValidateInt(line, "MAXIMUM-UPLOAD");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if(line.StartsWith("END-LIMITS"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else
                            {
                                Errors.Add("Inavlid line");
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                            }
                        }
                    }

                    else if (line.StartsWith("PROGRAM"))
                    {
                        data += "<p>" + line + "</p>";
                        while (!line.StartsWith("END-PROGRAM"))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();

                            if (line.StartsWith("//"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.Length == 0)
                            {

                            }
                            else if (line.StartsWith("DURATION"))
                            {
                                string[] lineError = ValidateDouble(line, "DURATION");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("TASKS"))
                            {
                                string[] lineError = ValidateInt(line, "TASKS");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith("PROCESSORS"))
                            {
                                 string[] lineError = ValidateInt(line,"PROCESSORS");

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }

                            }
                            else if (line.StartsWith("END-PROGRAM"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else
                            {
                                Errors.Add("Inavlid line");
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                            }
                        }


                       
                    }


                    else if (line.StartsWith("TASKS"))
                    {
                        data += "<p>" + line + "</p>";

                        while (!line.StartsWith("END-TASKS"))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();

                            if (line.StartsWith("//"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.Length == 0)
                            {

                            }
                            else if (line.StartsWith("TASK"))
                            {
                                data += "<p>" + line + "</p>";

                                while (!line.StartsWith("END-TASK"))
                                {
                                    line = streamReader.ReadLine();
                                    line = line.Trim();


                                    if (line.StartsWith("//"))
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else if (line.Length == 0)
                                    {

                                    }
                                    else if (line.StartsWith("ID"))
                                    {
                                        string[] lineError = ValidateInt(line, "ID");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("RUNTIME"))
                                    {
                                        string[] lineError = ValidateDouble(line, "RUNTIME");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("REFERENCE-FREQUENCY"))
                                    {
                                        string[] lineError = ValidateDouble(line, "REFERENCE-FREQUENCY");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("RAM"))
                                    {
                                        string[] lineError = ValidateDouble(line, "RAM");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("DOWNLOAD"))
                                    {
                                        string[] lineError = ValidateInt(line, "DOWNLOAD");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("UPLOAD"))
                                    {
                                        string[] lineError = ValidateInt(line, "UPLOAD");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("END-TASK"))
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else
                                    {
                                        Errors.Add("Inavlid line");
                                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                                    }

                                }
                            }
                            else if (line.StartsWith("END-TASKS"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else
                            {
                                Errors.Add("Inavlid line");
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                            }

                        }
                    }



                    else if (line.StartsWith("PROCESSORS"))
                    {
                        data += "<p>" + line + "</p>";
                       
                        while (!line.StartsWith("END-PROCESSORS"))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();

                            if (line.StartsWith("//"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.Length == 0)
                            {

                            }
                            else if (line.StartsWith("PROCESSOR"))
                            {
                                data += "<p>" + line + "</p>";

                                while (!line.StartsWith("END-PROCESSOR"))
                                {
                                    line = streamReader.ReadLine();
                                    line = line.Trim();


                                    if (line.StartsWith("//"))
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else if (line.Length == 0)
                                    {

                                    }
                                    else if (line.StartsWith("ID"))
                                    {
                                        string[] lineError = ValidateInt(line, "ID");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("TYPE"))
                                    {
                                        string[] lineError = ValidateString(line, "TYPE");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("FREQUENCY"))
                                    {
                                        string[] lineError = ValidateDouble(line, "FREQUENCY");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("RAM"))
                                    {
                                        string[] lineError = ValidateInt(line,"RAM");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("DOWNLOAD"))
                                    {
                                        string[] lineError = ValidateInt(line, "DOWNLOAD");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("UPLOAD"))
                                    {
                                        string[] lineError = ValidateInt(line, "UPLOAD");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith("END-PROCESSOR"))
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else
                                    {
                                        Errors.Add("Inavlid line");
                                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                                    }


                                }

                            }
                            else if (line.StartsWith("END-PROCESSORS"))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else
                            {
                                Errors.Add("Inavlid line");
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                            }

                        }
                    }



                }
                catch (Exception e)
                {
                    return data;
                }

            }
            streamReader.Close();

            return data;
        }
















            public string[] ValidateInt(String line, string section)
            {
                string[] dataError = new string[2];
                string error = "";
                int count = 0;
                string orginalLine = line;
                string[] texts = line.Split('=');

                if (orginalLine.Contains("//") && !orginalLine.StartsWith("//"))
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : MIXING STATEMENT AND COMMENT IS NOT ALLOWED" + "</span>" + "</p>";
                    error = "COMMENT AND STATEMENT IS MIXED IN: " + section;
                    dataError[0] = line;
                    dataError[1] = error;


                }
                else if (texts.Length.Equals(2))
                {
                    if (!texts[0].Equals(section))
                    {
                        line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : INVALID" + "ATTRIBUTE" + "NAME" + section + "</span>" + "</p>";
                        error = "INAVLID ATTRIBUTE NAME: " + section;

                        dataError[0] = line;
                        dataError[1] = error;


                    }


                    else if (int.TryParse(texts[1], out count))
                    {
                        line = "<p>" + orginalLine + "</p>";
                        dataError[0] = line;
                        dataError[1] = "empty";


                    }
                    else
                    {
                        line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR :" + section + " SHOULD BE AN INTEGER" + "</span>" + "</p>";

                        dataError[0] = line;
                        dataError[1] = section + "IS NOT AN INTEGER";



                    }

                }
                else
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR :  MISSING ASSIGNMENT SYMBOL" + "</span>" + "</p>";
                    dataError[0] = line;
                    dataError[1] = "NO ASSIGMENT SYMBOL FOUND OF" + section;


                }

                return dataError;
            }


            public string[] ValidateDouble(String line, string section)
            {
                string[] dataError = new string[2];
                string error = "";
                double count;
                string orginalLine = line;
                string[] texts = line.Split('=');

                if (orginalLine.Contains("//") && !orginalLine.StartsWith("//"))
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : MIXING STATEMENT AND COMMENT IS NOT ALLOWED" + "</span>" + "</p>";
                    error = "COMMENT AND STATEMENT IS MIXED IN: " + section;
                    dataError[0] = line;
                    dataError[1] = error;


                }
                else if (texts.Length.Equals(2))
                {
                    if (!texts[0].Equals(section))
                    {
                        line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : INVALID" + "ATTRIBUTE" + "NAME" + section + "</span>" + "</p>";
                        error = "INAVLID ATTRIBUTE NAME: " + section;

                        dataError[0] = line;
                        dataError[1] = error;


                    }


                    else if (Double.TryParse(texts[1], out count))
                    {
                        line = "<p>" + orginalLine + "</p>";
                        dataError[0] = line;
                        dataError[1] = "empty";


                    }
                    else
                    {
                        line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR :" + section + " SHOULD BE AN NUMBER" + "</span>" + "</p>";

                        dataError[0] = line;
                        dataError[1] = section + "IS NOT AN INTEGER";



                    }

                }
                else
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR :  MISSING ASSIGNMENT SYMBOL" + "</span>" + "</p>";
                    dataError[0] = line;
                    dataError[1] = "NO ASSIGMENT SYMBOL FOUND OF" + section;


                }

                return dataError;
            }
        public string[] ValidateString(String line, string section)
        {
            string[] dataError = new string[2];
            string error = "";
            int count = 0;
            string orginalLine = line;
            string[] texts = line.Split('=');
            string pattern = "[^0-9]";
            

            if (orginalLine.Contains("//") && !orginalLine.StartsWith("//"))
            {
                line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : MIXING STATEMENT AND COMMENT IS NOT ALLOWED" + "</span>" + "</p>";
                error = "COMMENT AND STATEMENT IS MIXED IN: " + section;
                dataError[0] = line;
                dataError[1] = error;


            }
            else if (texts.Length.Equals(2))
            {
                if (!texts[0].Equals(section))
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : INVALID" + "ATTRIBUTE" + "NAME" + section + "</span>" + "</p>";
                    error = "INAVLID ATTRIBUTE NAME: " + section;

                    dataError[0] = line;
                    dataError[1] = error;


                }
                else if (!texts[1].StartsWith("\"") && !texts[1].EndsWith("\""))
                {

                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR :  IS NOT A STRING" + "</span>" + "</p>";
                    dataError[0] = line;
                    dataError[1] = "NOT A STRING" + section;


                }
                else
                {
                    String strings = texts[1].Trim('"');
                    
                    Match m = Regex.Match(pattern, strings);
                    if (!m.Success)
                    {
                        line = "<p>" + orginalLine + "</p>";
                        dataError[0] = line;
                        dataError[1] = "empty";
                    }
                    else
                    {

                        line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR :  IS NOT A STRING" + "</span>" + "</p>";
                        dataError[0] = line;
                        dataError[1] = "NOT A STRING" + section;

                    }


                }

                
            }
            else
            {
                line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR :  MISSING ASSIGNMENT SYMBOL" + "</span>" + "</p>";
                dataError[0] = line;
                dataError[1] = "NO ASSIGMENT SYMBOL FOUND OF" + section;
            }
            
            return dataError;
        }
            public string[] ValidateMap(String line, string section)
            {
                string[] dataError = new string[2];
                string error = "";

                string orginalLine = line;
                string[] texts = line.Split('=');
                string pattern = "^s*MAP=(0|1)(,0|,1)*(;(0|1)(,0|,1)*)*$";
                Match m = Regex.Match(orginalLine, pattern);

                if (orginalLine.Contains("//") && !orginalLine.StartsWith("//"))
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : MIXING STATEMENT AND COMMENT IS NOT ALLOWED" + "</span>" + "</p>";
                    error = "COMMENT AND STATEMENT IS MIXED IN: " + section;
                    dataError[0] = line;
                    dataError[1] = error;


                }
                else if (texts.Length.Equals(2))
                {
                    if (!texts[0].Equals(section))
                    {
                        line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : INVALID" + "ATTRIBUTE" + "NAME" + section + "</span>" + "</p>";
                        error = "INAVLID ATTRIBUTE NAME: " + section;

                        dataError[0] = line;
                        dataError[1] = error;


                    }

                    else if (m.Success)
                    {
                        line = "<p>" + orginalLine + "</p>";
                        dataError[0] = line;
                        dataError[1] = "empty";
                    }

                    else
                    {
                        line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : INVALID MAPPING" + "</span>" + "</p>";

                        dataError[0] = line;
                        dataError[1] = "INVALID MAPPING";



                    }

                }
                else
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR :  MISSING ASSIGNMENT SYMBOL" + "</span>" + "</p>";
                    dataError[0] = line;
                    dataError[1] = "NO ASSIGMENT SYMBOL FOUND OF" + section;


                }

                return dataError;
            }

        

    }
}
