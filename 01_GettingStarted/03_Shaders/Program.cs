using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Shaders
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建对象
            using (var game = new Game())
            {
                //运行
                game.Run(30, 30); //指定帧率
            }
        }
    }
}
