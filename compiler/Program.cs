namespace Компилятор
{
    class Program
    {
        /* точка входа */
        static void Main()
        {
            const string path = @"/home/ijo42/RiderProjects/ConsoleApp1/compiler/examples/test2.pas";
            InputOutput.open(path);
            InputOutput.process();
        }
    }
}
