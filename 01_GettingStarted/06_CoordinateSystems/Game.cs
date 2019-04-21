using System;
using System.Drawing;
using System.Drawing.Imaging;
using Common;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace _06_CoordinateSystems
{
    internal class Game : GameWindow
    {
        private Shader _Shader;

        private int _TexID; //贴图ID
        private int _TexID2;

        private int attribute_texCoord;
        private int attribute_vpos;
        private float[] coord;


        private Matrix4 mviewdata;

        private float time;
        private int uniform_mview;

        private int vbo_coord;
        private int vbo_position;

        private float[] vertdata;

        public Game() : base(512, 512, new GraphicsMode(32, 24, 0, 4))
        {
        }

        private void InitProgram()
        {
            _Shader = new Shader(@"../../../../Shaders/06/vertex.glsl", @"../../../../Shaders/06/fragment.glsl");

            attribute_vpos = GL.GetAttribLocation(_Shader.PID, "vPosition");
            attribute_texCoord = GL.GetAttribLocation(_Shader.PID, "aTexCoord");
            uniform_mview = GL.GetUniformLocation(_Shader.PID, "modelview");

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_coord);

            //加载贴图
            _TexID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _TexID);
            GL.TextureParameter(_TexID, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            GL.TextureParameter(_TexID, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);

            GL.TextureParameter(_TexID, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
            GL.TextureParameter(_TexID, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);

            var image = new Bitmap("../../../../Resources/04/tex.jpg");
            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            image.UnlockBits(data);
            image.Dispose();

            _TexID2 = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _TexID2);
            GL.TextureParameter(_TexID2, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            GL.TextureParameter(_TexID2, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);

            GL.TextureParameter(_TexID2, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
            GL.TextureParameter(_TexID2, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);

            image = new Bitmap("../../../../Resources/04/tex2.png");
            //这是因为OpenGL要求y轴0.0坐标是在图片的底部的，但是图片的y轴0.0坐标通常在顶部。
            image.RotateFlip(RotateFlipType.Rotate180FlipX); //垂直翻转图片
            data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            image.UnlockBits(data);
            image.Dispose();

            //要设置Shader属性 必须先Use一下 否则无效
            _Shader.Use();
            _Shader.SetInt("texture1", 0);
            _Shader.SetInt("texture2", 1);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitProgram();

            //定点数据
            vertdata = new[]
            {
                //背
                -0.5f, -0.5f, -0.5f,
                0.5f, -0.5f, -0.5f,
                0.5f, 0.5f, -0.5f,
                0.5f, 0.5f, -0.5f,
                -0.5f, 0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,

                //正
                -0.5f, -0.5f, 0.5f,
                0.5f, -0.5f, 0.5f,
                0.5f, 0.5f, 0.5f,
                0.5f, 0.5f, 0.5f,
                -0.5f, 0.5f, 0.5f,
                -0.5f, -0.5f, 0.5f,

                //左
                -0.5f, 0.5f, 0.5f,
                -0.5f, 0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,
                -0.5f, -0.5f, 0.5f,
                -0.5f, 0.5f, 0.5f,

                //右
                0.5f, 0.5f, 0.5f,
                0.5f, 0.5f, -0.5f,
                0.5f, -0.5f, -0.5f,
                0.5f, -0.5f, -0.5f,
                0.5f, -0.5f, 0.5f,
                0.5f, 0.5f, 0.5f,

                //下
                -0.5f, -0.5f, -0.5f,
                0.5f, -0.5f, -0.5f,
                0.5f, -0.5f, 0.5f,
                0.5f, -0.5f, 0.5f,
                -0.5f, -0.5f, 0.5f,
                -0.5f, -0.5f, -0.5f,

                //上
                -0.5f, 0.5f, -0.5f,
                0.5f, 0.5f, -0.5f,
                0.5f, 0.5f, 0.5f,
                0.5f, 0.5f, 0.5f,
                -0.5f, 0.5f, 0.5f,
                -0.5f, 0.5f, -0.5f
            };

            //贴图UV坐标
            coord = new[]
            {
                0.0f, 0.0f,
                1.0f, 0.0f,
                1.0f, 1.0f,
                1.0f, 1.0f,
                0.0f, 1.0f,
                0.0f, 0.0f,

                0.0f, 0.0f,
                1.0f, 0.0f,
                1.0f, 1.0f,
                1.0f, 1.0f,
                0.0f, 1.0f,
                0.0f, 0.0f,

                1.0f, 0.0f,
                1.0f, 1.0f,
                0.0f, 1.0f,
                0.0f, 1.0f,
                0.0f, 0.0f,
                1.0f, 0.0f,

                1.0f, 0.0f,
                1.0f, 1.0f,
                0.0f, 1.0f,
                0.0f, 1.0f,
                0.0f, 0.0f,
                1.0f, 0.0f,

                0.0f, 1.0f,
                1.0f, 1.0f,
                1.0f, 0.0f,
                1.0f, 0.0f,
                0.0f, 0.0f,
                0.0f, 1.0f,

                0.0f, 1.0f,
                1.0f, 1.0f,
                1.0f, 0.0f,
                1.0f, 0.0f,
                0.0f, 0.0f,
                0.0f, 1.0f
            };

            mviewdata = Matrix4.Identity;

            Title = "CoordinateSystems!";
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //开启深度测试
            GL.Enable(EnableCap.DepthTest);

            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_texCoord);

            //绑定贴图
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _TexID);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, _TexID2);

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertdata.Length / 3);

            GL.DisableVertexAttribArray(attribute_vpos);
            GL.DisableVertexAttribArray(attribute_texCoord);

            GL.Flush();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (vertdata.Length * sizeof(float)), vertdata,
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_coord);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (coord.Length * sizeof(float)), coord,
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_texCoord, 2, VertexAttribPointerType.Float, true, 0, 0);

            time += (float) e.Time;

            //对模型进行矩阵变换
            mviewdata = Matrix4.CreateRotationY(0.55f * time) * Matrix4.CreateRotationX(0.15f * time) *
                        Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f) * Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f),
                            ClientSize.Width / (float) ClientSize.Height, 1.0f, 40.0f);

            GL.UniformMatrix4(uniform_mview, false, ref mviewdata);

            _Shader.Use();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}