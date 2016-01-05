using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace temp_network.Scripts
{
    class temp_json_test
    {
        public string name;
        public string test;
    }

    class JParser
    {
        public string init()
        {
            temp_json_test temp = new temp_json_test();
            temp.name = "json_test";
            temp.test = "hi";

            string json = JsonConvert.SerializeObject(temp);

            return json;
        }

        public void deserialize(string j)
        {
            temp_json_test m = JsonConvert.DeserializeObject<temp_json_test>(j);

            Console.WriteLine(m.name);
        }
    }
}
