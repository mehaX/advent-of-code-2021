using Path = System.Collections.Generic.Dictionary<Point, Point>;
using Visited = System.Collections.Generic.Dictionary<Point, int>;


static (int distance, List<Node> path) BellmanFord(int[,] matrix, List<Node> nodes, Node startNode, Node stopNode, int maxNeighbours)
{
    var previous = new Dictionary<Node, Node>();
    var distances = nodes.ToDictionary(n => n, _ => int.MaxValue);
    distances[startNode] = 0;

    for (var i = 0; i < maxNeighbours - 1; i++)
    {
        // Console.Write($"Step {i}: ");
        var cTime = DateTime.Now;
        foreach (var node in nodes)
        {
            var neighbours = AdjacentNodes(matrix, node);
            var edgeWeights = EdgeWeights(neighbours);
            foreach (var neighbour in neighbours)
            {
                var newPathLength = distances[node] + edgeWeights[neighbour];
                var oldPathLength = distances[neighbour];
                if (newPathLength < oldPathLength)
                {
                    distances[neighbour] = newPathLength;
                    previous[neighbour] = node;
                }
            }
        }

        var time = DateTime.Now.Subtract(cTime).Milliseconds;
        // Console.WriteLine($"{time}ms");
    }
    
    // foreach (var node in nodes)
    // {
    //     var neighbours = AdjacentNodes(nodes, node);
    //     var edgeWeights = EdgeWeights(neighbours);
    //     foreach (var neighbour in neighbours)
    //     {
    //         if (distances[node] + edgeWeights[neighbour] < distances[neighbour])
    //         {
    //             throw new Exception("There is a cycle with negative weight");
    //         }
    //     }
    // }
    
    var n = stopNode;
    var path = new List<Node>();
    while (n != startNode)
    {
        path.Add(n);
        n = previous[n];
    }
    path.Add(startNode);
    path.Reverse();

    return (distances[stopNode], path);
}

static Node[] AdjacentNodes(int[,] matrix, Node node)
{
    var points = new Point[]
    {
        new(node.Point.X - 1, node.Point.Y),
        new(node.Point.X + 1, node.Point.Y),
        new(node.Point.X, node.Point.Y - 1),
        new(node.Point.X, node.Point.Y + 1),
    }.Where(p => p.X >= 0 && p.X < matrix.GetLength(1) && p.Y >= 0 && p.Y < matrix.GetLength(0));

    return points.Select(p => new Node(p, matrix[p.Y, p.X])).ToArray();
}

static Dictionary<Node, int> EdgeWeights(Node[] neighbours)
{
    return neighbours.ToDictionary(n => n, n => n.Value);
}

static void Part1()
{
    var matrix = ReadInput();
    var nodes = new List<Node>();
    
    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        for (var col = 0; col < matrix.GetLength(1); col++)
        {
            nodes.Add(new (new (col, row), matrix[row, col]));
        }
    }

    var startNode = nodes.First();
    var stopNode = nodes.Last();
    var maxNeighbours = (matrix.GetLength(0) - 2) * (matrix.GetLength(1) - 2) * 4
                        + (matrix.GetLength(0) - 2) * 2 * 3
                        + (matrix.GetLength(1) - 2) * 2 * 3
                        + 4 * 2;

    var result = BellmanFord(matrix, nodes, startNode, stopNode, maxNeighbours);

    Console.WriteLine($"Part1: {result.distance}");
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

// static int CalculateRisk(int[,] matrix, Path path)
// {
//     return path.Select(p => matrix[p.y, p.x]).Sum();
// }

Part1();

public record Point(int X, int Y)
{
    public virtual bool Equals(Point? other)
    {
        return other != null && other.X == X && other.Y == Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}

public record Node(Point Point, int Value)
{
    public virtual bool Equals(Node? other)
    {
        return Point.Equals(other?.Point);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Point, Value);
    }
}
public record EdgeWeight(Node Neighbour, int Distance);
