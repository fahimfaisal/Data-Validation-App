using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataValidation
{
    
 
    
    public class FileReader
    {
       
        public FileReader()
        {

        }



        /// <summary>
        /// Method to read the taff file validate it
        /// </summary>
        /// <param name="path"> The address of the taff file</param>
        /// <param name="Errors">List of errors detected in the taff file</param>
        /// <returns>A string which cotains the full taff file texts</returns>

        public string ReadTaff(String path, out List<string> Errors )
        {
            StreamReader streamReader = new StreamReader(path);
            String data = "";
            Errors = new List<string>();
            String emptyCheck="^\\s*$";
            string commentCheck = @"^\s*//\s*.*$";
            string commentLineCheck = @"^\s*.*\s*//.*$";


            while (!streamReader.EndOfStream)
            {
                try
                {
                    String line = streamReader.ReadLine();
                    line = line.Trim();

                    
                    if (Regex.Match(line, commentCheck).Success)
                    {
                        data += "<p>" + line + "</p>";
                    }
                    else if (Regex.Match(line,commentLineCheck).Success)
                    {
                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                        Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                    }

                    else if(Regex.Match(line, emptyCheck).Success)
                    {
                        data += line ;
                    }
                    
                    else if (line.StartsWith(Keywords.config_start))
                    {

                        data += "<p>" + line + "</p>";
                        bool isFileName = false;

                        while (!line.StartsWith(Keywords.config_end))
                        {
                            line = streamReader.ReadLine();

                            line = line.Trim();


                            if (Regex.Match(line, commentCheck).Success)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (Regex.Match(line, commentLineCheck).Success)
                            {
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                            }

                            else if (Regex.Match(line, emptyCheck).Success)
                            {
                                data += line;
                            }
                            else if (line.StartsWith(Keywords.filename))
                            {
                                if (isFileName)
                                {
                                    data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END-CONFIGURATION STATEMENT FOUND" + "</span>" + "</p>";
                                    Errors.Add("NO END-CONFIGURATION STATEMENT FOUND");
                                }
                                else
                                {
                                    string[] lineError = ValidateString(line,Keywords.filename, true);

                                    data += lineError[0];
                                    if (!lineError[1].Equals("empty"))
                                    {
                                        Errors.Add(lineError[1]);
                                    }



                                    isFileName = true;
                                }
                               
                            }
                            else if (line.StartsWith(Keywords.config_end))
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
                    else if (line.StartsWith(Keywords.allocations_start))
                    {

                        data += "<p>" + line + "</p>";
                        bool isCount = false;
                        bool isTask = false;
                        bool isProcessor = false;


                        while (!line.StartsWith(Keywords.allocations_end))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();



                            if (Regex.Match(line, commentCheck).Success)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (Regex.Match(line, commentLineCheck).Success)
                            {
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                            }

                            else if (Regex.Match(line, emptyCheck).Success)
                            {
                                data += line;
                            }
                            else if (line.StartsWith(Keywords.count))
                            {
                                if (isCount)
                                {
                                    data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END-ALLOCATIONS FOUND" + "</span>" + "</p>";
                                    Errors.Add("END-ALLOCATIONS FOUND");
                                }
                                else
                                {
                                    string[] lineError = ValidateInt(line, Keywords.count);

                                    data += lineError[0];
                                    if (!lineError[1].Equals("empty"))
                                    {
                                        Errors.Add(lineError[1]);
                                    }

                                    isCount = true;
                                }
                             

                            }
                            else if (line.StartsWith(Keywords.tasks))
                            {
                                if (isTask)
                                {
                                    data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END-ALLOCATIONS FOUND" + "</span>" + "</p>";
                                    Errors.Add("END-ALLOCATIONS FOUND");
                                }
                                else
                                {
                                    string[] lineError = ValidateInt(line, Keywords.tasks);

                                    data += lineError[0];
                                    if (!lineError[1].Equals("empty"))
                                    {
                                        Errors.Add(lineError[1]);
                                    }

                                    isTask = true;
                                }

                              

                            }
                            else if (line.StartsWith(Keywords.processors))
                            {


                                if (isProcessor)
                                {
                                    data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END-ALLOCATIONS FOUND" + "</span>" + "</p>";
                                    Errors.Add("END-ALLOCATIONS FOUND");
                                }
                                else
                                {
                                    string[] lineError = ValidateInt(line, Keywords.processors);

                                    data += lineError[0];
                                    if (!lineError[1].Equals("empty"))
                                    {
                                        Errors.Add(lineError[1]);
                                    }

                                    isProcessor = true;
                                }

                               
                            }
                            else if (line.StartsWith(Keywords.allocations_end))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.StartsWith(Keywords.allocation_start))
                            {
                                data += "<p>" + line + "</p>";
                                bool isId = false;
                                bool isMap = false;
                                while (!line.StartsWith(Keywords.allocation_end))
                                {
                                    line = streamReader.ReadLine();
                                    line = line.Trim();

                                    if (Regex.Match(line, commentCheck).Success)
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else if (Regex.Match(line, commentLineCheck).Success)
                                    {
                                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                        Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                                    }

                                    else if (Regex.Match(line, emptyCheck).Success)
                                    {
                                        data += line;
                                    }
                                    else if (line.StartsWith(Keywords.id))
                                    {
                                        if(isId)
                                        {
                                            data += "<p>"+"<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END ALLOCATION FOUND" + "</span>"+"</p>";
                                        }
                                        else
                                        {
                                            string[] lineError = ValidateInt(line, Keywords.id);

                                           
                                            if(!lineError[1].Equals("empty"))
                                            {
                                                Errors.Add(lineError[1]);
                                            }

                                            isId = true;
                                        }
                                       

                                    }
                                    else if (line.StartsWith(Keywords.map))
                                    {
                                       

                                        if (isMap)
                                        {
                                            data += "</p>"+"<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END ALLOCATION FOUND" + "</span>"+"<p>";
                                        }
                                        else
                                        {
                                           
                                                                                                                               
                                            string[] lineError = ValidateMap(line, Keywords.map, "taff");

                                            data += lineError[0];
                                            if (!lineError[1].Equals("empty"))
                                            {
                                                Errors.Add(lineError[1]);
                                            }

                                            isMap = true;
                                            
                                            
                                           
                                        }
                                       

                                    }
                                    else if (line.StartsWith(Keywords.allocation_end))
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else if (line.StartsWith(Keywords.allocation_start))
                                    {
                                        data += "</p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END ALLOCATION FOUND" + "</span>" + "<p>";
                                    }
                                    else
                                    {
                                        Errors.Add("INVALID LINE");
                                        data += "<p>"+"<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO ATTRIBUTE PROVIDED" + "</span>"+"</p>";
                                    }
                                }




                            }
                            else
                            {
                                Errors.Add("INVALID LINE");
                                data += "<p>"+"<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                            }

                        }



                    }
                    else
                    {
                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                        Errors.Add("INVALID LINE");
                    }

                }


                catch (Exception)
                {
                    return data;
                }

            }
            streamReader.Close();

            if (data.Length == 0)
            {
                Errors.Add("THE FILE IS EMPTY");
            }

            
            return data;
        }










        /// <summary>
        /// Method to read the cff file validate it
        /// </summary>
        /// <param name="path"> The address of the cff file</param>
        /// <param name="Errors">List of errors detected in the cff file</param>
        /// <returns>A string which cotains the full cff file texts</returns>



        public string Readcff(String path, out List<String> Errors)
        {
            StreamReader streamReader = new StreamReader(path);
            String data = "";
            Errors = new List<string>();
            String emptyCheck = "^\\s*$";
            string commentCheck = @"^\s*//\s*.*$";
            string commentLineCheck = @"^\s*.*\s*//.*$";

            while (!streamReader.EndOfStream)
            {
                try
                {
                    String line = streamReader.ReadLine();
                    line = line.Trim();


                    if (Regex.Match(line, commentCheck).Success)
                    {
                        data += "<p>" + line + "</p>";
                    }
                    else if (Regex.Match(line, commentLineCheck).Success)
                    {
                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                        Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                    }

                    else if (Regex.Match(line, emptyCheck).Success)
                    {
                        data += line;
                    }

                    else if (line.StartsWith(Keywords.log_start))
                    {
                        data += "<p>" + line + "</p>";
                        bool isDefault = false;
                        
                        while (!line.StartsWith(Keywords.log_end))                           
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();

                          

                            if (Regex.Match(line, commentCheck).Success)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (Regex.Match(line, commentLineCheck).Success)
                            {
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                            }

                            else if (Regex.Match(line, emptyCheck).Success)
                            {
                                data += line;
                            }
                            else if (line.StartsWith(Keywords._default))
                            {
                                if (isDefault)
                                {
                                    data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END-CONFIGURATION STATEMENT FOUND" + "</span>" + "</p>";
                                    Errors.Add("NO END-CONFIGURATION STATEMENT FOUND");
                                }
                                else
                                {
                                    string[] lineError = ValidateString(line, Keywords._default, true) ;
                                    data += lineError[0];
                                    if (!lineError[1].Equals("empty"))
                                    {
                                        Errors.Add(lineError[1]);
                                    }

                                    isDefault = true;
                                }
                             
                            }
                            else if (line.StartsWith(Keywords.log_end))
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

                    else if (line.StartsWith(Keywords.limits))
                    {
                        data += "<p>" + line + "</p>";
                        while (!line.StartsWith(Keywords.limits_end))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();

                            if (Regex.Match(line, commentCheck).Success)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (Regex.Match(line, commentLineCheck).Success)
                            {
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                            }

                            else if (Regex.Match(line, emptyCheck).Success)
                            {
                                data += line;
                            }
                            if (line.StartsWith(Keywords.minimum_tasks))
                            {
                                string[] lineError = ValidateInt(line, Keywords.minimum_tasks);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.maximum_tasks))
                            {
                                string[] lineError = ValidateInt(line, Keywords.maximum_tasks);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.maximum_tasks))
                            {
                                string[] lineError = ValidateInt(line, Keywords.maximum_tasks);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.minimum_processors))
                            {
                                string[] lineError = ValidateInt(line, Keywords.minimum_processors);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.maximum_processors))
                            {
                                string[] lineError = ValidateInt(line, Keywords.maximum_processors);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.minimum_procfrequencies))
                            {
                                string[] lineError = ValidateDouble(line, Keywords.minimum_procfrequencies);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.maximum_procfrequencies))
                            {
                                string[] lineError = ValidateDouble(line, Keywords.maximum_procfrequencies);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.minimum_ram))
                            {
                                string[] lineError = ValidateInt(line, Keywords.minimum_ram);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.maximum_ram))
                            {
                                string[] lineError = ValidateInt(line, Keywords.maximum_ram);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.minimum_download))
                            {
                                string[] lineError = ValidateInt(line, Keywords.minimum_download);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.maximum_download))
                            {
                                string[] lineError = ValidateInt(line, Keywords.maximum_download);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.minimum_upload))
                            {
                                string[] lineError = ValidateInt(line, Keywords.minimum_upload);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.maximum_upload))
                            {
                                string[] lineError = ValidateInt(line, Keywords.maximum_upload);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.limits_end))
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

                    else if (line.StartsWith(Keywords.program))
                    {
                        data += "<p>" + line + "</p>";
                        while (!line.StartsWith(Keywords.program_end))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();

                            if (Regex.Match(line, commentCheck).Success)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (Regex.Match(line, commentLineCheck).Success)
                            {
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                            }

                            else if (Regex.Match(line, emptyCheck).Success)
                            {
                                data += line;
                            }
                            else if (line.StartsWith(Keywords.duration))
                            {
                                string[] lineError = ValidateDouble(line, Keywords.duration);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.tasks))
                            {
                                string[] lineError = ValidateInt(line, Keywords.tasks);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }
                            }
                            else if (line.StartsWith(Keywords.processors))
                            {
                                string[] lineError = ValidateInt(line, Keywords.processors);

                                data += lineError[0];
                                if (!lineError[1].Equals("empty"))
                                {
                                    Errors.Add(lineError[1]);
                                }

                            }
                            else if (line.StartsWith(Keywords.program_end))
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


                    else if (line.StartsWith(Keywords.tasks))
                    {
                        data += "<p>" + line + "</p>";

                        while (!line.StartsWith(Keywords.tasks_end))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();

                            if (Regex.Match(line, commentCheck).Success)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (Regex.Match(line, commentLineCheck).Success)
                            {
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                            }

                            else if (Regex.Match(line, emptyCheck).Success)
                            {
                                data += line;
                            }
                            else if (line.StartsWith(Keywords.task))
                            {
                                data += "<p>" + line + "</p>";

                                while (!line.StartsWith(Keywords.task_end))
                                {
                                    line = streamReader.ReadLine();
                                    line = line.Trim();


                                    if (Regex.Match(line, commentCheck).Success)
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else if (Regex.Match(line, commentLineCheck).Success)
                                    {
                                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                        Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                                    }

                                    else if (Regex.Match(line, emptyCheck).Success)
                                    {
                                        data += line;
                                    }
                                    else if (line.StartsWith(Keywords.id))
                                    {
                                        string[] lineError = ValidateInt(line, Keywords.id);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.runtime))
                                    {
                                        string[] lineError = ValidateDouble(line, Keywords.runtime);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.reference_frequency))
                                    {
                                        string[] lineError = ValidateDouble(line, Keywords.reference_frequency);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.ram))
                                    {
                                        string[] lineError = ValidateDouble(line, Keywords.ram);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.download))
                                    {
                                        string[] lineError = ValidateInt(line, Keywords.download);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.upload))
                                    {
                                        string[] lineError = ValidateInt(line, Keywords.upload);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.task_end))
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
                            else if (line.StartsWith(Keywords.tasks_end))
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



                    else if (line.StartsWith(Keywords.processors))
                    {
                        data += "<p>" + line + "</p>";

                        while (!line.StartsWith(Keywords.processors_end))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();


                            if (Regex.Match(line, commentCheck).Success)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (Regex.Match(line, commentLineCheck).Success)
                            {
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                            }

                            else if (Regex.Match(line, emptyCheck).Success)
                            {
                                data += line;
                            }
                            else if (line.StartsWith(Keywords.processor))
                            {
                                data += "<p>" + line + "</p>";

                                while (!line.StartsWith(Keywords.processor_end))
                                {
                                    line = streamReader.ReadLine();
                                    line = line.Trim();


                                    if (Regex.Match(line, commentCheck).Success)
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else if (Regex.Match(line, commentLineCheck).Success)
                                    {
                                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                        Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                                    }

                                    else if (Regex.Match(line, emptyCheck).Success)
                                    {
                                        data += line;
                                    }
                                    else if (line.StartsWith(Keywords.id))
                                    {
                                        string[] lineError = ValidateInt(line, Keywords.id);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.type))
                                    {
                                        string[] lineError = ValidateString(line, Keywords.type, false);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.frequency))
                                    {
                                        string[] lineError = ValidateDouble(line, Keywords.frequency);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.ram))
                                    {
                                        string[] lineError = ValidateInt(line, Keywords.ram);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.download))
                                    {
                                        string[] lineError = ValidateInt(line, Keywords.download);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.upload))
                                    {
                                        string[] lineError = ValidateInt(line, Keywords.upload);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.processor_end))
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
                            else if (line.StartsWith(Keywords.processors_end))
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



                    else if (line.StartsWith(Keywords.processor_types))
                    {

                        data += "<p>" + line + "</p>";

                        while (!line.StartsWith(Keywords.processortypes_end))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();



                            if (Regex.Match(line, commentCheck).Success)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (Regex.Match(line, commentLineCheck).Success)
                            {
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                            }

                            else if (Regex.Match(line, emptyCheck).Success)
                            {
                                data += line;
                            }
                            else if (line.StartsWith(Keywords.processortypes_end))
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (line.StartsWith(Keywords.processor_type))
                            {
                                data += "<p>" + line + "</p>";
                              

                                while (!line.StartsWith(Keywords.processortype_end))
                                {
                                    line = streamReader.ReadLine();
                                    line = line.Trim();

                                    if (Regex.Match(line, commentCheck).Success)
                                    {
                                        data += "<p>" + line + "</p>";
                                    }
                                    else if (Regex.Match(line, commentLineCheck).Success)
                                    {
                                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                        Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                                    }
                                    else if (line.StartsWith(Keywords.name))
                                    {
                                        string[] lineError = ValidateString(line, Keywords.name, false);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }


                                    }
                                    else if (line.StartsWith(Keywords.c2))
                                    {

                                        string[] lineError = ValidateDouble(line, Keywords.c2);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }

                                    }
                                    else if (line.StartsWith(Keywords.c1))
                                    {
                                        string[] lineError = ValidateDouble(line, Keywords.c1);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.c0))
                                    {
                                        string[] lineError = ValidateDouble(line, Keywords.c0);

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }
                                    }
                                    else if (line.StartsWith(Keywords.processortype_end))
                                    {
                                        data += "</p>"+ line +  "<p>";
                                    }
                                    else
                                    {
                                        Errors.Add("Invalid line");
                                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO ATTRIBUTE PROVIDED" + "</span>" + "</p>";
                                    }
                                }
                            }

                        }
                    }



                    else if (line.StartsWith(Keywords.localCommunication))
                    {
                        data += "<p>" + line + "</p>";

                        bool isMap = false;

                        while (!line.StartsWith(Keywords.localCommunication_end))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();

                            if (Regex.Match(line, commentCheck).Success)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (Regex.Match(line, commentLineCheck).Success)
                            {
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                            }

                            else if (Regex.Match(line, emptyCheck).Success)
                            {
                                data += line;
                            }
                            else if (line.StartsWith(Keywords.map))
                            {


                                if (isMap)
                                {
                                    data += "</p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END-LOCAL-COMMUNICATION FOUND" + "</span>" + "<p>";
                                }
                                else
                                {
                                    
                                        String orginalLine = line;
                                  

                                    
                                    
                                        string[] lineError = ValidateMap(line, Keywords.map, "cff");

                                        data += lineError[0];
                                        
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }

                                        isMap = true;
                                    


                                }


                            }
                            else if (line.StartsWith(Keywords.localCommunication_end))
                            {
                                data += "<p>" + line + "</p>";
                            }

                            else
                            {
                                Errors.Add("Invalid line");
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END-LOCAL-COMMUNICATION FOUND" + "</span>" + "</p>";
                            }
                        }



                    }

                    else if (line.StartsWith(Keywords.remoteCommunication))
                    {
                        data += "<p>" + line + "</p>";

                        bool isMap = false;
                        while (!line.StartsWith(Keywords.remoteCommunication_end))
                        {
                            line = streamReader.ReadLine();
                            line = line.Trim();


                            if (Regex.Match(line, commentCheck).Success)
                            {
                                data += "<p>" + line + "</p>";
                            }
                            else if (Regex.Match(line, commentLineCheck).Success)
                            {
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : COMMENT AND STATEMENT COMBMINED" + "</span>" + "</p>";
                                Errors.Add("COMBINIG STATEMENT AND COMMENT IS NOT ALLOWED");
                            }

                            else if (Regex.Match(line, emptyCheck).Success)
                            {
                                data += line;
                            }
                            else if (line.StartsWith(Keywords.map))
                            {


                                if (isMap)
                                {
                                    data += "</p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END-REMOTE-COMMUNICATION FOUND" + "</span>" + "<p>";
                                }
                                else
                                {
                                    

                                        string[] lineError = ValidateMap(line, Keywords.map, "cff");

                                        data += lineError[0];
                                        if (!lineError[1].Equals("empty"))
                                        {
                                            Errors.Add(lineError[1]);
                                        }

                                        isMap = true;
                                    


                                }


                            }
                            else if (line.StartsWith(Keywords.remoteCommunication_end))
                            {
                                data += "<p>" + line + "</p>";
                            }

                            else
                            {
                                Errors.Add("INVALID LINE");
                                data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : NO END-REMOTE-COMMUNICATION FOUND" + "</span>" + "</p>";
                            }
                        }
                    }
                    else
                    {
                        data += "<p>" + "<span style='color:red'>" + line + "&nbsp&nbsp&nbsp" + "ERROR : INVALID LINE PROVIDED" + "</span>" + "</p>";
                        Errors.Add("INVALID LINE");
                    }
                }


                
                catch (Exception)
                {
                    return data;
                }

            }
            streamReader.Close();

            if (data.Length == 0)
            {
                Errors.Add("THE FILE IS EMPTY");
            }

            return data;
        }

        /// <summary>
        /// Method to validate if a value of a section is valid and integer
        /// </summary>
        /// <param name="line">The string needs to be checked </param>
        /// <param name="section">The type of section to which we need to match the line</param>
        /// <returns></returns>


        public string[] ValidateInt(String line, string section)
        {
            string[] dataError = new string[2];
            string error = "";
            int count = 0;
            string orginalLine = line;
            string zeroCheck = @"^.*=\s*0+$";
            string commentLineCheck = @"^\s*.*\s*//.*$";
            string[] texts = line.Split('=');

            if (Regex.Match(orginalLine, commentLineCheck).Success)
            {
                line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : MIXING STATEMENT AND COMMENT IS NOT ALLOWED" + "</span>" + "</p>";
                error = "COMMENT AND STATEMENT IS MIXED IN: " + section;
                dataError[0] = line;
                dataError[1] = error;


            }
            else if(Regex.Match(orginalLine, zeroCheck).Success && !orginalLine.StartsWith(Keywords.id))
            {
                line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : VALUE CANNOT BE 0" + "</span>" + "</p>";
                error = "VALUE IS 0 IN: " + section;
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
                    dataError[1] = section + " IS NOT AN INTEGER";



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



        /// <summary>
        /// Method to validate if a value of a section is valid and double
        /// </summary>
        /// <param name="line">The string needs to be checked </param>
        /// <param name="section">The type of section to which we need to match the line</param>
        /// <returns></returns>


        public string[] ValidateDouble(String line, string section)
        {
            string[] dataError = new string[2];
            string error = "";
            double count;
            string orginalLine = line;
            string zeroCheck = @"^.*=\s*0+$";
            string commentLineCheck = @"^\s*.*\s*//.*$";
            string[] texts = line.Split('=');

            if (Regex.Match(orginalLine, commentLineCheck).Success)
            {
                line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : MIXING STATEMENT AND COMMENT IS NOT ALLOWED" + "</span>" + "</p>";
                error = "COMMENT AND STATEMENT IS MIXED IN: " + section;
                dataError[0] = line;
                dataError[1] = error;


            }
            else if (Regex.Match(orginalLine, zeroCheck).Success)
            {
                line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : VALUE CANNOT BE 0" + "</span>" + "</p>";
                error = "VALUE IS 0 IN: " + section;
                dataError[0] = line;
                dataError[1] = error;
            }
            else if (texts.Length.Equals(2))
            {
                if (!texts[0].Trim().Equals(section))
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + " ERROR : INVALID" + "ATTRIBUTE " + " NAME " + section + "</span>" + "</p>";
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
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + " ERROR : " + section + " SHOULD BE AN NUMBER" + "</span>" + "</p>";

                    dataError[0] = line;
                    dataError[1] = section + " IS NOT AN INTEGER";



                }

            }
            else
            {
                line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR :  MISSING ASSIGNMENT SYMBOL" + "</span>" + "</p>";
                dataError[0] = line;
                dataError[1] = "NO ASSIGMENT SYMBOL FOUND OF " + section;


            }

            return dataError;
        }


        /// <summary>
        /// Method to validate if a value of a section is valid and string
        /// </summary>
        /// <param name="line">The string needs to be checked </param>
        /// <param name="section">The type of section to which we need to match the line</param>
        /// <returns></returns>



        public string[] ValidateString(String line, string section, bool isFile)
        {
            string[] dataError = new string[2];
            string error = "";
            string orginalLine = line;
            string commentLineCheck = @"^\s*.*\s*//.*$";
            string[] texts = line.Split('=');
            string pattern = @"^" + '"' +".+" +'"' + "$";


            if (Regex.Match(orginalLine, commentLineCheck).Success)
            {
                line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : MIXING STATEMENT AND COMMENT IS NOT ALLOWED" + "</span>" + "</p>";
                error = "COMMENT AND STATEMENT IS MIXED IN: " + section;
                dataError[0] = line;
                dataError[1] = error;


            }
            else if (texts.Length.Equals(2))
            {
                if (!texts[0].Trim().Equals(section))
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR : INVALID ATTRIBUTE NAME" + section + "</span>" + "</p>";
                    

                    dataError[0] = line;
                    dataError[1] = "INAVLID ATTRIBUTE NAME: " + section;

                }
                else if (Regex.Match(texts[1],pattern).Success)
                {
                    if (isFile)
                    {
                        if (texts[1].Trim('"').IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                        {
                            line = "<p>" + orginalLine + "</p>";
                            dataError[0] = line;
                            dataError[1] = "empty";

                        }
                        else
                        {
                            line = "<p>" + orginalLine + "</p>";
                            dataError[0] = line;
                            dataError[1] = "INVALID FILE NAME";

                        }


                    }
                    else
                    {
                        line = "<p>" + orginalLine + "</p>";
                        dataError[0] = line;
                        dataError[1] = "empty";

                    }

                }
                else
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + "ERROR :  IS NOT A STRING" + "</span>" + "</p>";
                    dataError[0] = line;
                    dataError[1] = "NOT A STRING" + section;


                }

                
            }
            else
            {
                line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + " ERROR :  MISSING ASSIGNMENT SYMBOL" + "</span>" + "</p>";
                dataError[0] = line;
                dataError[1] = "NO ASSIGMENT SYMBOL FOUND OF" + section;
            }
            
            return dataError;
        }

        /// <summary>
        /// Method to validate if a value of a section is valid and MAP
        /// </summary>
        /// <param name="line">The string needs to be checked </param>
        /// <param name="section">The type of section to which we need to match the line</param>
        /// <returns></returns>


        public string[] ValidateMap(String line, string section, string file)
        {
            string[] dataError = new string[2];
            string error = "";
            string pattern;
            string orginalLine = line;
            string commentLineCheck = @"^\s*.*\s*//.*$";
            string[] texts = line.Split('=');
               
            if (file == "taff")
            {
                pattern = "^\\s*MAP\\s*=(0|1)(,0|,1)*(;(0|1)(,0|,1)*)*$";
                
            }
            else 
            {
                pattern = @"^\s*MAP\s*=\s*(\d|\d.\d*)(,\d|,\d.\d*)*(;(\d|\d.\d*)(,\d|,\d.\d*)*)*$";

            }




            Match m = Regex.Match(orginalLine, pattern);



            if (Regex.Match(orginalLine, commentLineCheck).Success)
            {
                line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + " ERROR : MIXING STATEMENT AND COMMENT IS NOT ALLOWED" + "</span>" + "</p>";
                error = "COMMENT AND STATEMENT IS MIXED IN: " + section;
                dataError[0] = line;
                dataError[1] = error;


            }
            else if (texts.Length.Equals(2))
            {
                if (!texts[0].Trim().Equals(section))
                {
                    line = "<p>" + "<span style='color:red'>" + orginalLine + "&nbsp&nbsp&nbsp" + " ERROR : INVALID" + "ATTRIBUTE" + "NAME" + section + "</span>" + "</p>";
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
