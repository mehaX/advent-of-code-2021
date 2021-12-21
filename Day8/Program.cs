
static void Part1()
{
    var input = File.ReadLines("input.txt");
    var outputCount = 0;
    var filteredLengths = new[] { 2, 4, 3, 7 }; // only for 1, 4, 7, 8

    foreach (var row in input)
    {
        var output = row.Split(" | ")[1];
        foreach (var number in output.Split(" "))
        {
            if (filteredLengths.Contains(number.Length))
            {
                outputCount++;
            }
        }
    }

    Console.WriteLine($"Part1 result: {outputCount}");
}

Part1();
