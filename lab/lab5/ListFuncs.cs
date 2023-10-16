namespace ConsoleApp1.lab5;

public class ListFuncs
{
    public static List<int> reverse(List<int> list)
    {
        list.Reverse();
        return list;
    }

    // в списке L справа и слева от элемента E вставляет элемент F;
    public static LinkedList<int> boundWithF(LinkedList<int> list, int searchElement, int F)
    {
        var element = list.Find(searchElement);
        if(element != null)
        {
            list.AddAfter(element, F);
            list.AddBefore(element, F);
        }

        return list;
    }

    public static void discosStatistic(HashSet<int> allDiscos, HashSet<int>fDiscos, HashSet<int>sDiscos, HashSet<int>tDiscos)
    {
        
        Console.WriteLine("Список дискотек: ");
        foreach (var word in allDiscos)
        {
            Console.Write($"{word} ");
        }
        Console.WriteLine("\nСписок дискотек 1 студента: ");
        foreach (var word in fDiscos)
        {
            Console.Write($"{word} ");
        }
        Console.WriteLine("\nСписок дискотек 2 студента: ");
        foreach (var word in sDiscos)
        {
            Console.Write($"{word} ");
        }
        
        Console.WriteLine("\nСписок дискотек 3 студента: ");
        foreach (var word in tDiscos)
        {
            Console.Write($"{word} ");
        }
        
        Console.WriteLine("\nВ какие дискотеки из перечня ходили все студенты группы:");
        foreach (var disco in fDiscos.Intersect(sDiscos).Intersect(tDiscos)) // пересечение множеств посещенных студентами
        {
            Console.Write($"{disco}\t");
        }
        Console.WriteLine("\nВ какие дискотеки из перечня ходили некоторые студенты группы:");
        foreach (var disco in fDiscos.Union(sDiscos).Union(tDiscos))         // объединение множеств посещенных студентами
        {
            Console.Write($"{disco}\t");
        }
        Console.WriteLine("\nВ какие дискотеки из перечня не ходил ни один из студентов группы:");
        foreach (var disco in allDiscos.Except(fDiscos.Union(sDiscos).Union(tDiscos))) // allDiscos - (объединение множеств посещенных студентами)
        {
            Console.Write($"{disco}\t");
        }
    }
    
    public static void admittedApplicants(string filename)
    {
        var list = new SortedList<string, string>();
        try
        {
            using var fr = File.OpenText("/home/ijo42/RiderProjects/ConsoleApp1/lab/lab5/" + filename);
            for (var s = fr.ReadLine(); s != null; s = fr.ReadLine())
            {
                var line = s.Split(" ");
                int exam1 = int.Parse(line[2]), exam2 = int.Parse(line[3]), exam3 = int.Parse(line[4]);
                if (exam1 > 30 && exam2 > 30 && exam3 > 30 && exam3 + exam2 + exam1 > 130)
                {
                    list.Add(line[0], line[1]);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }

        foreach (var applicant in list)
        {
           Console.Write($"{applicant.Key} {applicant.Value}\n");
        }
    }
}