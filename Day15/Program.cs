
static List<(int r, int c)> AdjacentNodes(int r, int c)
{
    return new List<(int r, int c)>
    {
        (r + 1, c),
        (r - 1, c),
        (r, c + 1),
        (r, c - 1),
    };
}

static int CalculateRisk(int[,] graph)
{
    var totalRows = graph.GetLength(0);
    var totalCols = graph.GetLength(1);

    var queue = new List<(int value, (int r, int c) point)> { (0, (0, 0)) };
    var locked = new List<(int r, int c)>();
    var result = 0;
    
    while (queue.Any())
    {
        var (risk, pos) = queue.First();
        queue.RemoveAt(0);
        
        if (pos.r == totalRows - 1 && pos.c == totalCols - 1)
        {
            result = risk;
            break;
        }

        if (locked.Contains(pos))
        {
            continue;
        }
        locked.Add(pos);
        
        foreach (var (r, c) in AdjacentNodes(pos.r, pos.c))
        {
            if (r >= 0 && r < totalRows && c >= 0 && c < totalCols)
            {
                queue.Add((risk + graph[r, c], (r, c)));
            }
        }
        queue.Sort((a, b) => a.value < b.value ? -1 : 1);
    }

    return result;
}

static void Part1()
{
    var input = ReadInput();
    var result = CalculateRisk(input);

    Console.WriteLine($"Part1: {result}");
}

static void Part2()
{
    var input = ReadInput();
    var totalRows = input.GetLength(0);
    var totalCols = input.GetLength(1);
    
    var graph = new int[input.GetLength(0) * 5, input.GetLength(1) * 5];

    for (int row = 0; row < graph.GetLength(0); row++)
    {
        for (int col = 0; col < graph.GetLength(1); col++)
        {
            var originalValue = input[row % totalRows, col % totalCols];
            var newValue = originalValue + row / totalRows + col / totalCols;
            newValue = (newValue - 1) % 9 + 1; // between 1-9
            graph[row, col] = newValue;
        }
    }
    
    var result = CalculateRisk(graph);

    Console.WriteLine($"Part2: {result}");
}


static int[,] ReadInput()
{
    var input = File.ReadLines("input.txt").ToList();
    var matrix = new int[input.Count, input[0].Length];

    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        for (var col = 0; col < matrix.GetLength(1); col++)
        {
            matrix[row, col] = input[row][col] - '0';
        }
    }

    return matrix;
}

Part1();
Part2();

