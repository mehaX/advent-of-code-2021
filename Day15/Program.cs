using Path = System.Collections.Generic.Dictionary<Point, Point>;
using Previous = System.Collections.Generic.Dictionary<Node, Node>;
using Nodes = System.Collections.Generic.List<Node>;
using EdgeWeights = System.Collections.Generic.List<EdgeWeight>;

static (int distance, Previous previous) BreadthFirstSearch(int[,] matrix, Node startNode, Node stopNode)
{
    var previous = new Previous();
    var visited = new Nodes();
    var queue = new EdgeWeights();
    visited.Add(startNode);
    queue.Add(new EdgeWeight(startNode, 0));

    while (queue.Any())
    {
        var ew = queue.First();
        queue.RemoveAt(0);
        
        if (ew.Neighbour == stopNode)
        {
            return (ew.Distance, previous);
        }

        var neighbours = AdjacentNodes(matrix, ew.Neighbour);
        foreach (var neighbour in neighbours)
        {
            if (!visited.Contains(neighbour))
            {
                previous[neighbour] = ew.Neighbour;
                queue.Add(new (neighbour, ew.Distance + 1));
                visited.Add(neighbour);
            }
        }
    }

    return (-1, previous);
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

static void Part1()
{
    var matrix = ReadInput();

    var startNode = new Node(new(0, 0), matrix[0, 0]);
    var stopNode = new Node(new(matrix.GetLength(1) - 1, matrix.GetLength(0) - 1),
        matrix[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1]);
    var result = BreadthFirstSearch(matrix, startNode, stopNode);
    
    var path = new Nodes();
    var n = stopNode;
    do
    {
        path.Add(n);
        n = result.previous[n];
    } while (n != startNode);

    var risk = path.Select(p => p.Value).Sum();

    Console.WriteLine($"Part1: {risk}");
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
