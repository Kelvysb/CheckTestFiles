using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using Newtonsoft.Json;

namespace CheckTestFiles.Model
{
    class TestFileModelPatern
    {
        public string MoldelFileName { get; set; }

        public string ModelFile { get; set; }

        public string PythonScript { get; set; }

        public List<Parameter> Parameters { get; set; }

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

        public static TestFileModelPatern deserialize(string p_Input)
        {
            try
            {
                return JsonConvert.DeserializeObject<TestFileModelPatern>(p_Input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static TestFileModelPatern load(string p_Path)
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

    class Parameter
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public string Expression { get; set; }
    }
}
