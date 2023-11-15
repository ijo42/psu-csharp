namespace Компилятор;

struct Err
{
    public TextPosition errorPosition;
    public string errorCode;

    public Err(TextPosition errorPosition, string errorCode)
    {
        this.errorPosition = errorPosition;
        this.errorCode = errorCode;
    }
}