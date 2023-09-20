using ConsoleApp1.lab2_1;

namespace ConsoleApp1.lab2_2;

public class Program
{
    public static void Main_(string[] args)
    {
        var x = InputDataWithCheck.InputDouble("Введите координату точки x (вещ. число)");
        var y = InputDataWithCheck.InputDouble("Введите координату точки y (вещ. число)");

        var point1 = new Point(x, y);        
        
        x = InputDataWithCheck.InputDouble("Введите координату точки x (вещ. число)");
        y = InputDataWithCheck.InputDouble("Введите координату точки y (вещ. число)");
        var point2 = new Point(x, y);
       
        
        Console.WriteLine($"point = {point1}");
        Console.WriteLine($"Расстояние от точки до O = {point1.calcLenToO()}");

        Console.WriteLine();
        point1--;
        Console.WriteLine($"point1-: {point1}");

        point1 = -point1;
        Console.WriteLine($"-point1: {point1}");

        Console.WriteLine($"point: {point1}");
        Console.WriteLine();
        
        int k = point1;
        Console.WriteLine($"Неявно приведенный point1 = {k}");
        Console.WriteLine($"Явно приведенный point1 = {(double)point1}");

        Console.WriteLine($"point1 - 5 = {point1 - 5}");
        Console.WriteLine($"k - point1 = {5 - point1}");
        Console.WriteLine($"point1 - point2 = {point1 - point2}");

    }
}