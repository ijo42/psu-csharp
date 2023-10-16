namespace ConsoleApp1.lab4;

[Serializable]
public struct Toy
{
    public string name;
    public int price;
    public int fromAge;
    public int toAge;
    public override string ToString()
    {
        return $"Название: {name}, Цена: {price}, Возраст: {fromAge}-{toAge}";
    }
}