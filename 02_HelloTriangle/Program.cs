using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace HelloTriangle
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

    class Game : GameWindow
    {
        //顶点着色器
        private string vShader = @"#version 330 core
                                    layout (location = 0) in vec3 aPos;

                                    void main()
                                    {
                                        gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
                                    }";

        //片段着色器
        private string fShader = @"#version 330 core
                                    out vec4 FragColor;

                                    void main()
                                    {
                                        FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
                                    }";

        private int pID; //着色器程序id
        private int vsID; //顶点着色器id
        private int fsID; //片段着色器id
        Vector3[] vertdata;
        int VBO, VAO;

        public Game() : base(600, 600, GraphicsMode.Default, "", GameWindowFlags.Default, DisplayDevice.Default, 4, 0,
            GraphicsContextFlags.ForwardCompatible)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Hello Triangle";

            pID = GL.CreateProgram();
            //创建定点着色器, 并编译
            vsID = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vsID, vShader);
            GL.CompileShader(vsID);
            //创建片段着色器, 并编译
            fsID = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fsID, fShader);
            GL.CompileShader(fsID);

            //把着色器指定给着色器程序
            GL.AttachShader(pID, vsID);
            GL.AttachShader(pID, fsID);
            //链接着色器程序
            GL.LinkProgram(pID);
            GL.DeleteShader(vsID);
            GL.DeleteShader(fsID);
            Console.WriteLine(GL.GetProgramInfoLog(pID));

            vertdata = new Vector3[]
            {
                new Vector3(-0.8f, -0.8f, 0f),
                new Vector3(0.8f, -0.8f, 0f),
                new Vector3(0f, 0.8f, 0f)
            };

            
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata,
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.BindVertexArray(VAO);
            GL.UseProgram(pID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3); 

            GL.Flush();
            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            ProcessInput();
        }

        private void ProcessInput()
        {
            //使用esc键作为退出窗口快捷键, 防止在鼠标不可用的时候无法退出
            if (Keyboard.GetState().IsKeyDown(Key.Escape))
            {
                Exit();
            }
        }
    }
}
