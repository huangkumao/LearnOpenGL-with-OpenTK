
using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace HelloWindow
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建对象
            using (var game = new Game())
            {
                //运行
                game.Run(60); //指定帧率
            }
        }
    }

    class Game : GameWindow
    {
        public Game() : base(900, //窗口大小 宽 * 高
                             600, 
                             GraphicsMode.Default, 
                             "Hello Game Window", //标题
                             GameWindowFlags.Default, 
                             DisplayDevice.Default, 
                             4, 0, //OpenGL 版本
                             GraphicsContextFlags.Default)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Hello Game Window"; //也可以这样设置标题

            //设置背景色(清空屏幕色)
            //在每个新的渲染迭代开始的时候我们总是希望清屏，否则我们仍能看见上一次迭代的渲染结果（这可能是你想要的效果，但通常这不是）。
            GL.ClearColor(Color4.CornflowerBlue);  //需要引用 System.Drawing
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //清空屏幕缓冲
            //当调用glClear函数，清除颜色缓冲之后，整个颜色缓冲都会被填充为glClearColor里所设置的颜色。
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //交换颜色缓冲
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
