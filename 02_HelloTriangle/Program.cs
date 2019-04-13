
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
}
