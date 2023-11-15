using System.Diagnostics;

namespace Компилятор
{
    class InputOutput
    {
        const byte ERRMAX = 9;
        public static char Ch { get; private set; }
        public static TextPosition positionNow = new TextPosition();
        public static bool isEnd = false;
        private static string line;
        private static byte lastInLine = 0;
        private static List<Err> err;
        private static StreamReader file;
        private static uint errCount = 0;

        /* открытие файла */
        public static void open(string path)
        {
            file = File.OpenText(path);
            ReadNextLine();
            Ch = line[0];
        }

        /* работа */
        public static void process()
        {
            var lexicalAnalyzer = new LexicalAnalyzer();
            lexicalAnalyzer.process();
        }

        /* инкремент позиции чтения */
    public static void NextCh()
        {
           if (positionNow.charNumber == lastInLine)
           {
                ListThisLine();
                if (err.Count > 0)
                        ListErrors();
                ReadNextLine();
                positionNow.lineNumber++;
                positionNow.charNumber = 0;
           }
           else ++positionNow.charNumber;
           Ch = line[positionNow.charNumber];
        }

    /* вывод строки */
        private static void ListThisLine()
        {
            Console.WriteLine(line);
        }

        /* инкремент позиции для строки */
        private static void ReadNextLine()
        {
            if (!file.EndOfStream)
            {
                line = file.ReadLine();
                err = new List<Err>();
                lastInLine = (byte)(line.Length - 1);
            }
            else
            {
                End();
            }
        }

        static void End()
        {
            isEnd = true;
            Console.WriteLine($"Компиляция завершена: : ошибок — {errCount}!");
        }

        /* вывод ошибок */
        static void ListErrors()
        {
            var pos = 6 - $"{positionNow.lineNumber} ".Length;
            string s;
            foreach (var item in err)
            {
                ++errCount;
                s = "**";
                if (errCount < 10) s += "0";
                s += $"{errCount}**";
                while (s.Length +5 < pos + item.errorPosition.charNumber) s += " ";
                s += $"^ ошибка {item.errorCode}";
                Console.WriteLine(s);
            }
        }
        
        /* запись ошибок */
        public static void Error(byte errorCode, TextPosition position)
        {
            if (err.Count <= ERRMAX)
            {
                var e = new Err(position, ErrorCodes.Dictionary[errorCode]);
                err.Add(e);
            }
        }
    }
}