using ConsoleApp1.lab4;

namespace ConsoleApp1.lab5;

public class Program
{
    public static void Main(string[] args)
    {
        // з. 1
        /*var L = new List<int> {1,2,3};
        Console.WriteLine("Исходный список: ");
        L.ForEach(k=> Console.Write($"{k}\t"));
        ListFuncs.reverse(L);
        Console.WriteLine("\nПеревернутый список: ");
        L.ForEach(k=> Console.Write($"{k}\t"));*/

        // з. 2
        /*var L = new LinkedList<int>(new List<int> {1,2,3});
        Console.WriteLine("Исходный список: ");
        foreach (var word in L)
        {
            Console.Write(word + " ");
        }

        ListFuncs.boundWithF(L, 1, 5);
        Console.WriteLine("\nНовый список: ");
        foreach (var word in L)
        {
            Console.Write(word + " ");
        }*/
        
        // з. 3
        /*HashSet<int> allDiscos = new() { 1, 2, 3, 4, 5 },
            fDiscos = new() { 1, 3, 5 },
            sDiscos = new() { 1, 2, 5 },
            tDiscos = new() { 1, 2, 5 };
        
        ListFuncs.discosStatistic(allDiscos, fDiscos, sDiscos, tDiscos);*/

        // з. 4
        ListFuncs.admittedApplicants("applicants.txt");
    }
}