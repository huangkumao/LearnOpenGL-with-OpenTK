using System;
using System.Drawing;
using System.Drawing.Imaging;
using Common;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;


namespace _04_Textures
{
    class Game : GameWindow
    {
        private Shader _Shader;

        private int _VAO;
        private int _VBO;
        private int _EBO;
        private int _TexID; //贴图ID
        private int _TexID2;

        private float[] _VertData; //顶点数据

        public Game() : base(600, 600, GraphicsMode.Default, "", GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Shader";

            _Shader = new Shader(@"../../../../Shaders/04/vertex.glsl", @"../../../../Shaders/04/fragment.glsl");

            //传递给Shader的顶点数据
            _VertData = new[]
            {
                // positions          // colors           // texture coords
                0.5f,  0.5f, 0.0f,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f, // top right
                0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 0.0f, // bottom right
                -0.5f, -0.5f, 0.0f,   0.0f, 0.0f, 1.0f,   0.0f, 0.0f, // bottom left
                -0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 0.0f,   0.0f, 1.0f  // top left 
            };

            //创建VAO
            _VAO = GL.GenVertexArray();
            GL.BindVertexArray(_VAO); //绑定VAO - 表示下面的操作(VBO)都和此VAO相关 直到解绑

            int[] indices = {
                0, 1, 3, // first triangle
                1, 2, 3  // second triangle
            };
            //EBO
            _EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, BufferUsageHint.StaticDraw);

            //创建VBO
            _VBO = GL.GenBuffer();
            //绑定 表示当前对类型(BufferTarget.ArrayBuffer)的操作 都是针对此VBO的
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            //绑定用户数据到GPU
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_VertData.Length * sizeof(float)), _VertData, BufferUsageHint.StaticDraw);
            //告知GL如何解析数据. 第一个参数0 对应的VS中的 (location = 0)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            //启用定点属性
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            //解绑VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //解绑VAO 传递0表示解绑当前的VAO
            GL.BindVertexArray(0);

            //加载贴图
            _TexID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _TexID);
            GL.TextureParameter(_TexID, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TextureParameter(_TexID, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TextureParameter(_TexID, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameter(_TexID, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            var image = new Bitmap("../../../../Resources/04/tex.jpg");
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            image.UnlockBits(data);
            image.Dispose();

            _TexID2 = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _TexID2);
            GL.TextureParameter(_TexID2, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TextureParameter(_TexID2, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TextureParameter(_TexID2, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameter(_TexID2, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            image = new Bitmap("../../../../Resources/04/tex2.png");
            //这是因为OpenGL要求y轴0.0坐标是在图片的底部的，但是图片的y轴0.0坐标通常在顶部。
            image.RotateFlip(RotateFlipType.Rotate180FlipX); //垂直翻转图片
            data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            image.UnlockBits(data);
            image.Dispose();

            //要设置Shader属性 必须先Use一下 否则无效
            _Shader.Use();
            _Shader.SetInt("texture1", 0);
            _Shader.SetInt("texture2", 1);

            GL.ClearColor(Color.CornflowerBlue);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            //绑定贴图
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _TexID);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, _TexID2);

            //指定Shader程序
            _Shader.Use();
            //指定VAO
            GL.BindVertexArray(_VAO);
            
            //绘制图像
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

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
