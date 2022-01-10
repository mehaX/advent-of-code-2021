namespace Day18.Values;

public class LiteralValue : IElementValue
{
    public int Value { get; set; }

    public LiteralValue(int value)
    {
        Value = value;
    }
}