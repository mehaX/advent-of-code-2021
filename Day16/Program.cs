
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
    var result = rootPackage.CalculateVersionSum();
    Console.WriteLine($"Part 1: {result}");
}

Part1();

abstract class Package
{
    public int Version { get; set; }
    
    public string Payload { get; protected set; }

    public abstract void RegisterPayload(ref string payload);

    public abstract int CalculateVersionSum();
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
