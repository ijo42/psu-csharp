namespace ConsoleApp1;

public partial class Program
{
    /* поля отвечают за длины треугольника */
    private class Triangle : ThreeDouble
    {
        public Triangle(double aLen, double bLen, double cLen) : base(aLen, bLen, cLen)
        {
        }

        public double CalculatePerimeter() /* подсчёт периметра треугольника */
        {
            return aLen + bLen + cLen;
        }

        public double CalculateSemiPerimeter() /* подсчёт полупериметра треугольника */
        {
            return CalculatePerimeter() / 2.0;
        }
    }
}