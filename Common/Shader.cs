using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Common
{
    public class Shader
    {
        private int PID; //着色器程序id

        //构造器读取并构建着色器
        public Shader(string pVertexPath, string pFragmentPath)
        {
            PID = GL.CreateProgram();

            string _V = File.ReadAllText(pVertexPath);
            string _F = File.ReadAllText(pFragmentPath);

            int _VID = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_VID, _V);
            GL.CompileShader(_VID);
            GL.AttachShader(PID, _VID);
            Console.WriteLine(GL.GetShaderInfoLog(_VID));

            int _FID = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_FID, _F);
            GL.CompileShader(_FID);
            GL.AttachShader(PID, _FID);
            Console.WriteLine(GL.GetShaderInfoLog(_FID));

            GL.LinkProgram(PID);
            Console.WriteLine(GL.GetProgramInfoLog(PID));

            GL.DeleteShader(_VID);
            GL.DeleteShader(_FID);
        }

        //使用/激活程序
        public void Use()
        {
            GL.UseProgram(PID);
        }

        //uniform工具函数
        public void SetBool(string name, bool value)
        {
            GL.Uniform1(GL.GetUniformLocation(PID, name), value ? 1 : 0);
        }

        public void SetInt(string name, int value)
        {
            GL.Uniform1(GL.GetUniformLocation(PID, name), value);
        }

        public void SetFloat(string name, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(PID, name), value);
        }
    }
}
