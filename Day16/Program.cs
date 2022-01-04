
static void Part1()
{
    var inputHex = File.ReadAllText("input.txt");
    var inputBin = string.Join("", inputHex.ToCharArray().Select(c =>
    {
        var number = Convert.ToInt32(c.ToString(), 16);
        var result = Convert.ToString(number, 2);
        if (number < 8)
        {
            result = "0" + result;
        }

        if (number < 4)
        {
            result = "0" + result;
        }

        if (number < 2)
        {
            result = "0" + result;
        }
        return result;
    }));
    

    var rootPackage = PackageParser.Parse(ref inputBin);
    var part1 = rootPackage.CalculateVersionSum();
    var part2 = rootPackage.CalculateResult();
    Console.WriteLine($"Part 1: {part1}");
    Console.WriteLine($"Part 2: {part2}");
}

Part1();

abstract class Package
{
    public int Version { get; set; }
    
    public string Payload { get; protected set; }

    public abstract void RegisterPayload(ref string payload);

    public abstract int CalculateVersionSum();

    public abstract long CalculateResult();
}

class LiteralPackage : Package
{
    public long Value { get; private set; }
    
    public override void RegisterPayload(ref string payload)
    {
        Payload = payload;
        
        var literal = "";
        while (payload.Length >= 5)
        {
            PackageParser.SpliceNumber(ref payload, out var prefix, 1);
            PackageParser.SpliceString(ref payload, out var value, 4);
            literal += value;
            if (prefix == 0)
            {
                break;
            }
        }
        Value = Convert.ToInt64(literal, 2);
    }

    public override int CalculateVersionSum()
    {
        return Version;
    }

    public override long CalculateResult()
    {
        return Value;
    }
}

class OperatorPackage : Package
{
    public int Id { get; set; }
    
    public readonly List<Package> SubPackages = new List<Package>();

    public OperatorPackage(int id)
    {
        Id = id;
        Payload = "";
    }
    
    public override void RegisterPayload(ref string payload)
    {
        Payload = payload;
        
        PackageParser.GetLengthTypeId(ref payload, out var lengthTypeId);
        if (lengthTypeId == 0)
        {
            PackageParser.SpliceNumber(ref payload, out var length, 15);
            var newBin = payload.Substring(0, length);
            while (newBin.Length > 0)
            {
                SubPackages.Add(PackageParser.Parse(ref newBin));
            }

            payload = payload.Substring(length);
        }
        else
        {
            PackageParser.SpliceNumber(ref payload, out var count, 11);
            for (int i = 0; i < count; i++)
            {
                SubPackages.Add(PackageParser.Parse(ref payload));
            }
        }
    }

    public override int CalculateVersionSum()
    {
        return Version + SubPackages.Select(p => p.CalculateVersionSum()).Sum();
    }

    public override long CalculateResult()
    {
        var packageValues = SubPackages.Select(p => p.CalculateResult()).ToArray();
        switch (Id)
        {
            case 0: // ADD
                return packageValues.Sum();
            
            case 1: // MULTIPLY
                return packageValues.Aggregate((a, b) => a * b);
            
            case 2: // MIN
                return packageValues.Min();
            
            case 3: // MAX
                return packageValues.Max();
            
            case 5: // GREATER THAN
                return packageValues.Length >= 2 && packageValues[0] > packageValues[1] ? 1 : 0;
            
            case 6: // LESS THAN
                return packageValues.Length >= 2 && packageValues[0] < packageValues[1] ? 1 : 0;
            
            case 7: // EQUALS
                return packageValues.Length >= 2 && packageValues[0] == packageValues[1] ? 1 : 0;
            
            default:
                return 0;
        }
    }
}

class PackageParser
{
    public static Package Parse(ref string inputBin)
    {
        Package package;
        GetVersion(ref inputBin, out var version);
        GetPackageId(ref inputBin, out var packageId);

        if (packageId == 4) // literal number
        {
            package = new LiteralPackage();
        }
        else // operator
        {
            package = new OperatorPackage(packageId);
        }
        package.RegisterPayload(ref inputBin);
        package.Version = version;

        return package;
    }
    
    public static void GetVersion(ref string inputBin, out int version)
    {
        SpliceNumber(ref inputBin, out version, 3);
    }

    public static void GetPackageId(ref string inputBin, out int packageId)
    {
        SpliceNumber(ref inputBin, out packageId, 3);
    }

    public static void GetLengthTypeId(ref string inputBin, out int lengthTypeId)
    {
        SpliceNumber(ref inputBin, out lengthTypeId, 1);
    }

    public static void SpliceNumber(ref string inputBin, out int result, int length)
    {
        var cLength = length < inputBin.Length ? length : inputBin.Length;
        var strResult = inputBin.Substring(0, cLength);
        inputBin = inputBin.Substring(cLength);
        if (inputBin == "")
        {
            inputBin = "0";
        }
        result = Convert.ToInt32(strResult, 2);
    }

    public static void SpliceString(ref string inputBin, out string result, int length)
    {
        var cLength = length < inputBin.Length ? length : inputBin.Length;
        result = inputBin.Substring(0, cLength);
        inputBin = inputBin.Substring(cLength);
    }
}
