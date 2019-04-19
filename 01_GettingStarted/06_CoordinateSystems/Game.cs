using System;
using System.Drawing;
using Common;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace _06_CoordinateSystems
{
    internal class Game : GameWindow
    {
        private Shader _Shader;

        /// <summary>
        ///     Address of the color parameter
        /// </summary>
        private int attribute_vcol;

        /// <summary>
        ///     Address of the position parameter
        /// </summary>
        private int attribute_vpos;

        /// <summary>
        ///     Array of our vertex colors
        /// </summary>
        private Vector3[] coldata;

        /// <summary>
        ///     Index Buffer Object
        /// </summary>
        private int ibo_elements;

        /// <summary>
        ///     Array of our indices
        /// </summary>
        private int[] indicedata;

        /// <summary>
        ///     Array of our modelview matrices
        /// </summary>
        private Matrix4[] mviewdata;

        /// <summary>
        ///     Current time, for animation
        /// </summary>
        private float time;

        /// <summary>
        ///     Address of the modelview matrix uniform
        /// </summary>
        private int uniform_mview;

        /// <summary>
        ///     Address of the Vertex Buffer Object for our color parameter
        /// </summary>
        private int vbo_color;

        /// <summary>
        ///     Address of the Vertex Buffer Object for our modelview matrix
        /// </summary>
        private int vbo_mview;

        /// <summary>
        ///     Address of the Vertex Buffer Object for our position parameter
        /// </summary>
        private int vbo_position;

        /// <summary>
        ///     Array of our vertex positions
        /// </summary>
        private Vector3[] vertdata;

        public Game() : base(512, 512, new GraphicsMode(32, 24, 0, 4))
        {
        }

        private void initProgram()
        {
            _Shader = new Shader(@"../../../../Shaders/06/vertex.glsl", @"../../../../Shaders/06/fragment.glsl");

            /** We have multiple inputs on our vertex shader, so we need to get
            * their addresses to give the shader position and color information for our vertices.
            * 
            * To get the addresses for each variable, we use the 
            * GL.GetAttribLocation and GL.GetUniformLocation functions.
            * Each takes the program's ID and the name of the variable in the shader. */
            attribute_vpos = GL.GetAttribLocation(_Shader.PID, "vPosition");
            attribute_vcol = GL.GetAttribLocation(_Shader.PID, "vColor");
            uniform_mview = GL.GetUniformLocation(_Shader.PID, "modelview");

            /** Now our shaders and program are set up, but we need to give them something to draw.
             * To do this, we'll be using a Vertex Buffer Object (VBO).
             * When you use a VBO, first you need to have the graphics card create
             * one, then bind to it and send your information. 
             * Then, when the DrawArrays function is called, the information in
             * the buffers will be sent to the shaders and drawn to the screen. */
            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);

            /** We'll need to get another buffer object to put our indice data into.  */
            GL.GenBuffers(1, out ibo_elements);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            initProgram();

            vertdata = new[]
            {
                new Vector3(-0.8f, -0.8f, -0.8f),
                new Vector3(0.8f, -0.8f, -0.8f),
                new Vector3(0.8f, 0.8f, -0.8f),
                new Vector3(-0.8f, 0.8f, -0.8f),
                new Vector3(-0.8f, -0.8f, 0.8f),
                new Vector3(0.8f, -0.8f, 0.8f),
                new Vector3(0.8f, 0.8f, 0.8f),
                new Vector3(-0.8f, 0.8f, 0.8f)
            };

            indicedata = new[]
            {
                //left
                0, 2, 1,
                0, 3, 2,
                //back
                1, 2, 6,
                6, 5, 1,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //front
                0, 7, 3,
                0, 4, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
            };


            coldata = new[]
            {
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f)
            };

            mviewdata = new[]
            {
                Matrix4.Identity
            };

            Title = "Hello OpenTK!";
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
            GL.EnableVertexAttribArray(attribute_vcol);

            GL.DrawElements(BeginMode.Triangles, indicedata.Length, DrawElementsType.UnsignedInt, 0);

            GL.DisableVertexAttribArray(attribute_vpos);
            GL.DisableVertexAttribArray(attribute_vcol);

            GL.Flush();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (vertdata.Length * Vector3.SizeInBytes), vertdata,
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (coldata.Length * Vector3.SizeInBytes), coldata,
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vcol, 3, VertexAttribPointerType.Float, true, 0, 0);

            time += (float) e.Time;

            mviewdata[0] = Matrix4.CreateRotationY(0.55f * time) * Matrix4.CreateRotationX(0.15f * time) *
                           Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f) * Matrix4.CreatePerspectiveFieldOfView(1.3f,
                               ClientSize.Width / (float) ClientSize.Height, 1.0f, 40.0f);

            GL.UniformMatrix4(uniform_mview, false, ref mviewdata[0]);

            _Shader.Use();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr) (indicedata.Length * sizeof(int)), indicedata,
                BufferUsageHint.StaticDraw);
        }
    }
}