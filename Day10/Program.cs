
static void Part1()
{
    var input = File.ReadLines("input.txt");
    var open = new[] { '(', '[', '{', '<' };
    var close = new[] { ')', ']', '}', '>' };
    var scoreboard = new[] { 3, 57, 1197, 25137 };

    var score = 0;
    foreach (var line in input)
    {
        var stack = new Stack();
        foreach (var c in line.ToArray())
        {
            if (open.Contains(c))
            {
                stack.Add(c);
            }
            else if (close.Contains(c))
            {
                if (!stack.Matches(c))
                {
                    score += scoreboard[Array.IndexOf(close, c)];
                    break;
                }
                
                _ = stack.Remove();
            }
        }
    }

    Console.WriteLine($"Part1: {score}");
}

Part1();

class Stack
{
    private List<char> mContent = new();
    private static Dictionary<char, char> mMatches = new()
    {
        {'(', ')'},
        {'[', ']'},
        {'{', '}'},
        {'<', '>'},
    };

    public void Add(char c)
    {
        mContent.Add(c);
    }

    public char Remove()
    {
        var c = mContent.Last();
        mContent.RemoveAt(mContent.Count - 1);
        return c;
    }

    public bool Matches(char c)
    {
        var last = mContent.LastOrDefault();
        return mMatches.ContainsKey(last) && mMatches[last] == c;
    }
}
