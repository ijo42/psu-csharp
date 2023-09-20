namespace ConsoleApp1.lab2_1;

public class Point /* класс отражает точку на коорд. плоскости xOy, поля - соотв. координаты точки */
{
    private double x { get; } /* поле x с get-доступом */
    private double y { get; } /* поле y с get-доступом */

    public Point(double x, double y) /* конструктор */
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString() /* перегруженный toString*/
    {
        return $"x = {x}; y = {y}";
    }

    public double calcLenToO() /* расстояние от точки до начала координат */
    {
        return double.Sqrt(x*x + y*y); /* sqrt(x^2 + y^2) */
    }
}