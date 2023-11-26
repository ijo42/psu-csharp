namespace Компилятор;

public class LexemeCoord
{
    public byte Lexem { get; }
    public TextPosition Position { get; }

    public LexemeCoord(byte nextSym, TextPosition token)
    {
        this.Lexem = nextSym;
        Position = token;
    }
}