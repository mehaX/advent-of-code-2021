static void Part1()
{
    string result = File.ReadAllText("measurements-1.txt");
    string[] measurements = result.Split("\n");

    int? prevNumber = null;
    var count = 0;
    foreach(var measurement in measurements)
    {
        var nextNumber = Convert.ToInt32(measurement);
        if (prevNumber != null && nextNumber > prevNumber)
        {
            count++;
        }

        prevNumber = nextNumber;
    }

    Console.WriteLine($"Day 1, part 1: {count}");
}

static void Part2()
{
    string result = File.ReadAllText("measurements-2.txt");
    var measurements = result.Split("\n").ToList();

    int? prevSum = null;
    var count = 0;
    for (var index = 0; index < measurements.Count; index++)
    {
        var nextSum = measurements.Skip(index).Take(3).Select(m => Convert.ToInt32(m)).Sum();
        if (prevSum != null && nextSum > prevSum)
        {
            count++;
        }

        prevSum = nextSum;
    }

    Console.WriteLine($"Day 1, part 2: {count}");
}

Part1();
Part2();