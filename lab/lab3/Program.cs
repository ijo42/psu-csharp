using ConsoleApp1.lab2_1;

namespace ConsoleApp1.lab3;

public class Program
{
    public static void Main_(string[] args)
    {
        /*var o = new Arr(
            InputDataWithCheck.InputIntegerWithValidation("Введите размерность матрицы N"), 
            InputDataWithCheck.InputIntegerWithValidation("Введите размерность матрицы M")
            );
        Console.WriteLine("Первый массив: ");
        Console.WriteLine(o.ToString());*/
           var g = new Arr((double)InputDataWithCheck.InputIntegerWithValidation("Введите размерность матрицы"));
        Console.WriteLine("Третий массив: ");
           Console.WriteLine(g.ToString());
           
        
        /*var d = new Arr(
            InputDataWithCheck.InputIntegerWithValidation("Введите размерность матрицы N")
            );
        Console.WriteLine("Второй массив (ЛР 21 - з.3): ");
        Console.WriteLine(d.ToString());
        Console.WriteLine($"Самый большой долг имеет банк с i = {d.maxSumm()}");*/


        /*
        Arr a = new Arr((double)2), b = new Arr(2), c = new Arr((double)2);
        Console.WriteLine($"a=\n{a}");
        Console.WriteLine($"b=\n{b}");
        Console.WriteLine($"c=\n{c}");

        Console.WriteLine($"a*c = \n{a*c}\nb*c = \n{b*c}\n a*c+b*c=\n{a*c+b*c}");*/

    }
}