﻿using System;
using System.Text.RegularExpressions;

namespace Компилятор
{
    class LexicalAnalyzer
    {
        private byte symbol; // код символа
        private TextPosition token; // позиция символа
        private string ident;
        private int nmb_int; // значение целой константы
        private float nmb_float; // значение вещественной константы
        private char one_symbol; // значение символьной константы
        private const short MaxInt = short.MaxValue;
        private readonly Dictionary<byte, Dictionary<string, byte>> keywords = new Keywords().Kw;
        

        /* работа лексического анализатора*/
        public byte process() => NextSym();

        private byte NextSym()
        {
            // пропуск пробелов и табуляций
            while (!InputOutput.isEnd && InputOutput.Ch is ' ' or '\t') InputOutput.NextCh();
            symbol = 255;
            token.lineNumber = InputOutput.positionNow.lineNumber;
            token.charNumber = InputOutput.positionNow.charNumber;

            //сканировать символ
            switch (InputOutput.Ch)
            {
                    
                case var chr when char.IsDigit(chr):
                    nmb_int = 0;
                    while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                    {
                        var digit = (byte)(InputOutput.Ch - '0');
                        if (nmb_int <= MaxInt / 10 &&
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

                    if (InputOutput.Ch == '.')
                    {
                        InputOutput.NextCh();
                        nmb_float = nmb_int;
                        while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                        {
                            var digit = (byte)(InputOutput.Ch - '0');

                            nmb_int = 10 * nmb_int + digit;
                            InputOutput.NextCh();
                        }
                        symbol = Lexemes.floatc;
                    } else
                        symbol = Lexemes.intc;
                    break;
                case var chr when char.IsLetter(chr):
                    var name = "";
                    while  (char.IsLetter(InputOutput.Ch) || char.IsDigit(InputOutput.Ch))
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

                    if (Types.Kw.TryGetValue(name, out var typeKey))
                        InputOutput.declaredTypes.Enqueue(typeKey);

                    symbol = (byte)(keywordKey + (typeKey > 0 ? Lexemes.typesy : 0));

                    /* ошибка если ключевого слова не найдено */
                    if (symbol == 0)
                    {
                        symbol = Lexemes.ident;
                        InputOutput.declaredIdents.Enqueue(name);
                    }

                    break;
                case '<':
                    InputOutput.NextCh();

                   switch (InputOutput.Ch)
                   {
                       case '=':
                           symbol = Lexemes.laterequal; InputOutput.NextCh();
                           break;
                       case '>':
                           symbol = Lexemes.latergreater; InputOutput.NextCh();
                           break;
                       default:
                           symbol = Lexemes.later;
                           break;
                   }
                    break;                
                case '>':
                    InputOutput.NextCh();
                   switch (InputOutput.Ch)
                   {
                       case '=':
                           symbol = Lexemes.greaterequal; InputOutput.NextCh();
                           break;
                       default:
                           symbol = Lexemes.greater;
                           break;
                   }
                    break;
                case ':':
                    InputOutput.NextCh();
                    if (InputOutput.Ch == '=')
                    {
                        symbol = Lexemes.assign; InputOutput.NextCh();
                    }
                    else
                        symbol = Lexemes.colon;
                    break;
                case ';':
                    symbol = Lexemes.semicolon;
                    InputOutput.NextCh();
                    break;
                case '.':
                    InputOutput.NextCh();
                    if (InputOutput.Ch == '.')
                    {
                        symbol = Lexemes.twopoints; InputOutput.NextCh();
                    }
                    else symbol = Lexemes.point;
                    break;
                case ',':
                    InputOutput.NextCh();
                    symbol = Lexemes.comma;
                    break;
                case '=':
                    symbol = Lexemes.equal;
                    InputOutput.NextCh();
                    break;
                /* многострочные комментарии */
                case '{':
                    while (!InputOutput.isEnd)
                    {
                        if (InputOutput.Ch == '}')
                        {
                            InputOutput.NextCh();
                            return NextSym();
                        }
                        InputOutput.NextCh();
                    }             
                    break;
                case '*':
                    symbol = Lexemes.star;
                    InputOutput.NextCh();
                    break;
                case '/':
                    symbol = Lexemes.slash;
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
