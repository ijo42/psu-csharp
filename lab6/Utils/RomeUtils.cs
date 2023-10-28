using System;
using System.Collections.Generic;
using System.Text;

namespace RomeCalc.Utils;

public static class RomeUtils
{
    private static readonly Dictionary<char, int> RomanNumberDictionary;
    private static readonly Dictionary<int, string> NumberRomanDictionary;

    static RomeUtils()
    {
        /* словарь для перевода в римскую СС */
        RomanNumberDictionary = new Dictionary<char, int>
        {
            { 'I', 1 },
            { 'V', 5 },
            { 'X', 10 },
            { 'L', 50 },
            { 'C', 100 },
            { 'D', 500 },
            { 'M', 1000 },
        };

        /* словарь для обратного перевода */
        NumberRomanDictionary = new Dictionary<int, string>
        {
            { 1000, "M" },
            { 900, "CM" },
            { 500, "D" },
            { 400, "CD" },
            { 100, "C" },
            { 90, "XC" },
            { 50, "L" },
            { 40, "XL" },
            { 10, "X" },
            { 9, "IX" },
            { 5, "V" },
            { 4, "IV" },
            { 1, "I" },
        };
    }

    /* метод перевода в рискую СС */
    public static string To(int number)
    {
        if(number is > 3999 or < 0)
        {
            Console.Error.Write("Ввод должен быть менее 3999 и более 0");
            return "N";
        }
        var roman = new StringBuilder();
    
        foreach (var item in NumberRomanDictionary)
        {
            while (number >= item.Key)
            {
                roman.Append(item.Value);
                number -= item.Key;
            }
        }

        return roman.ToString();
    }

    /* метод для обратного перевода */
    public static int From(string roman)
    {
        var total = 0;
        var previousRoman = '\0';

        foreach (var currentRoman in roman)
        {
            var previous = previousRoman != '\0' ? RomanNumberDictionary[previousRoman] : '\0';
            var current = RomanNumberDictionary[currentRoman];

            if (previous != 0 && current > previous)
            {
                total = total - 2 * previous + current;
            }
            else
            {
                total += current;
            }

            previousRoman = currentRoman;
        }

        return total;
    }
}