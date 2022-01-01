using Path = System.Collections.Generic.Dictionary<Point, Point>;
using Previous = System.Collections.Generic.Dictionary<Node, Node>;
using Nodes = System.Collections.Generic.List<Node>;
using EdgeWeights = System.Collections.Generic.List<EdgeWeight>;

static void ShortestPathBidirectionalSearch(int[,] matrix, ref EdgeWeights queue, ref EdgeWeights visitedOwn,
    EdgeWeights visitedOther, ref Previous previous, bool isBackwards)
{
    if (!queue.Any())
    {
        return;
    }

    var ew = queue.First();
    queue.RemoveAt(0);
    var dist = ew.Distance;

    var touchNode = visitedOther.FirstOrDefault(v => v.Neighbour.Equals(ew.Neighbour));
    if (touchNode != null)
    {
        return;
    }

    var adjList = AdjacentNodes(matrix, ew.Neighbour);
    foreach (var neighbour in adjList)
    {
        if (visitedOwn.FindIndex(d => d.Neighbour.Equals(neighbour)) == -1)
        {
            if (isBackwards)
            {
                previous.TryAdd(ew.Neighbour, neighbour);
            }
            else
            {
                previous.TryAdd(neighbour, ew.Neighbour);
            }
            queue.Add(new(neighbour, dist + 1));
            visitedOwn.Add(new(neighbour, dist + 1));
        }
    }
}

static (int distance, Nodes path) BidirectionalSearch(int[,] matrix, Node startNode, Node stopNode)
{
    var previous = new Previous();
    var visited1 = new EdgeWeights();
    var visited2 = new EdgeWeights();
    var queue1 = new EdgeWeights();
    var queue2 = new EdgeWeights();
    var ew1 = new EdgeWeight(startNode, 0);
    var ew2 = new EdgeWeight(stopNode, 0);
    queue1.Add(ew1);
    queue2.Add(ew2);
    visited1.Add(ew1);
    visited2.Add(ew2);

    while (queue1.Any() || queue2.Any())
    {
        ShortestPathBidirectionalSearch(matrix, ref queue1, ref visited1, visited2, ref previous, false);
        ShortestPathBidirectionalSearch(matrix, ref queue2, ref visited2, visited1, ref previous, true);
    }

    var path = new Nodes();
    var node = stopNode;
    do
    {
        path.Add(node);
        node = previous[node];
    } while (node != startNode);

    path.Reverse();

    return (path.Select(p => p.Value).Sum(), path);
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
    var result = BidirectionalSearch(matrix, startNode, stopNode);

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
