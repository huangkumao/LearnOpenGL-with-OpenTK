using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GlmNet;
using OpenTK;

namespace Common
{
    public static class DebugHelper
    {
        public static void Dump(this Matrix4 m4)
        {
            Console.WriteLine("========================================");
            Console.WriteLine($"{m4.M11,-10:#0.0#}  {m4.M12,-10:#0.0#}  {m4.M13,-10:#0.0#}  {m4.M14,-10:#0.0#}");
            Console.WriteLine($"{m4.M21,-10:#0.0#}  {m4.M22,-10:#0.0#}  {m4.M23,-10:#0.0#}  {m4.M24,-10:#0.0#}");
            Console.WriteLine($"{m4.M31,-10:#0.0#}  {m4.M32,-10:#0.0#}  {m4.M33,-10:#0.0#}  {m4.M34,-10:#0.0#}");
            Console.WriteLine($"{m4.M41,-10:#0.0#}  {m4.M42,-10:#0.0#}  {m4.M43,-10:#0.0#}  {m4.M44,-10:#0.0#}");
            Console.WriteLine("========================================");
        }

        public static void Dump(this mat4 m4)
        {
            Console.WriteLine("========================================");
            Console.WriteLine($"{m4[0][0],-10:#0.0#}  {m4[0][1],-10:#0.0#}  {m4[0][2],-10:#0.0#}  {m4[0][3],-10:#0.0#}");
            Console.WriteLine($"{m4[1][0],-10:#0.0#}  {m4[1][1],-10:#0.0#}  {m4[1][2],-10:#0.0#}  {m4[1][3],-10:#0.0#}");
            Console.WriteLine($"{m4[2][0],-10:#0.0#}  {m4[2][1],-10:#0.0#}  {m4[2][2],-10:#0.0#}  {m4[2][3],-10:#0.0#}");
            Console.WriteLine($"{m4[3][0],-10:#0.0#}  {m4[3][1],-10:#0.0#}  {m4[3][2],-10:#0.0#}  {m4[3][3],-10:#0.0#}");
            Console.WriteLine("========================================");
        }
    }
}
