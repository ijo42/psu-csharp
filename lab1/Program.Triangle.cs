namespace ConsoleApp1;

public partial class Program
{
    private class Triangle: ThreeDouble
    {
        public Triangle(double aLen, double bLen, double cLen) : base(aLen, bLen, cLen)
        {
            this.aLen = aLen;
            this.bLen = bLen;
            this.cLen = cLen;
        }

        public double CalculatePerimeter() /* подсчёт периметра */
        {
            return aLen + bLen + cLen;
        }

        public double CalculateSemiPerimeter() /* подсчёт полупериметра */
        {
            return CalculatePerimeter() / 2.0;
        }

    }
}