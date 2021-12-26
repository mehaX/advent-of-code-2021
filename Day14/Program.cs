
static void Part(int maxSteps = 1)
{
    var time = DateTime.Now;
    ReadInput(out var template, out var pairInsertion);
    var counts = string.Join("", pairInsertion.Select(p => p.Item1)).ToCharArray().Distinct().ToDictionary(c => c, _ => (long)0);
    
    for (var i = 0; i < template.Length; i++)
    {
        counts[template[i]]++;
    }
    for (var i = 0; i < template.Length - 1; i++)
    {
        CalculateDepth(template.Substring(i, 2).ToCharArray(), 1, maxSteps - 1, pairInsertion, ref counts);
    }

    var highest = counts.Max(e => e.Value);
    var lowest = counts.Min(e => e.Value);
    var output = highest - lowest;
    var timestamp = DateTime.Now.Subtract(time).TotalSeconds;
    Console.WriteLine($"Result with {maxSteps}: {output} {timestamp}, steps: " + string.Join(", ", counts.Select(kvp => kvp.Key + "=" + kvp.Value)));
}

static void CalculateDepth(char[] pair, int depth, int maxDepth, List<(string, char)> pairInsertion, ref Dictionary<char, long> counts)
{
    var newElement = GetElementFromPair(pair, pairInsertion);
    counts[newElement]++;

    if (depth <= maxDepth)
    {
        CalculateDepth(new []{pair[0], newElement}, depth + 1, maxDepth, pairInsertion, ref counts);
        CalculateDepth(new[]{newElement, pair[1]}, depth + 1, maxDepth, pairInsertion, ref counts);
    }
}

static char GetElementFromPair(char[] pair, List<(string, char)> pairInsertion)
{
    return pairInsertion.First(p => p.Item1[0] == pair[0] && p.Item1[1] == pair[1]).Item2;
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
    for (int steps = 2; steps <= 40; steps++)
    {
        Part(steps);
    }
}
static void Part2()
{
    Part(40);
}

Part1();
Part2();
