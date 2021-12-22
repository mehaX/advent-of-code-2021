
using Edges = System.Collections.Generic.List<(string, string)>;
using Nodes = System.Collections.Generic.List<string>;

static void Part1()
{
    var edges = GetEdges();
    var startNode = "start";
    var solutions = new List<Edges>();
    var track = new Edges();
    
    FindPath(edges, startNode, ref track, ref solutions);
    var result = solutions.Count;

    Console.WriteLine($"Part1: {result}");
}

static Edges GetEdges()
{
    var input = File.ReadLines("input.txt");
    Edges edges = new();
    
    foreach (var s in input)
    {
        var nodes = s.Split("-");
        edges.Add((nodes[0], nodes[1]));
    }

    return edges;
}

static void FindPath(Edges edges, string sourceNode, ref Edges track, ref List<Edges> solutions)
{
    var targetNodes = GetConnectedNodes(edges, sourceNode, track);
    
    foreach (var targetNode in targetNodes)
    {
        var newList = track.ToList(); // clone
        newList.Add((sourceNode, targetNode));
        if (targetNode.IsEnd())
        {
            solutions.Add(newList);
        }
        else
        {
            FindPath(edges, targetNode, ref newList, ref solutions);
        }
    }
}

static Nodes GetConnectedNodes(Edges edges, string node, Edges track)
{
    var connectedNodes = edges
        .Where(e => e.HasConnection(node))
        .Select(e => e.TargetNode(node))
        .Where(n => !n.IsSmallCave() || !track.IsInTrack(n))
        .ToList();

    return connectedNodes;
}

Part1();


public static class Extensions
{
    public static bool IsBigCave(this string cave)
    {
        return cave[0] >= 'A' && cave[0] <= 'Z';
    }
    
    public static bool IsSmallCave(this string cave)
    {
        return cave[0] >= 'a' && cave[0] <= 'z';
    }

    public static bool IsStart(this string cave)
    {
        return cave == "start";
    }

    public static bool IsEnd(this string cave)
    {
        return cave == "end";
    }

    public static bool HasConnection(this (string, string) edge, string node)
    {
        return edge.Item1 == node || edge.Item2 == node;
    }

    public static string TargetNode(this (string, string ) edge, string sourceNode)
    {
        return edge.Item1 == sourceNode ? edge.Item2 : edge.Item1;
    }

    public static bool IsInTrack(this Edges track, string node)
    {
        return track.Any(edge => edge.HasConnection(node));
    }
}