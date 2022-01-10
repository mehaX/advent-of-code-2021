using Day18.Values;

namespace Day18.Elements;

public class PairElement : Element
{
    public PairValue Data { get; set; }
    
    public override bool IsLiteral()
    {
        return false;
    }

    public PairElement(PairValue value, int depth) : base(depth)
    {
        Data = value;
    }
}