using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_Camera
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var G = new Game())
            {
                G.Run(30, 30);
            }
        }
    }
}
