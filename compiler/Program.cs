namespace Компилятор
{
    class Program
    {
        /* точка входа */
        static void Main()
        {
            const string path = @"C:\Users\ijo42\RiderProjects\psu-csharp\compiler\examples\test2.pas";
            InputOutput.open(path);
            InputOutput.process();
        }
    }
}
