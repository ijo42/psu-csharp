namespace Компилятор;

struct TextPosition
{
    public uint lineNumber; // номер строки
    public byte charNumber; // номер позиции в строке

    public TextPosition(uint ln = 0, byte c = 0)
    {
        lineNumber = ln;
        charNumber = c;
    }
}