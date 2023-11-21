namespace Компилятор;

public class ErrorCodes
{
    public static Dictionary<byte, string> Dictionary = new()
    {
        { 201, "Переполнение типа" },
        { 202, "Неизвестное имя"},
        { 203, "Неизвестная конструкция" }
    };
}