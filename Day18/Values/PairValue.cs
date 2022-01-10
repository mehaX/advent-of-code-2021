namespace Day18.Values;

public class PairValue : IElementValue
{
    public IElementValue X { get; set; }
    public IElementValue Y { get; set; }

    public bool ContainsLiterals()
    {
        return X is LiteralValue && Y is LiteralValue;
    }
}