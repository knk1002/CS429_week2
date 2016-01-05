using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace temp_network.Scripts
{
    class LoginAccept
    {
        static string path = @"C:\8CardMember\ID.txt";

        public string[] ReadID()
        {
            string[] textvalue = System.IO.File.ReadAllLines(path);
            return textvalue;
        }

        public void WriteID(string ID)
        {
            string textvalue = "\r\n" + ID;
            System.IO.File.AppendAllText(path, textvalue);
        }
    }
}
