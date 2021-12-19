
static void Part1()
{
    var input = File.ReadLines("input.txt");
    var strGamma = "";
    var strEpsilon = "";

    var length = input.First().Length;
    for (var index = 0; index < length; index++)
    {
        var counts = new[] { 0, 1 }
            .Select(bin => input.Select(line => line[index] - '0').Count(b => b == bin))
            .ToArray();

        strGamma += counts[0] > counts[1] ? '0' : '1';
        strEpsilon += counts[0] > counts[1] ? '1' : '0';
    }

    var gamma = Convert.ToInt32(strGamma, 2);
    var epsilon = Convert.ToInt32(strEpsilon, 2);
    var result = gamma * epsilon;

    Console.WriteLine($"Part1 result: {result}");
}

Part1();