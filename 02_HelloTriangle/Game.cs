using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace HelloTriangle
{
    internal class Game : GameWindow
    {
        //顶点着色器
        private readonly string _VertexShader = @"#version 330 core
                                    layout (location = 0) in vec3 aPos;

                                    void main()
                                    {
                                        gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
                                    }";

        //片段着色器
        private readonly string _FragmentShader = @"#version 330 core
                                    out vec4 FragColor;

                                    void main()
                                    {
                                        FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
                                    }";

        private int _VsId; //顶点着色器id
        private int _FsId; //片段着色器id
        private int _PId; //着色器程序id

        private int _VAO; //顶点数组对象：Vertex Array Object，VAO
        private int _VBO; //顶点缓冲对象：Vertex Buffer Object，VBO

        private Vector3[] _VertData; //顶点数据

        public Game() : base(600, 600, GraphicsMode.Default, "", GameWindowFlags.Default, DisplayDevice.Default, 4, 0,
            GraphicsContextFlags.ForwardCompatible)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Hello Triangle";

            _PId = GL.CreateProgram();
            //创建定点着色器, 并编译
            _VsId = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_VsId, _VertexShader);
            GL.CompileShader(_VsId);
            //创建片段着色器, 并编译
            _FsId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_FsId, _FragmentShader);
            GL.CompileShader(_FsId);

            //把着色器指定给着色器程序
            GL.AttachShader(_PId, _VsId);
            GL.AttachShader(_PId, _FsId);
            //链接着色器程序
            GL.LinkProgram(_PId);
            GL.DeleteShader(_VsId);
            GL.DeleteShader(_FsId);
            Console.WriteLine(GL.GetProgramInfoLog(_PId));

            //传递给Shader的顶点数据
            _VertData = new[]
            {
                new Vector3(-0.8f, -0.8f, 0f),
                new Vector3(0.8f, -0.8f, 0f),
                new Vector3(0f, 0.8f, 0f)
            };

            //创建VAO
            _VAO = GL.GenVertexArray();
            GL.BindVertexArray(_VAO); //绑定VAO - 表示下面的操作(VBO)都和此VAO相关 直到解绑
            //创建VBO
            _VBO = GL.GenBuffer();
            //绑定 表示当前对类型(BufferTarget.ArrayBuffer)的操作 都是针对此VBO的
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            //绑定用户数据到GPU
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (_VertData.Length * Vector3.SizeInBytes), _VertData,
                BufferUsageHint.StaticDraw);
            //告知GL如何解析数据. 第一个参数0 对应的VS中的 (location = 0)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //启用定点属性
            GL.EnableVertexAttribArray(0);

            //解绑VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //解绑VAO 传递0表示解绑当前的VAO
            GL.BindVertexArray(0);

            GL.ClearColor(Color.CornflowerBlue);
            //GL.PointSize(5f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            //指定VAO
            GL.BindVertexArray(_VAO); 
            //指定Shader程序
            GL.UseProgram(_PId);
            //绘制图像
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
            if (Keyboard.GetState().IsKeyDown(Key.Escape)) Exit();
        }
    }
}