namespace ConsoleApp1.lab2_1;

public class Program
{
    public static void Main_(string[] args)
    {
        var x = InputDataWithCheck.InputDouble("Введите координату точки x (вещ. число)");
        var y = InputDataWithCheck.InputDouble("Введите координату точки y (вещ. число)");

        var point = new Point(x, y);
        Console.WriteLine($"point = {point}");
        Console.WriteLine($"Расстояние от точки до O = {point.calcLenToO()}");
        
    }
}