
namespace Day18.Elements;

public abstract class Element
{
    public int Depth { get; set; }
    public abstract bool IsLiteral();

    public virtual bool IsPair()
    {
        return !IsLiteral();
    }

    public Element(int depth)
    {
        Depth = depth;
    }
}