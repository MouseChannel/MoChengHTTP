using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System;
namespace MoChengHttp
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    public class JsonUtil
    {
        /// <summary>
        /// [JsonIgnore] attr to ignore some field
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(string text)
        {
            try
            {
                T res = JsonConvert.DeserializeObject<T>(text);
                return res;

            }
            catch (Exception e)
            {
                return default(T);
            }



        }
        /// <summary>
        /// convert to string
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string toString<T>(T obj)
        {

            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        /// <summary>
        /// will create jsonpath auto
        /// </summary>
        /// <param name="c"></param>
        /// <param name="jsonPath"></param>
        /// <typeparam name="T"></typeparam>
        public static void SerializeToDisk<T>(T c, string jsonPath)
        {


            if (!File.Exists(jsonPath))
            {
                string dirPath = Path.GetDirectoryName(jsonPath);
                if (!File.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            }

            try
            {
                File.WriteAllText(jsonPath, toString(c));

            }
            catch (Exception e)
            {
                throw new Exception($"save json to disk {jsonPath} failed: " + e.ToString());
            }



        }
        public static string ReadFromDisk(string jsonPath)
        {
            try
            {
                return File.ReadAllText(jsonPath);
            }
            catch (Exception e)
            {
                throw new Exception($"Read json from disk {jsonPath} failed : " + e.ToString());
            }


        }



    }
}