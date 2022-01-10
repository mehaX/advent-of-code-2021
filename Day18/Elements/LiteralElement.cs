using Day18.Values;

namespace Day18.Elements;

public class LiteralElement : Element
{
    public LiteralValue Data { get; set; }
    
    public override bool IsLiteral()
    {
        return true;
    }

    public LiteralElement(int value, int depth) : base(depth)
    {
        Data = new LiteralValue(value);
    }

    public LiteralElement(LiteralValue value, int depth) : base(depth)
    {
        Data = value;
    }
}