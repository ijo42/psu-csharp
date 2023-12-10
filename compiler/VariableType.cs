namespace Компилятор;

public struct VariableType
{
    public byte? simpleType;
    public Dictionary<string, byte>? recordType;

    public VariableType(Dictionary<string, byte>? recordType) : this()
    {
        this.simpleType = null;
        this.recordType = recordType;
    }
    public VariableType(byte? simpleType) : this()
    {
        this.simpleType = simpleType;
        this.recordType = null;
    }

    public override string ToString()
    {
        if (simpleType != null)
            return $"{simpleType}";
        else
            return recordType.Aggregate("record ", (current, field) => current + $"{field.Key} - {field.Value}\t");
    }
}