static void Part1()
{
    var input = File.ReadAllText("input.txt");
    var fishes = input.Split(",").Select(c => Convert.ToInt32(c)).ToList();
    var days = 80;

    // Console.WriteLine($"Initial state: " + string.Join(", ", fishes));
    for (var day = 1; day <= days; day++)
    {
        var totalFishes = fishes.Count;
        for (var i = 0; i < totalFishes; i++)
        {
            if (fishes[i] == 0)
            {
                fishes.Add(8);
                fishes[i] = 7;
            }

            fishes[i]--;
        }

        // Console.WriteLine($"Day {day}: " + string.Join(", ", fishes));
    }

    var count = fishes.Count;
    
    Console.WriteLine($"Part1: {count}");
}

Part1();
