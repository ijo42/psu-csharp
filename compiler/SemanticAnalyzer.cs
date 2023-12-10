namespace Компилятор;

public class SemanticAnalyzer
{
    public void process()
    {
        /*foreach (var list in InputOutput.lexemes)
        {
            Console.WriteLine($"\n*");

            foreach (var lexeme in list)
            {
                Console.Write($"{lexeme}\t");
            }

        }*//*
        Console.WriteLine($"\n-");

        foreach (var ident in InputOutput.declaredIdents)
        {
            Console.Write($"{ident}\t");
        }Console.WriteLine($"\n+");

        foreach (var type in InputOutput.declaredTypes)
        {
            Console.Write($"{type}\t");
        }
        */

        var lexemeSegment = 0;
        foreach (var lexeme in InputOutput.lexemes[lexemeSegment])
        {
            if (lexeme == Lexemes.ident)
                InputOutput.declaredIdents.Dequeue();
        }

        lexemeSegment++;
        processVarBlock(InputOutput.lexemes[lexemeSegment++]);
        
        /*
        Console.WriteLine($"\n$");

        foreach (var VARIABLE in InputOutput.varTypes)
        {
            Console.Write($"{VARIABLE.Key}: {VARIABLE.Value};\n");
        }*/
        
        processBlock(InputOutput.lexemes[lexemeSegment]);
    }
    
    private bool CommaNext(Queue<byte> lexemes)
    {
        var o = lexemes.Peek() == Lexemes.comma;
        if (o)
            lexemes.Dequeue();
        return o;
    }    private bool SemicolonNext(Queue<byte> lexemes)
    {
        var o = lexemes.Peek() == Lexemes.semicolon;
        if (o)
            lexemes.Dequeue();
        return o;
    }

    void processVarBlock(Queue<byte> lexemes)
    {
        lexemes.Dequeue(); // var

        do
        {
            var matchingType = new Queue<string>();
            do
            {
                matchingType.Enqueue(InputOutput.declaredIdents.Dequeue());
                lexemes.Dequeue();
            } while (CommaNext(lexemes));

            lexemes.Dequeue(); // : 
            switch (lexemes.Dequeue())
            {
                case Lexemes.typesy:
                    var type = InputOutput.declaredTypes.Dequeue();
                    foreach (var ident in matchingType)
                    {
                        if (InputOutput.varTypes.TryGetValue(ident, out var unused))
                            Console.WriteLine($"Переменная {ident} определена повторно");
                        else
                        {
                            InputOutput.varTypes[ident] = new(type);
                        }
                    }

                    break;
                case Lexemes.recordsy:
                    Dictionary<string, byte> recordFields = new();
                    do
                    {
                        var matchingRecordType = new Queue<string>();
                        do
                        {
                            matchingRecordType.Enqueue(InputOutput.declaredIdents.Dequeue());
                            lexemes.Dequeue();
                        } while (CommaNext(lexemes));

                        lexemes.Dequeue(); // :
                        lexemes.Dequeue(); // typesy
                        var typeField = InputOutput.declaredTypes.Dequeue();
                        foreach (var field in matchingRecordType)
                        {
                            if (recordFields.TryGetValue(field, out var unused))
                                Console.WriteLine($"Поле {field} определено повторно");
                            else
                            {
                                recordFields[field] = typeField;
                            }
                            
                        }
                    } while (lexemes.Dequeue() == Lexemes.semicolon);
                    
                    foreach (var ident in matchingType)
                    {
                        if (InputOutput.varTypes.TryGetValue(ident, out var unused))
                            Console.WriteLine($"Переменная {ident} определена повторно");
                        else
                        {
                            InputOutput.varTypes[ident] = new(new Dictionary<string,byte>(recordFields));
                        }
                    }

                    break;
                default:
                    Console.WriteLine("%%%err");
                    break;
            }

            lexemes.Dequeue(); // ;
        } while (lexemes.Count > 0);
    }

    void processBlock(Queue<byte> lexemes)
    {
     processBlock(lexemes, null);   
    }
    void processBlock(Queue<byte> lexemes,  Dictionary<string, VariableType>? varScope)
    {
        var scope = varScope ?? InputOutput.varTypes;
        do
        {
            switch (lexemes.Dequeue())
            {
                case Lexemes.ident:
                    lexemes.Dequeue(); // :=
                    var type = lexemes.Dequeue();
                    var ident = InputOutput.declaredIdents.Dequeue();
                    if (!scope!.TryGetValue(ident, out var variableType))
                    {
                        Console.WriteLine($"\nИспользуемая переменная {ident} не объявлена!");
                    }
                    else if(type != variableType.simpleType)
                    {
                        Console.WriteLine($"\nНельзя присвоить тип {type} к переменной {ident} типа {variableType.simpleType}");
                    }

                    lexemes.Dequeue(); // ;
                    break;
                case Lexemes.withsy:
                    lexemes.Dequeue();  // ident
                    ident = InputOutput.declaredIdents.Dequeue();
                    if(scope![ident].recordType == null)
                        Console.WriteLine($"\nНельзя использовать оператор присоеденения над простой переменной {ident}");
                    else
                    {
                        var localScope = new Dictionary<string, VariableType>(scope);
                        foreach (var field in scope[ident].recordType!)
                        {
                            localScope[field.Key] = new(field.Value);
                        }

                        processBlock(lexemes, localScope);
                    }
                    //lexemes.Dequeue();  // do
                    break;
            }
        } while (lexemes.Count > 0);
    }
}