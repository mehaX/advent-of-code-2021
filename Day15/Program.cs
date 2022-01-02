
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

static void Part1()
{
    var matrix = ReadInput();
    var totalRows = matrix.GetLength(0);
    var totalCols = matrix.GetLength(1);

    var queue = new List<(int value, (int r, int c) point)> { (0, (0, 0)) };
    var locked = new List<(int r, int c)>();
    var result = 0;
    
    while (queue.Any())
    {
        var (risk, pos) = queue.First();
        queue.RemoveAt(0);
        
        result = risk;
        if (pos.r == totalRows - 1 && pos.c == totalCols - 1)
        {
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
                queue.Add((risk + matrix[r, c], (r, c)));
            }
        }
        queue.Sort((a, b) => a.value < b.value ? -1 : 1);
    }

    Console.WriteLine($"Part1: {result}");
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

