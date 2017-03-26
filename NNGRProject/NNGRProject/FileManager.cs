using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace NNGRProject
{
    public class FileManager
    {
        private static string savingPath;

        static FileManager()
        {
            string workingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            savingPath = Path.Combine(workingPath, "scores.xml");
        }

        public static T Load<T>()
        {
            if (!File.Exists(savingPath))
            {
                return default(T);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream stream = File.Open(savingPath, FileMode.Open, FileAccess.Read))
            {
                return (T)serializer.Deserialize(stream);
            }
        }


        public static void Save(object saveData)
        {
            XmlSerializer serializer = new XmlSerializer(saveData.GetType());
            using (FileStream stream = File.Open(savingPath, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(stream, saveData);
            }

        }
    }
}