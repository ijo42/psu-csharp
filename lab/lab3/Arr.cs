using ConsoleApp1.lab2_1;

namespace ConsoleApp1.lab3;

public class Arr
{
    private int[,] arr;
    
    public Arr(int n,int m) /* ввод построчно c клавиатуры*/
    {
        arr = new int[n,m];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                arr[i, j] = InputDataWithCheck.InputIntegerWithValidation($"Введите элемент {i}; {j}");
            }
        }
    }
    
    public Arr(int n) /* ввод случайными числами по принципу: четные - в черные ячейки шахматной доски, нечетные - в остальные */
    {
        arr = new int[n,n];
        var rnd = new Random();
        
        for (int i = 0, k; i < n * n; )
        {
            k = rnd.Next(100);
            if (k % 2 == i % 2)
            {
                arr[i / n, i % n] = k;
                i++;
            }
        }
    }

    public Arr(double k) /* ввод змейкой: с правого нижнего конца к началу строки, вверх на строку и до конца строки, и т.д*/
    {
        arr = new int[(int)k,(int)k];
        int i = (int)k, j = (int)k, l = 1;
        bool toRight = false;
        while (i > 0)
        {
            arr[i - 1, j - 1] = l++;
            if ((j == 1 && !toRight) || (j == (int)k && toRight))
            {
                toRight = !toRight;
                i--;
            }
            else if(toRight)
            {
                j++;
            }
            else
            {
                j--;
            }
        }
    }

    public Arr(int[,] arr)
    {
        this.arr = arr;
    }

    /* форматирование в таблицу */
    public override string ToString()
    {
        string str = "";
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                str += ($"{arr[i, j],5}");
            }

            str += "\n";
        }

        return str;
    }
    
    /* операция умножения двух матриц строка-на-столбец */
    public static Arr operator *(Arr a, Arr b)
    {
        int[,] aArr = a.arr, bArr = b.arr;
        if (aArr.GetLength(1) != bArr.GetLength(0))
        {
            throw new Exception("Матрицы перемножить невозможно");
        }
        
        var res = new int[aArr.GetLength(0), bArr.GetLength(1)];
        for (var i = 0; i < aArr.GetLength(0); i++)
        {
            for (var j = 0; j < bArr.GetLength(1); j++)
            {
                for (var k = 0; k < bArr.GetLength(0); k++)
                {
                    res[i, j] += aArr[i, k] * bArr[k, j];
                }
            }
        }

        return new Arr(res);

    }
    
    /* операция сложения двух матриц */
    public static Arr operator +(Arr a, Arr b)
    {
        int[,] aArr = a.arr, bArr = b.arr;
        if (aArr.GetLength(0) != bArr.GetLength(0) ||
            aArr.GetLength(1) != bArr.GetLength(1))
        {
            throw new Exception("Матрицы сложить невозможно");
        }
        
        var res = new int[aArr.GetLength(0), bArr.GetLength(1)];
        for (var i = 0; i < aArr.GetLength(0); i++)
        {
            for (var j = 0; j < bArr.GetLength(1); j++)
            {
                {
                    res[i, j] += aArr[i, j] + bArr[i, j];
                }
            }
        }

        return new Arr(res);

    }
    
    /* 9. В городе П. есть m банков. Известны величины задолженностей банков друг другу Укажите банк с максимальным долгом.*/
    public int maxSumm() /* для поиска максимальной задолжности реализуется поиск максимальной суммы построчно (от 0 до диаг. элемента)*/
    {
        int maxSumm = 0, maxSummI = -1;
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            int tSumm = 0;
            for (int j = 0; j-i + 2 < arr.GetLength(1) / 2.0; j++)
            {
                tSumm += arr[i, j];
            }

            Console.WriteLine($"i= {i}; s = {tSumm}");
            if (tSumm > maxSumm)
            {
                maxSumm = tSumm;
                maxSummI = i;
            }
        }

        return maxSummI;
    }
}