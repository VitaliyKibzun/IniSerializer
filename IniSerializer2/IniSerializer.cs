using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace IniSerializer2
{
    public class IniSerializer
    {
        //public void Serialize(string filePathAndName, object objectToSerialize)
        //{
            
        //}

        public string Serialize(object objectToSerialize)
        {
            Type type = objectToSerialize.GetType();
            PropertyInfo[] listOfProperties = type.GetProperties();
            Dictionary<string, List<KeyValuePair<string, object>>> dataCollection = 
                new Dictionary<string, List<KeyValuePair<string, object>>>();

            foreach (var property in listOfProperties)
            {
                IniSectionAttribute sectionAttribute = 
                    (IniSectionAttribute) property.GetCustomAttribute(typeof (IniSectionAttribute));
                if (sectionAttribute == null)
                    continue;

                IniKeyAttribute keyAttribute = property.GetCustomAttribute<IniKeyAttribute>();
                if(keyAttribute == null)
                    continue;
                
                object propertyValue = property.GetGetMethod().Invoke(objectToSerialize, new object[0]);

                KeyValuePair<string, object> parameterAndName =
                        new KeyValuePair<string, object>(keyAttribute.ElementName, propertyValue);

                if (!dataCollection.ContainsKey(sectionAttribute.ElementName))
                {
                    List<KeyValuePair<string, object>> listOfParameters = new List<KeyValuePair<string, object>>();
                    listOfParameters.Add(parameterAndName);
                    dataCollection.Add(sectionAttribute.ElementName, listOfParameters);
                }
                else
                {
                    List<KeyValuePair<string, object>> listOfParameters =
                        dataCollection[sectionAttribute.ElementName];
                    listOfParameters.Add(parameterAndName);
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (string dictionaryKey in dataCollection.Keys)
            {
                stringBuilder.AppendFormat("[{0}]", dictionaryKey).AppendLine();
                foreach (KeyValuePair<string, object> parameterAndName in dataCollection[dictionaryKey])
                {
                    stringBuilder.AppendFormat("{0}={1}", parameterAndName.Key, parameterAndName.Value);
                    stringBuilder.AppendLine();
                }
            }

            return stringBuilder.ToString();
        }


        public T Deserialize<T>(FileInfo fileInfo) where T : new()
        {
            if (!fileInfo.Exists) throw new FileNotFoundException();
            var fileContent = File.ReadAllLines(fileInfo.FullName);
            string allFileContent = String.Join(Environment.NewLine, fileContent);
            return Deserialize<T>(allFileContent);
        }


        public T Deserialize<T>(string iniContent) where T : new()
        {
            T deserializationResult = new T();

            string section = null;
            string dictionaryKey = null;
            string dictionaryValue = null;
            string secondPartOfDictionaryKey = null;
            Dictionary<string, string> collectedDataFromFile = new Dictionary<string, string>();
            foreach (var line in iniContent.Split(new string[]{Environment.NewLine}, 
                StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    section = line;
                    continue;
                }

                if (!line.StartsWith("[") && line.Contains("=") && (line.IndexOf("=")>0))
                {
                    int delimiterLocation = line.IndexOf("=");
                    secondPartOfDictionaryKey = line.Substring(0, delimiterLocation);
                    dictionaryValue = line.Substring(delimiterLocation + 1);
                }

                dictionaryKey = section + secondPartOfDictionaryKey;
                collectedDataFromFile.Add(dictionaryKey,dictionaryValue);
            }

            Type typeT = typeof (T);

            PropertyInfo[] properties = typeT.GetProperties();
            foreach (var property in properties)
            {
                IniSectionAttribute iniSectionAttribute = property.GetCustomAttribute<IniSectionAttribute>();
                if(iniSectionAttribute == null)
                    continue;
                
                IniKeyAttribute iniKeyAttribute = property.GetCustomAttribute<IniKeyAttribute>();
                if(iniKeyAttribute == null)
                    continue;

                string elementKey = String.Format("[{0}]{1}",
                    iniSectionAttribute.ElementName, iniKeyAttribute.ElementName);
                
                if (collectedDataFromFile.ContainsKey(elementKey))
                {
                    string elementValue = collectedDataFromFile[elementKey];
                    property.GetSetMethod().Invoke(deserializationResult, new object[]{elementValue});
                    collectedDataFromFile.Remove(elementKey);
                }
            }

            try
            {
                if (collectedDataFromFile != null) ;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} properties were not defined in the Class", collectedDataFromFile.Count);
            }
   
            return deserializationResult;
        }
    }
}