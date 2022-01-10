
using Day18;
using Day18.Elements;
using Day18.Values;

var input = File.ReadLines("input.txt").ToList();
var inputs = new List<PairValue>();
var allElements = new List<List<Element>>();

foreach (var s in input)
{
    var index = 1;
    var elements = new List<Element>();
    var root = Deserializer.DeserializePair(s, 1, ref index, ref elements);
    elements.Add(new PairElement(root, 1));
    inputs.Add(root);
    allElements.Add(elements);
}

while (inputs.Count > 1)
{
    var elements = allElements[0];
    var a = inputs[0];
    var b = inputs[1];

    inputs[0] = Actions.Addition(a, b, ref elements, allElements[1]);
    inputs.RemoveAt(1);
    allElements.RemoveAt(1);
   
    Print(elements, "After addition:\t\t");
    
    int toSplit = -1;
    var toExplode = -1;

    while((toSplit = elements.FindIndex(e => e.IsLiteral() && (e as LiteralElement).Data.Value >= 10)) != -1
          || (toExplode = elements.FindIndex(e => e.IsPair() && (e as PairElement).Depth > 4 && (e as PairElement).Data.ContainsLiterals())) != -1)
    {
        if (toSplit != -1)
        {
            Console.Write($"After split {(elements[toSplit] as LiteralElement).Data.Value}:\t\t");
            Actions.Split(toSplit, ref elements);
            toSplit = -1;
        }

        if (toExplode != -1)
        {
            Console.Write($"After explode ({((elements[toExplode] as PairElement).Data.X as LiteralValue).Value},{((elements[toExplode] as PairElement).Data.Y as LiteralValue).Value}):\t");
            Actions.Explode(toExplode, ref elements);
            toExplode = -1;
        }

        Print(elements);
    }
    allElements[0] = elements;
   
    Print(elements, "Result:\t\t\t");
}

static void Print(List<Element> elements, string prefix = "")
{
    Console.Write(prefix + string.Join(",", elements.Where(e => e.IsLiteral()).Select(e => (e as LiteralElement).Data.Value)));
    Console.Write("\t\t");
    foreach (var element in elements)
    {
        if (element.IsLiteral())
        {
            Console.Write($"({element.Depth})");
        }
        else
        {
            Console.Write($"[{element.Depth}]");
        }
    }
    Console.WriteLine();
}
