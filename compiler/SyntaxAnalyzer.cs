namespace Компилятор
{
    class SyntaxAnalyzer
    {
        private readonly Func<byte> _lexicalNext;
        private byte _lexeme;
        private TextPosition _position;
        private string? errorMsg = null;
        public SyntaxAnalyzer(Func<byte> lexicalNext)
        {
            _lexicalNext = lexicalNext;
        }
        
        public void Process()
        {
            NextSym();
            Programme();
        }

        void NextSym()
        {
            _position = InputOutput.positionNow;
            _lexeme = InputOutput.isEnd ? (byte)255 : _lexicalNext();
        }

        void Accept(byte expectedtoken)
        {
            Accept(expectedtoken, new HashSet<byte>());
        }
        void Accept(byte expectedtoken, HashSet<byte> suitable)
        {
            if(_lexeme == 255)
                return;
            if (_lexeme != expectedtoken)
            {
                var l = _lexeme;
                var pos = _position;
                if(suitable.Count > 0)
                    Skip(suitable);
                Error(expectedtoken, l, pos);
            }
            NextSym();
        }

        private bool CommaNext()
        {
            var o = _lexeme == Lexemes.comma;
            if (o)
                NextSym();
            return o;
        }

        void Error(byte expectedToken, byte token, TextPosition position)
        {
            InputOutput.Error(203, position, $" - Ожидалось {expectedToken}, найдено {token}" +
                                              (errorMsg!=null ? $"{errorMsg}" : ""));
            errorMsg = null;
        }

        void Skip(HashSet<byte> suitable)
        {
            int i;
            for (i = 0; !suitable.Contains(_lexeme) && _lexeme != 255; i++)
            {
                NextSym();
            }
            errorMsg = $"\n% Пропущено {i} лексемы для продолжения работы";
        }

        public void Programme()
        {
            Accept(Lexemes.programsy);
            Accept(Lexemes.ident);
            Accept(Lexemes.semicolon);
            Block();
            Accept(Lexemes.point);
        }

        public void Block()
        {
            Labelpart();
            Constpart();
            Typepart();
            Varpart();
            // procfuncpart();
            // functionpart()
            Statementpart();
        }

        void Statementpart()
        {
            Accept(Lexemes.beginsy, new HashSet<byte> {Lexemes.beginsy});

            do
            {
                Statement();
                if(_lexeme is Lexemes.endsy or 255)
                    break;
                Accept(Lexemes.semicolon);
            } while (true);

            Accept(Lexemes.endsy);
        }

        void Statement() // оператор
        {
            switch (_lexeme)
            {
                case Lexemes.ident:
                    Assignpart();
                    break;
                case Lexemes.beginsy:
                    Accept(Lexemes.beginsy);
                    do
                    {
                        Statement();
                        if (_lexeme == Lexemes.endsy)
                            break;
                        Accept(Lexemes.semicolon);
                    } while (true);

                    Accept(Lexemes.endsy);
                    break;
                case Lexemes.forsy:
                    Accept(Lexemes.forsy);
                    Forstatement();
                    break;
                case Lexemes.withsy:
                    Accept(Lexemes.withsy, new HashSet<byte>{Lexemes.withsy, Lexemes.ident});
                    Withpart();
                    break;
            }
        }

        private void Withpart()
        {
            do {
                Accept(Lexemes.ident);
            } while (CommaNext());
            Accept(Lexemes.dosy);
            Statement();
        }

        void Assignpart()
        {
            Accept(Lexemes.ident);
            Accept(Lexemes.assign);
            Expression();
        }

        readonly List<byte> _relations = new()
        {
            Lexemes.equal, Lexemes.greater, Lexemes.later,
            Lexemes.greaterequal, Lexemes.laterequal, Lexemes.latergreater, Lexemes.insy
        };
        void Expression()
        {
            SimpleExpression();
            if (_relations.Contains(_lexeme))
            {
                NextSym();
                SimpleExpression();
            }
        }

        void SimpleExpression()
        {
            if (_lexeme is Lexemes.plus or Lexemes.minus)
                NextSym();
            Summingpart();
            while (_lexeme is Lexemes.plus or Lexemes.minus or Lexemes.orsy)
            {
                NextSym();
                Summingpart();
            }
        }

        void Summingpart()
        {
            Multiplierpart();
            while (_lexeme is Lexemes.star or Lexemes.slash or Lexemes.divsy or Lexemes.modsy or Lexemes.andsy)
            {
                NextSym();
                Multiplierpart();
            }
        }

        private void Multiplierpart()
        {
            switch (_lexeme)
            {
                case Lexemes.lbracket:
                    Accept(Lexemes.lbracket);
                    if(_lexeme == Lexemes.rbracket)
                        Accept(Lexemes.rbracket);
                    else
                    {
                        do
                        {
                            Expression();
                            if (_lexeme == Lexemes.twopoints)
                            {
                                Expression();
                            }
                        } while (CommaNext());
                        Accept(Lexemes.rbracket);
                    }
                    break;
                case Lexemes.notsy:
                    Accept(Lexemes.notsy);
                    Expression();
                    break;
                case Lexemes.leftpar:
                    Accept(Lexemes.leftpar);
                    Expression();
                    Accept(Lexemes.rightpar);
                    break;
                case Lexemes.ident:
                    Accept(Lexemes.ident);
                    break;
                case Lexemes.intc:
                    Accept(Lexemes.intc);
                    break;
                case Lexemes.floatc:
                    Accept(Lexemes.floatc);
                    break;
            }
        }

        void Forstatement()
        {
            Accept(Lexemes.forsy);
            Accept(Lexemes.ident);
            Accept(Lexemes.assign);
            Expression();
            if (_lexeme==Lexemes.tosy || _lexeme == Lexemes.downtosy)
                NextSym();
            Expression();
            Accept(Lexemes.dosy);
            Statement();
        }
        void Whilestatement()
        {
            Accept(Lexemes.whilesy);
            Expression();
            Accept(Lexemes.dosy);
            Statement();
        }

        void Compoundstatement()
        {
            Accept(Lexemes.beginsy);
            Statement();
            while (_lexeme == Lexemes.semicolon)
            {
                NextSym();
                Statement();
            }
            Accept(Lexemes.endsy);
        }

        void Typepart()
        {
            if (_lexeme == Lexemes.typesy)
            {
                Accept(Lexemes.typesy);
                do
                {
                    Typedec();
                    Accept(Lexemes.semicolon);
                } while (_lexeme == Lexemes.ident);
            }
        }

        void Typedec()
        {
            Accept(Lexemes.ident);
            Accept(Lexemes.equal);
            Type();
        }
        
        void Varpart()
        {
            if (_lexeme == Lexemes.varsy)
            {
                Accept(Lexemes.varsy);
                do
                {
                    Vardeclaration();
                    Accept(Lexemes.semicolon);
                } while (_lexeme == Lexemes.ident);
            }
        }

        private readonly HashSet<byte> _varContinue = new()
        {
            Lexemes.colon, Lexemes.ident, Lexemes.comma
        };

        void Vardeclaration()
        {
            do
            {
                Accept(Lexemes.ident, _varContinue);
            } while (CommaNext());
            Accept(Lexemes.colon, _varContinue);
            Type();
        }

        void Type()
        {
           if(SimpleType()) return;
           if(CompoundType()) return;
           RefType();
        }
        
        bool SimpleType()
        {
            var flag = false;
            switch (_lexeme)
            {
                case Lexemes.leftpar:
                    // enum type
                    Accept(Lexemes.leftpar);
                    Accept(Lexemes.ident);
                    while(CommaNext())
                    {
                        Accept(Lexemes.ident);
                    }
                    Accept(Lexemes.rightpar);
                    flag = true;

                break;
                case Lexemes.floatc:
                    // Limited type
                    Accept(Lexemes.floatc);
                    Accept(Lexemes.twopoints);
                    Accept(Lexemes.floatc);
                    flag = true;
                    break;
                case Lexemes.intc:
                    // Limited type
                    Accept(Lexemes.intc);
                    Accept(Lexemes.twopoints);
                    Accept(Lexemes.intc);
                    flag = true;
                break;
                case Lexemes.ident:
                    // type name
                    Accept(Lexemes.ident);
                    flag = true;
                    break;
            }

            return flag;
        }

        bool CompoundType()
        {
            if(_lexeme == Lexemes.packedsy)
                Accept(Lexemes.packedsy);
            switch (_lexeme)
            {
                case Lexemes.arraysy:
                    break;
                case Lexemes.recordsy:
                    Accept(Lexemes.recordsy);
                    if (_lexeme == Lexemes.casesy)
                    {
                        
                    }
                    else
                    {
                        do
                        {
                            Vardeclaration();
                        } while (_lexeme == Lexemes.semicolon);
                    }
                    Accept(Lexemes.endsy);
                    break;
                case Lexemes.setsy:
                    break;
                case Lexemes.filesy:
                    break;
            }
            return false;
        }

        bool RefType()
        {
            if (_lexeme == Lexemes.arrow)
            {
                Accept(Lexemes.arraysy);
                Accept(Lexemes.ident);
                return true;
            }
            return false;
        }

        void Labelpart()
        {
            if(_lexeme == Lexemes.labelsy)
            {
                Accept(Lexemes.labelsy);
                do
                {
                    Accept(Lexemes.intc);
                    if (_lexeme != Lexemes.comma)
                        break;
                    Accept(Lexemes.comma);
                } while (true);

                Accept(Lexemes.semicolon);
            }
        }

        void Constpart()
        {
            if(_lexeme == Lexemes.constsy)
            {
                Accept(Lexemes.constsy);
                do
                {
                    Accept(Lexemes.ident);
                    Accept(Lexemes.equal);
                    Accept(Lexemes.intc); // or float or char
                    Accept(Lexemes.semicolon);
                } while (_lexeme == Lexemes.ident);
            }
        }
    }
}