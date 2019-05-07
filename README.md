# LearnOpenGL-with-OpenTK

​	学习OpenGL有助于理解渲染流程,了解渲染幕后的工作和Shader编程,以及一些数学方法等都有非常大的帮助.  特别是对Unity3D的程序员来说更为重要,因为Unity封装了很多细节, 学习一遍OpenGL会使你的思维更加清晰.

​	 学习OpenGL我推荐[LearnOpenGL]: https://learnopengl.com/
​        中文地址[LearnOpenGL CN]:https://learnopengl-cn.github.io/

​        教程里的代码都是用C/C++来编写的, 对于不太了解C++的人来说你可能会被与我们目标无关的内容(配置环境/第三方库)浪费大量精力,很难尽快入手学习. 所以我使用[OpenTK]:https://github.com/opentk/opentk 来重新编写里教程中的示例. OpenTK:The Open Toolkit library is a fast, low-level C# binding for OpenGL, OpenGL ES and OpenAL. It runs on all major platforms and powers hundreds of apps, games and scientific research.

## 环境配置

Visual Studio 2017

OpenTK 3.0.1 - 使用NuGet安装

GlmNet 0.7.0 - GLM的.Net实现 使用NuGet安装

System.Drawing - 提供对图片的操作 通过引用添加到项目

克隆工程后需要先还原NuGet包, 右键工程'管理NuGet程序包'点击还原即可.

## 一些函数接口对应关系

OpenGL提供的接口/函数/枚举/常量等 在OpenTK里都有对应 对应关系规律也比较明显, 如下图

| OpenGL            | Opentk                   |
| :---------------- | :----------------------- |
| glCreateShader    | GL.CreateShader          |
| GL_VERTEX_SHADER  | ShaderType.VertexShader  |
| glGenVertexArrays | GL.GenVertexArray        |
| GL_ARRAY_BUFFER   | BufferTarget.ArrayBuffer |
| glDrawElements    | GL.DrawElements          |
| glViewport        | GL.Viewport              |
| ...               | ...                      |

其他的对模型/贴图/数学操作等 .Net/OpenTK要么已经封装, 要么你很容易能找到第三方库来处理.

当前我已经完成了教程的第一部分Begining的内容, 代码中也添加了详细的注释.后续的内容会陆续添加.

![](https://github.com/huangkumao/GitProjectImgs/blob/master/LearnOpenGL-with-OpenTK/01/HelloWindow.png) ![](https://github.com/huangkumao/GitProjectImgs/blob/master/LearnOpenGL-with-OpenTK/01/HelloTriangel.png) ![](https://github.com/huangkumao/GitProjectImgs/blob/master/LearnOpenGL-with-OpenTK/01/Camera.gif?raw)

