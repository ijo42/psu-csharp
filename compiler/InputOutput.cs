using System.Diagnostics;

namespace Компилятор
{
    class InputOutput
    {
        const byte ERRMAX = 9;
        public static char Ch { get; private set; }
        public static TextPosition positionNow;
        public static bool isEnd;
        private static string line;
        private static byte lastInLine = 0;
        private static List<Err> err;
        private static StreamReader file;
        private static uint errCount = 0;
        private static List<string> fileContent = new();
        // словарь переменных
        public static Dictionary<string, VariableType>? varTypes = new();
        public static Queue<byte> declaredTypes = new();
        public static Queue<string> declaredIdents = new();
        public static List<Queue<byte>> lexemes = new() {new Queue<byte>(), new Queue<byte>(), new Queue<byte>()};

        /* открытие файла */
        public static void open(string path0)
        {
            file = File.OpenText(path0);
            ReadNextLine();
            Ch = line[0];
        }

        /* работа */
        public static void process()
        {
            var lexicalAnalyzer = new LexicalAnalyzer();
            var syntaxAnalyzer = new SyntaxAnalyzer(() => lexicalAnalyzer.process());

            while(!isEnd)
            {
                syntaxAnalyzer.Process();
            }

            var semanticAnalyzer = new SemanticAnalyzer();
            semanticAnalyzer.process();
        }

        /* инкремент позиции чтения */
    public static void NextCh()
        {
           if (positionNow.charNumber == lastInLine)
           {
                ListThisLine();
                if (err.Count > 0)
                        ListErrors();
                do
                {
                    ReadNextLine();
                } while (line.Length == 0 && !isEnd);
                positionNow.charNumber = 0;
           }
           else ++positionNow.charNumber;
           Ch = line[positionNow.charNumber];
        }
    
    /* вывод строки */
    public static void ListThisLine()
        {
            Console.WriteLine(line);
        }

        /* инкремент позиции для строки */
        public static void ReadNextLine()
        {
            if (!file.EndOfStream)
            {
                line = file.ReadLine();
                fileContent.Add(line);
                err = new List<Err>();
                lastInLine = (byte)(line.Length - 1);
                positionNow.lineNumber++;
            }
            else
            {
                End();
            }
        }

        static void End()
        {
            isEnd = true;
            Console.WriteLine($"Анализ завершен: ошибок — {errCount}!");
        }

        /* вывод ошибок */
        public static void ListErrors()
        {
            var pos = 6 - $"{positionNow.lineNumber} ".Length;
            string s;
            foreach (var item in err)
            {
                ++errCount;
                s = "**";
                if (errCount < 10) s += "0";
                s += $"{errCount}**";
                while (s.Length +3 < pos + item.errorPosition.charNumber) s += " ";
                s += $"^ ошибка {item.errorCode} {item.desc}";
                Console.WriteLine(s);
            }
        }
        
        /* запись ошибок */
        public static void Error(byte errorCode, TextPosition position)
        {
            Error(errorCode, position, null);
        }        /* запись ошибок */
        public static void Error(byte errorCode, TextPosition position, string? description)
        {
            if (err.Count <= ERRMAX)
            {
                var e = new Err(position, ErrorCodes.Dictionary[errorCode], description);
                err.Add(e);
            }
        }
    }
}