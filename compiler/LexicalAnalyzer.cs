using System;
using System.Text.RegularExpressions;

namespace Компилятор
{
    class LexicalAnalyzer
    {
        public const byte   // описание лексем
            star = 21, // *
            slash = 60, // /
            equal = 16, // =
            comma = 20, // ,
            semicolon = 14, // ;
            colon = 5, // :
            point = 61,	// .
            arrow = 62,	// ^
            leftpar = 9,	// (
            rightpar = 4,	// )
            lbracket = 11,	// [
            rbracket = 12,	// ]
            flpar = 63,	// {
            frpar = 64,	// }
            later = 65,	// <
            greater = 66,	// >
            laterequal = 67,	//  <=
            greaterequal = 68,	//  >=
            latergreater = 69,	//  <>
            plus = 70,	// +
            minus = 71,	// –
            lcomment = 72,	//  (*15
            rcomment = 73,	//  *)
            assign = 51,	//  :=
            twopoints = 74,	//  ..
            ident = 2,	// идентификатор
            floatc = 82,	// вещественная константа
            intc = 15,	// целая константа
            casesy = 31,
            elsesy = 32,
            filesy = 57,
            gotosy = 33,
            thensy = 52,
            typesy = 34,
            untilsy = 53,
            dosy = 54,
            withsy = 37,
            ifsy = 56,
            insy = 100,
            ofsy = 101,
            orsy = 102,
            tosy = 103,
            endsy = 104,
            varsy = 105,
            divsy = 106,
            andsy = 107,
            notsy = 108,
            forsy = 109,
            modsy = 110,
            nilsy = 111,
            setsy = 112,
            beginsy = 113,
            whilesy = 114,
            arraysy = 115,
            constsy = 116,
            labelsy = 117,
            downtosy = 118,
            packedsy = 119,
            recordsy = 120,
            repeatsy = 121,
            programsy = 122,
            functionsy = 123,
            procedurensy = 124;

        private byte symbol; // код символа
        private TextPosition token; // позиция символа
        private string addrName; // адрес идентификатора в таблице имен
        private int nmb_int; // значение целой константы
        private float nmb_float; // значение вещественной константы
        private char one_symbol; // значение символьной константы
        private const short MaxInt = short.MaxValue;
        private readonly Dictionary<byte, Dictionary<string, byte>> keywords = new Keywords().Kw;
        
        /* работа лексического анализатора*/
        public void process()
        {
            var list = new List<byte>();
            while (!InputOutput.isEnd)
            {
                list.Add(NextSym());
            }
            list.ForEach(s => Console.Write($"{s} "));
        }

        private byte NextSym()
        {
            // пропуск пробелов и табуляций
            while (!InputOutput.isEnd && InputOutput.Ch is ' ' or '\t') InputOutput.NextCh();
            token.lineNumber = InputOutput.positionNow.lineNumber;
            token.charNumber = InputOutput.positionNow.charNumber;

            //сканировать символ
            switch (InputOutput.Ch)
            {
                // regexp для цифры
                case var chr when new Regex("^\\d$").IsMatch(chr.ToString()):
                    nmb_int = 0;
                    while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                    {
                        var digit = (byte)(InputOutput.Ch - '0');
                        if (nmb_int < MaxInt / 10 ||
                        nmb_int == MaxInt / 10 &&
                        digit <= MaxInt % 10)
                            nmb_int = 10 * nmb_int + digit;
                        else
                        {
                            // константа превышает предел
                            InputOutput.Error(201, InputOutput.positionNow);
                            nmb_int = 0;
                            while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9') InputOutput.NextCh();
                        }
                        InputOutput.NextCh();
                    }
                    symbol = intc;
                    break;
                // regexp для буквы
                case var chr when new Regex("^[a-zA-Z]$").IsMatch(chr.ToString()):
                    var name = "";
                    while  (InputOutput.Ch >= 'a' && InputOutput.Ch <= 'z' ||
                            InputOutput.Ch >= 'A' && InputOutput.Ch <= 'Z' ||
                            InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                    {
                        name += InputOutput.Ch;
                        InputOutput.NextCh();
                    }

                    /* попытка взятия ключевого слова */
                    byte keywordKey = 0;
                    if (keywords.TryGetValue((byte)name.Length, out var lengthEntry))
                    {
                        lengthEntry.TryGetValue(name, out keywordKey);
                    }
                    symbol = keywordKey;

                    /* ошибка если ключевого слова не найдено */
                    if (keywordKey == 0)
                    {
                        InputOutput.Error(202, InputOutput.positionNow);
                        InputOutput.NextCh();
                    }

                    break;
                case '<':
                    InputOutput.NextCh();

                   switch (InputOutput.Ch)
                   {
                       case '=':
                           symbol = laterequal; InputOutput.NextCh();
                           break;
                       case '>':
                           symbol = latergreater; InputOutput.NextCh();
                           break;
                       default:
                           symbol = later;
                           break;
                   }
                    break;                
                case '>':
                    InputOutput.NextCh();
                   switch (InputOutput.Ch)
                   {
                       case '=':
                           symbol = greaterequal; InputOutput.NextCh();
                           break;
                       default:
                           symbol = greater;
                           break;
                   }
                    break;
                case ':':
                    InputOutput.NextCh();
                    if (InputOutput.Ch == '=')
                    {
                        symbol = assign; InputOutput.NextCh();
                    }
                    else
                        symbol = colon;
                    break;
                case ';':
                    symbol = semicolon;
                    InputOutput.NextCh();
                    break;
                case '.':
                    InputOutput.NextCh();
                    if (InputOutput.Ch == '.')
                    {
                        symbol = twopoints; InputOutput.NextCh();
                    }
                    else symbol = point;
                    break;
                case '=':
                    symbol = equal;
                    InputOutput.NextCh();
                    break;
                /* многострочные комментарии */
                case '{':
                    while (!InputOutput.isEnd)
                    {
                        if (InputOutput.Ch != '}')
                            InputOutput.NextCh();
                        else
                            break;
                    }             
                    InputOutput.NextCh();
                    break;
                default:
                    /* ошибка в случае неизвестной лексемы */
                    InputOutput.Error(202, InputOutput.positionNow);
                    InputOutput.NextCh();
            
                    break;
                }


            return symbol;
        }
    }
}
