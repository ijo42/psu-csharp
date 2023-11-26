namespace Компилятор;

struct Err
{
    public TextPosition errorPosition;
    public string errorCode;
    public string? desc;

    public Err(TextPosition errorPosition, string errorCode, string? desc)
    {
        this.errorPosition = errorPosition;
        this.errorCode = errorCode;
        this.desc = desc;
    }
}