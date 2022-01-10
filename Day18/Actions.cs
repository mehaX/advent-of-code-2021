using Day18.Elements;
using Day18.Values;

namespace Day18;

public class Actions
{
    public static PairValue Addition(PairValue pair1, PairValue pair2, ref List<Element> elements, List<Element> secondElements)
    {
        var newPairValue = new PairValue();
        newPairValue.X = pair1;
        newPairValue.Y = pair2;
        elements.AddRange(secondElements);
        
        for (var i = 0; i < elements.Count; i++)
        {
            elements[i].Depth++;
        }
        
        elements.Add(new PairElement(newPairValue, 1));

        return newPairValue;
    }

    public static void Explode(int index, ref List<Element> elements)
    {
        var pairElement = elements[index];
        var newDepth = pairElement.Depth;
    
        var x = (pairElement as PairElement).Data.X as LiteralValue;
        var y = (pairElement as PairElement).Data.Y as LiteralValue;
    
        var xIndex = elements.FindIndex(e => e.IsLiteral() && (e as LiteralElement).Data == x);
        var yIndex = elements.FindIndex(e => e.IsLiteral() && (e as LiteralElement).Data == y);

        var leftLiteral = elements.Take(xIndex).LastOrDefault(e => e.IsLiteral());
        var rightLiteral = elements.Skip(yIndex + 1).FirstOrDefault(e => e.IsLiteral());
        if (leftLiteral != null)
        {
            (leftLiteral as LiteralElement).Data.Value += x.Value;
        }
        if (rightLiteral != null)
        {
            (rightLiteral as LiteralElement).Data.Value += y.Value;
        }
    
        elements[index] = new LiteralElement(0, newDepth);
        elements.RemoveAt(yIndex);
        elements.RemoveAt(xIndex);
    }

    public static void Split(int index, ref List<Element> elements)
    {
        var literalElement = elements[index] as LiteralElement;
        var newDepth = literalElement.Depth;

        var xLiteral = new LiteralValue(literalElement.Data.Value / 2);
        var yLiteral = new LiteralValue(literalElement.Data.Value - xLiteral.Value);

        var pairValue = new PairValue();
        pairValue.X = xLiteral;
        pairValue.Y = yLiteral;

        elements[index] = new PairElement(pairValue, newDepth);
        elements.Insert(index + 1, new LiteralElement(xLiteral, newDepth + 1));
        elements.Insert(index + 2, new LiteralElement(yLiteral, newDepth + 1));
    }
}
