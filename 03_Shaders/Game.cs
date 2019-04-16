using System;
using System.Drawing;
using Common;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace _03_Shaders
{
    internal class Game : GameWindow
    {
        private Shader _Shader;

        private int _VAO; //顶点数组对象：Vertex Array Object，VAO
        private int _VBO; //顶点缓冲对象：Vertex Buffer Object，VBO
        private int _EBO; //索引缓冲对象：Element Buffer Object，EBO或Index Buffer Object，IBO

        private Vector3[] _VertData; //顶点数据
        private int[] _IndiceData; //索引数据

        public Game() : base(600, 600, GraphicsMode.Default, "", GameWindowFlags.Default, DisplayDevice.Default, 4, 0,
            GraphicsContextFlags.ForwardCompatible)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Hello Triangle";

            _Shader = new Shader(@"../../Shader/vertex.glsl", @"../../Shader/fragment.glsl");

            //传递给Shader的顶点数据
            _VertData = new[]
            {
                //颜色
                new Vector3(0.5f, -0.5f, 0.0f),
                new Vector3(0.0f,  0.5f, 0.0f),
                new Vector3(-0.5f, -0.5f, 0.0f),
                //位置
                new Vector3(1.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
                new Vector3(0.0f, 0.0f, 1.0f) 
            };

            /* 两种方式都可以 方便理解 VertexAttribPointer 参数的含义
            _VertData = new[]
            {
                //颜色 & 位置 混合
                new Vector3(0.5f, -0.5f, 0.0f),
                new Vector3(1.0f, 0.0f, 0.0f),

                new Vector3(0.0f,  0.5f, 0.0f),
                new Vector3(0.0f, 1.0f, 0.0f),

                new Vector3(-0.5f, -0.5f, 0.0f),
                new Vector3(0.0f, 0.0f, 1.0f)
            };
            */

            //创建VAO
            _VAO = GL.GenVertexArray();
            GL.BindVertexArray(_VAO); //绑定VAO - 表示下面的操作(VBO)都和此VAO相关 直到解绑

            //创建VBO
            _VBO = GL.GenBuffer();
            //绑定 表示当前对类型(BufferTarget.ArrayBuffer)的操作 都是针对此VBO的
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            //绑定用户数据到GPU
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_VertData.Length * Vector3.SizeInBytes), _VertData,
                BufferUsageHint.StaticDraw);
            //告知GL如何解析数据. 第一个参数0 对应的VS中的 (location = 0)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 9 * sizeof(float));

            //使用第二种_VertData数据定义方式
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            //启用定点属性
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            //解绑VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //解绑VAO 传递0表示解绑当前的VAO
            GL.BindVertexArray(0);

            GL.ClearColor(Color.CornflowerBlue);

            //线框模式
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
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
            _Shader.Use();

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
