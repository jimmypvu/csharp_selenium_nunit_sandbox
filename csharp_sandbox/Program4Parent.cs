using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpSandbox
{
    internal class Program4Parent
    {
        string name = "shyguy4";

        public void setData()
        {
            Console.WriteLine($"i am {name}, printed from the setData() method in parent Program4");
        }
    }
}
