using Day18.Elements;
using Day18.Values;

namespace Day18;

public class Deserializer
{
    public static PairValue DeserializePair(string input, int depth, ref int index, ref List<Element> elements)
    {
        PairValue pair = new();
    
        pair.X = DeserializeElement(input, depth + 1, ref index, ref elements);
        index++; // escape `,`
    
        pair.Y = DeserializeElement(input, depth + 1, ref index, ref elements);
        index++; // escape `]`
    
        return pair;
    }

    public static IElementValue DeserializeElement(string input, int currentDepth, ref int index, ref List<Element> elements)
    {
        if (input[index] >= '0' && input[index] <= '9')
        {
            int length = 1;
            while (input[index + length] >= '0' && input[index + length] <= '9')
            {
                length++;
            }

            var number = Convert.ToInt32(input.Substring(index, length));

            index += length;
            var literalValue = new LiteralElement(number, currentDepth);
            elements.Add(literalValue);
            return literalValue.Data;
        }

        index++;
        var subPair = new PairElement(DeserializePair(input, currentDepth, ref index, ref elements), currentDepth);
        elements.Add(subPair);
        return subPair.Data;
    }

}