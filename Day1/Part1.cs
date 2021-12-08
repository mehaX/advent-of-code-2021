using System;
using System.IO;

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

Console.WriteLine($"The count of measurements increased: {count}");
