using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CheckTestFiles.Model
{
    class Config
    {

        public Config()
        {
            Name = "";
            ConfigKeys = new List<ConfigKey>();
        }

        public string Name { get; set; }
        public List<ConfigKey> ConfigKeys { get; set; }

        public string serialize()
        {
            try
            {
                return JsonConvert.SerializeObject(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void save(string p_Path)
        {
            StreamWriter file;
            string input;

            try
            {
                input = serialize();
                file = new StreamWriter(p_Path, false);
                file.Write(input);
                file.Close();
                file.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static Config deserialize(string p_Input)
        {
            try
            {
                return JsonConvert.DeserializeObject<Config>(p_Input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static Config load(string p_Path)
        {
            StreamReader file;
            string input;

            try
            {
                file = new StreamReader(p_Path);
                input = file.ReadToEnd();
                file.Close();
                file.Dispose();
                return deserialize(input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    class ConfigKey
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
