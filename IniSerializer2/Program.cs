using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniSerializer2
{
    class Program
    {
        static void Main(string[] args)
        {   
            //Input parameteres for Properties
            DatabaseOptions databaseOptions = new DatabaseOptions
            {
                Name = "John Doe",
                Organization = "Acme Widgets Inc.",
                Server = "192.0.2.62",
                Port = "143",
                File = "\"payroll.dat\""
            };
            
            // Launch Serialization
            IniSerializer iniSerializer = new IniSerializer();
            string content = iniSerializer.Serialize(databaseOptions);

            // Write Serialization result to the file
            try
            {
                Console.WriteLine(content);
                File.WriteAllText("DatabaseOptions.ini", content, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }



            // Launch Deserialization
            try
            {
                FileInfo fileLocation = 
                    new FileInfo(@"C:\Users\vitalyki\Documents\Visual Studio 2012\Projects\IniSerializer2\IniSerializer2\bin\Debug\DatabaseOptions2.ini");

                DatabaseOptions deserializedFromFile = iniSerializer.Deserialize<DatabaseOptions>(fileLocation);
            }
            catch (Exception e)
            {
                Console.WriteLine("File doesn't exist. Deserialization couldn't be done");
            }

        }
    }
}
