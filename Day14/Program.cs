
static string Part(int maxSteps = 1)
{
    ReadInput(out var template, out var pairInsertion);

    var result = "" + template;
    for (int step = 0; step < maxSteps; step++)
    {
        var newPairs = "";
        for (var index = 0; index < result.Length - 1; index++)
        {
            var pair = result.Substring(index, 2);
            var newElements = GetElementFromPair(pair, pairInsertion);
            newPairs += $"{pair[0]}{newElements}";
        }

        newPairs += result.Last();

        result = newPairs;
    }

    return result;
}

static char GetElementFromPair(string pair, List<(string, char)> pairInsertion)
{
    return pairInsertion.First(p => p.Item1 == pair).Item2;
}

static void ReadInput(out string template, out List<(string, char)> pairInsertion)
{
    var input = File.ReadAllText("input.txt").Split("\n\n");

    template = input[0];

    pairInsertion = input[1].Split("\n").Select(row =>
    {
        var segments = row.Split(" -> ");
        return (segments[0], segments[1][0]);
    }).ToList();
}

static void Part1()
{
    var elements = Part(10);

    Dictionary<char, int> counts = new();
    foreach (var element in elements.ToArray())
    {
        if (counts.ContainsKey(element))
        {
            counts[element]++;
        }
        else
        {
            counts.Add(element, 1);
        }
    }

    var highest = counts.Max(e => e.Value);
    var lowest = counts.Min(e => e.Value);
    var result = highest - lowest;
    Console.WriteLine($"Part1 result: {result}");
}

Part1();
