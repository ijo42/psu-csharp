namespace ConsoleApp1;

/*
 * Мальцев Александр
 * КМБ-1   вн-т 9
 */

public partial class Program
{
    public static void Main(string[] args)
    {
        var threeDouble = new ThreeDouble(1.9, 2.5,3.1);
        Console.WriteLine($"Параметры длин: {threeDouble}");
        threeDouble.Round();
        Console.WriteLine($"Параметры длин после округления: {threeDouble}");

        var newThreeDouble = new ThreeDouble(threeDouble);
        Console.WriteLine($"Параметры длин копии: {newThreeDouble}");

        
        Console.WriteLine();
       var triangle = new Triangle(7.9, 2, 1);
       Console.WriteLine($"Параметры треугольника: {triangle}");
       Console.WriteLine($"Периметр треугольника: {triangle.CalculatePerimeter()}");
       triangle.Round();
       Console.WriteLine($"Параметры тругольника после округления: {triangle}");
       Console.WriteLine($"Полупериметр: {triangle.CalculateSemiPerimeter()}");
       Console.WriteLine($"Периметр: {triangle.CalculatePerimeter()}");
       
       var newTriangle = new ThreeDouble(triangle);
       Console.WriteLine($"Параметры длин копии: {newTriangle}");

    }
}