namespace Компилятор
{
    class SyntaxAnalyzer
    {
        private readonly Queue<LexemeCoord> _lexemes;

        public SyntaxAnalyzer(Queue<LexemeCoord> lexemes)
        {
            _lexemes = lexemes;
        }

        TextPosition position()
        {
            return _lexemes.Peek().Position;
        }

        byte token()
        {
            try
            {
                return _lexemes.Peek().Lexem;
            }
            catch
            {
                return 255;
            }
        }

        LexemeCoord nextsym()
        {
            return _lexemes.Dequeue();
        }

        void accept(byte expectedtoken)
        {
            accept(expectedtoken, new HashSet<byte>());
        }
        void accept(byte expectedtoken, HashSet<byte> suitable)
        {
            if(token() == 255)
                return;
            if (token() != expectedtoken)
            {
                error(expectedtoken, token());
                if(suitable.Count > 0)
                    skip(suitable);
            }
            if(_lexemes.Count > 0)
                nextsym();
        }

        private bool commaNext()
        {
            var o = token() == Lexemes.comma;
            if (o)
                nextsym();
            return o;
        }

        void error(byte expectedToken, byte token)
        {
            InputOutput.Error(203, position());
            InputOutput.printSyntaxError();
            Console.WriteLine($"\r Ожидалось {expectedToken}, найдено {token}");
        }

        void skip(HashSet<byte> suitable)
        {
            int i;
            for (i = 0; !suitable.Contains(token()) && _lexemes.Count > 0; i++)
            {
                nextsym();
            }
            Console.WriteLine($"% Пропущено {i} лексемы для продолжения работы");
        }

        public void programme()
        {
            accept(Lexemes.programsy);
            accept(Lexemes.ident);
            accept(Lexemes.semicolon);
            block();
            accept(Lexemes.point);
        }

        public void block()
        {
            labelpart();
            constpart();
            typepart();
            varpart();
            // procfuncpart();
            // functionpart()
            statementpart();
        }

        void statementpart()
        {
            accept(Lexemes.beginsy, new HashSet<byte> {Lexemes.beginsy});

            do
            {
                statement();
                if(token() is Lexemes.endsy or 255)
                    break;
                accept(Lexemes.semicolon);
            } while (true);

            accept(Lexemes.endsy);
        }

        void statement() // оператор
        {
            switch (token())
            {
                case Lexemes.ident:
                    assignpart();
                    break;
                case Lexemes.beginsy:
                    accept(Lexemes.beginsy);
                    do
                    {
                        statement();
                        if (token() == Lexemes.endsy)
                            break;
                        accept(Lexemes.semicolon);
                    } while (true);

                    accept(Lexemes.endsy);
                    break;
                case Lexemes.forsy:
                    accept(Lexemes.forsy);
                    forstatement();
                    break;
                case Lexemes.withsy:
                    accept(Lexemes.withsy, new HashSet<byte>{Lexemes.withsy, Lexemes.ident});
                    withpart();
                    break;
            }
        }

        private void withpart()
        {
            do {
                accept(Lexemes.ident);
            } while (commaNext());
            accept(Lexemes.dosy);
            statement();
        }

        void assignpart()
        {
            accept(Lexemes.ident);
            accept(Lexemes.assign);
            expression();
        }

        readonly List<byte> relations = new()
        {
            Lexemes.equal, Lexemes.greater, Lexemes.later,
            Lexemes.greaterequal, Lexemes.laterequal, Lexemes.latergreater, Lexemes.insy
        };
        void expression()
        {
            simpleExpression();
            if (relations.Contains(token()))
            {
                nextsym();
                simpleExpression();
            }
        }

        void simpleExpression()
        {
            if (token() is Lexemes.plus or Lexemes.minus)
                nextsym();
            summingpart();
            while (token() is Lexemes.plus or Lexemes.minus or Lexemes.orsy)
            {
                nextsym();
                summingpart();
            }
        }

        void summingpart()
        {
            multiplierpart();
            while (token() is Lexemes.star or Lexemes.slash or Lexemes.divsy or Lexemes.modsy or Lexemes.andsy)
            {
                nextsym();
                multiplierpart();
            }
        }

        private void multiplierpart()
        {
            switch (token())
            {
                case Lexemes.lbracket:
                    accept(Lexemes.lbracket);
                    if(token() == Lexemes.rbracket)
                        accept(Lexemes.rbracket);
                    else
                    {
                        do
                        {
                            expression();
                            if (token() == Lexemes.twopoints)
                            {
                                expression();
                            }
                        } while (commaNext());
                        accept(Lexemes.rbracket);
                    }
                    break;
                case Lexemes.notsy:
                    accept(Lexemes.notsy);
                    expression();
                    break;
                case Lexemes.leftpar:
                    accept(Lexemes.leftpar);
                    expression();
                    accept(Lexemes.rightpar);
                    break;
                case Lexemes.ident:
                    accept(Lexemes.ident);
                    break;
                case Lexemes.intc:
                    accept(Lexemes.intc);
                    break;
                case Lexemes.floatc:
                    accept(Lexemes.floatc);
                    break;
            }
        }

        void forstatement()
        {
            accept(Lexemes.forsy);
            accept(Lexemes.ident);
            accept(Lexemes.assign);
            expression();
            if (token()==Lexemes.tosy || token() == Lexemes.downtosy)
                nextsym();
            expression();
            accept(Lexemes.dosy);
            statement();
        }
        void whilestatement()
        {
            accept(Lexemes.whilesy);
            expression();
            accept(Lexemes.dosy);
            statement();
        }

        void compoundstatement()
        {
            accept(Lexemes.beginsy);
            statement();
            while (token() == Lexemes.semicolon)
            {
                nextsym();
                statement();
            }
            accept(Lexemes.endsy);
        }

        void typepart()
        {
            if (token() == Lexemes.typesy)
            {
                accept(Lexemes.typesy);
                do
                {
                    typedec();
                    accept(Lexemes.semicolon);
                } while (token() == Lexemes.ident);
            }
        }

        void typedec()
        {
            accept(Lexemes.ident);
            accept(Lexemes.equal);
            type();
        }
        
        void varpart()
        {
            if (token() == Lexemes.varsy)
            {
                accept(Lexemes.varsy);
                do
                {
                    vardeclaration();
                    accept(Lexemes.semicolon);
                } while (token() == Lexemes.ident);
            }
        }

        private readonly HashSet<byte> varContinue = new()
        {
            Lexemes.colon, Lexemes.ident, Lexemes.comma
        };

        void vardeclaration()
        {
            do
            {
                accept(Lexemes.ident, varContinue);
            } while (commaNext());
            accept(Lexemes.colon, varContinue);
            type();
        }

        void type()
        {
           if(simpleType()) return;
           if(compoundType()) return;
           refType();
        }
        
        bool simpleType()
        {
            var flag = false;
            switch (token())
            {
                case Lexemes.leftpar:
                    // enum type
                    accept(Lexemes.leftpar);
                    accept(Lexemes.ident);
                    while(commaNext())
                    {
                        accept(Lexemes.ident);
                    }
                    accept(Lexemes.rightpar);
                    flag = true;

                break;
                case Lexemes.floatc:
                    // Limited type
                    accept(Lexemes.floatc);
                    accept(Lexemes.twopoints);
                    accept(Lexemes.floatc);
                    flag = true;
                    break;
                case Lexemes.intc:
                    // Limited type
                    accept(Lexemes.intc);
                    accept(Lexemes.twopoints);
                    accept(Lexemes.intc);
                    flag = true;
                break;
                case Lexemes.ident:
                    // type name
                    accept(Lexemes.ident);
                    flag = true;
                    break;
            }

            return flag;
        }

        bool compoundType()
        {
            if(token() == Lexemes.packedsy)
                accept(Lexemes.packedsy);
            switch (token())
            {
                case Lexemes.arraysy:
                    break;
                case Lexemes.recordsy:
                    accept(Lexemes.recordsy);
                    if (token() == Lexemes.casesy)
                    {
                        
                    }
                    else
                    {
                        do
                        {
                            vardeclaration();
                        } while (token() == Lexemes.semicolon);
                    }
                    accept(Lexemes.endsy);
                    break;
                case Lexemes.setsy:
                    break;
                case Lexemes.filesy:
                    break;
            }
            return false;
        }

        bool refType()
        {
            if (token() == Lexemes.arrow)
            {
                accept(Lexemes.arraysy);
                accept(Lexemes.ident);
                return true;
            }
            return false;
        }

        void labelpart()
        {
            if(token() == Lexemes.labelsy)
            {
                accept(Lexemes.labelsy);
                do
                {
                    accept(Lexemes.intc);
                    if (token() != Lexemes.comma)
                        break;
                    accept(Lexemes.comma);
                } while (true);

                accept(Lexemes.semicolon);
            }
        }

        void constpart()
        {
            if(token() == Lexemes.constsy)
            {
                accept(Lexemes.constsy);
                do
                {
                    accept(Lexemes.ident);
                    accept(Lexemes.equal);
                    accept(Lexemes.intc); // or float or char
                    accept(Lexemes.semicolon);
                } while (token() == Lexemes.ident);
            }
        }
    }
}