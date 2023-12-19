using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public static class FileManager
    {
        public static void SaveObject(object obj, string path)
        {

            //var path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "TestClass.json";
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            using StreamWriter streamWriter = new StreamWriter(path);
            streamWriter.Write(json);
            streamWriter.Close();
        }
        public static T LoadObject<T>(string data)
        {
            List<string> errors = new List<string>();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                {
                    errors.Add(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                },
                Converters = { new IsoDateTimeConverter() }
            };
            var result = JsonConvert.DeserializeObject<T>(data, settings);
            foreach (var er in errors)
            {
                Debug.LogError(er);
            }
            return result;
        }
    }
}