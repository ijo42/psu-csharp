namespace ConsoleApp1.lab2_2;

public class Point /* класс отражает точку на коорд. плоскости xOy, поля - соотв. координаты точки */
{
    private double x { get; set; } /* поле x с get-доступом */
    private double y { get; set; } /* поле y с get-доступом */

    public Point(double x, double y) /* конструктор */
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString() /* перегруженный toString*/
    {
        return $"x = {x}; y = {y}";
    }

    public static Point operator --(Point point1) /* перегрузка оператора --: выполняет вычитание по обоим координатам */
    {
        point1.x--;
        point1.y--;
        return point1;
    }

    public static Point operator -(Point point) /* перегрузка оператора : выполняет обмен координат точек*/
    {
        (point.x, point.y) = (point.y, point.x);
        return point;
    }

    public static implicit operator int(Point point) /* перегрузка неявного оператора приведения типа Point к int: возвращает целую часть поля X*/
    {
        return (int)point.x;
    }

    public static explicit operator double(Point point) /* перегрузка явного оператора приведения типа Point к double: возвращает поле Y*/
    {
        return point.y;
    }
    
    public static Point operator +(Point point, int val) /* перегрузка оператора + для Point, Int: уменьшает значения поля X на val */
    {
        point.x -= val;
        return point;
    }
        
    public static Point operator +(int val, Point point) /* перегрузка оператора + для Int, Point: уменьшает значения поля Y на val */
    {
        point.y -= val;
        return point;
    }

    public static double operator -(Point point1, Point point2) /* перегрузка оператора - для Point, Point: возвращает расстояние от точки до точки */
    {
        return double.Sqrt(double.Pow(point2.x - point1.x, 2) + double.Pow(point2.y - point1.y, 2));
    }
    
    public double calcLenToO() /* расстояние от точки до начала координат */
    {
        return double.Sqrt(x*x + y*y); /* sqrt(x^2 + y^2) */
    }
}