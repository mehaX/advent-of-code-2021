
static void Part1()
{
    var commands = File.ReadLines("input-1.txt");

    int h = 0;
    int d = 0;

    foreach (var command in commands)
    {
        var segments = command.Split(" ");
        var c = segments[0];
        var v = Convert.ToInt32(segments[1]);

        switch (c)
        {
            case "forward":
                h += v;
                break;
            
            case "up":
                d -= v;
                break;
            
            case "down":
                d += v;
                break;
        }
    }

    var result = h * d;

    Console.WriteLine($"Result for part 1 {result}");
}

Part1();