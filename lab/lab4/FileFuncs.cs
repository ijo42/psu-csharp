using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApp1.lab4;

public class FileFuncs
{
    private const string FilePath = "/home/ijo42/RiderProjects/ConsoleApp1/lab/lab4/assets/";
    private const int N = 10;
    /* запись бин файла */
    public static void WriteRandom(string fileName)
    {
        var rnd = new Random();
        try
        {
            using var bw = new BinaryWriter(File.Open(FilePath + fileName, FileMode.OpenOrCreate));
            
            for (var i = 1; i <= N; i++)
            {
                var k = rnd.Next(0, 10);
                Console.Write($"{k}\t");
                bw.Write(k);
            }
        }
        catch (FileNotFoundException exp)
        {
            Console.WriteLine("Ошибка чтения файла");
            return;
        }

        Console.WriteLine();
    }

    /* копирование файла исключая числа кратные K */
    public static void CopyToFileExcludingK(string inFileName, string outFileName, int k)
    {
        try
        {
            using var br = new BinaryReader(File.Open(FilePath + inFileName, FileMode.Open));
            using var bw = new BinaryWriter(File.Open(FilePath + outFileName, FileMode.OpenOrCreate));
            Console.WriteLine($"Содержимое файла {inFileName}:");
            for (var i = 0; i < N; i++)
            {
                var c = br.ReadInt32();
                if ((c % k) != 0)
                {
                    
                    Console.Write($"{c}\t");
                    bw.Write(c);
                }
            }
        }
        catch (FileNotFoundException exp)
        {
            Console.WriteLine("Ошибка чтения файла");
        }
    }

    /* Скопировать файл в матрицу и найти столбец, сумма которого дальше всего от 0 - большая по абс. значению */
    public static int CopyToMatrixAndFindFarthestColumnFromZero(string fileName, int n)
    {
        var arr = new int[n, n];
        try
        {
            using var br = new BinaryReader(File.Open(FilePath + fileName, FileMode.Open));
            for (var i = 0; (i < N) && (i < n * n); i++)
            {
                var c = br.ReadInt32();
                arr[i / n, i % n] = c;
            }

            for (var i = N; i < n * n; i++)
            {
                arr[i / n, i % n] = 0;
            }

            var str = "";
            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    str += ($"{arr[i, j],5}");
                }

                str += "\n";
            }

            Console.Write(str);
        }
        catch (FileNotFoundException exp)
        {
            Console.WriteLine("Ошибка чтения файла");
            return -1;
        }


        int maxV = 0, maxI = 0;

        for (var i = 0; i < n; i++)
        {
            var tmpMult = 1;
            for (var j = 0; j < n; j++)
            {
                tmpMult *= arr[j, i];
            }
            Console.WriteLine($"i = {i} => {tmpMult}");

            if (maxV < Math.Abs(tmpMult))
            {
                maxV = Math.Abs(tmpMult);
                maxI = i;
            }
        }
        return maxI + 1;
    }

    /* заполнение файла структуры */
    public static void fillToyFile(string filename)
    {
        var toys = new List<Toy>();
        toys.Add(new Toy{name="Кран", price=100, fromAge=5, toAge=10});
        toys.Add(new Toy{name="Машинка", price=50, fromAge=2, toAge=5});
        toys.Add(new Toy{name="Мяч", price=100, fromAge=2, toAge=5});
        toys.Add(new Toy{name="Кукла", price=150, fromAge=1, toAge=9});

        try
        {
            using var file = File.OpenWrite(FilePath + filename);
            var writer = new BinaryFormatter();
#pragma warning disable SYSLIB0011
            writer.Serialize(file, toys); // Writes the entire list.
#pragma warning restore SYSLIB0011
        }catch (FileNotFoundException exp)
        {
            Console.WriteLine("Ошибка чтения файла");
        }
    }

    /* чтение из файла
        Получить сведения о том, можно ли подобрать
       игрушку, любую, кроме мяча, подходящую ребенку трех лет*/
    public static List<Toy> FilterToyFile(string filename)
    {
        List<Toy> data;
        try
        {
            using var file = File.OpenRead(FilePath + filename);
            var reader = new BinaryFormatter();
#pragma warning disable SYSLIB0011
            data = (List<Toy>)reader.Deserialize(file); // Reads the entire list.
#pragma warning restore SYSLIB0011
        }
        catch (FileNotFoundException exp)
        {
            Console.WriteLine("Ошибка чтения файла");
            return new List<Toy>();
        }

        return new List<Toy>(data.Where(t => !t.name.Equals("Мяч") && t is { fromAge: <= 3, toAge: >= 3 }));
    }

    /* сумма максимального и минимального элементов файла (одно число в строке) */
    public static int sumMaxMin(string filename)
    {
        int max = int.MinValue, min = int.MaxValue, t;
        try
        {
            using var fr = File.OpenText(FilePath + filename);

            for (var s = fr.ReadLine(); s != null; s = fr.ReadLine())
            {
                t = Convert.ToInt32(s);
                if (t < min)
                    min = t;
                else if (t > max)
                    max = t;
            }
        }catch (FileNotFoundException exp)
        {
            Console.WriteLine("Ошибка чтения файла");
            return -1;
        }

        return max + min;
    }

    /* сумма четных (несколько чисел в строке) */
    public static int sumEven(string filename)
    {
        int sum = 0, t;
        try
        {
            using var fr = File.OpenText(FilePath + filename);
            for (var s = fr.ReadLine(); s != null; s = fr.ReadLine())
            {
                foreach (var i in s.Split(" "))
                {
                    t = Convert.ToInt32(i);
                    if (t % 2 == 0)
                        sum += t;
                }
            }
        }catch (FileNotFoundException exp)
        {
            Console.WriteLine("Ошибка чтения файла");
            return -1;
        }

        return sum;
    }

    /* скопировать первый символ каждой строки в новый файл */
    public static void copyFirstSymbol(string inFilename, string outFilename)
    {
        try
        {
            using var fr = File.OpenText(FilePath + inFilename);
            using var fw = File.CreateText(FilePath + outFilename);
            for (var s = fr.ReadLine(); s != null; s = fr.ReadLine())
            {
                fw.WriteLine(s[0]);
            }
        }catch (FileNotFoundException exp)
        {
            Console.WriteLine("Ошибка чтения файла");
        }
    }
}