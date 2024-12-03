
using System.Text.RegularExpressions;

var input = File.ReadAllText(@"..\..\..\input.txt");

var regex = new Regex(@"(mul\(\d{1,3},\d{1,3}\))|(do\(\))|(don't\(\))");

var matches = regex.Matches(input);

var result1 = matches.Where(m => GetInstruction(m) == Instruction.Mul).Sum(GetMulResult);

Console.WriteLine("Part 1: " + result1);

// Part 2

var add = true;
var sum = 0;
// for each match
foreach (Match match in matches)
{
    switch (GetInstruction(match))
    {
        case Instruction.Mul:
            if (add)
                sum += GetMulResult(match);
            break;
        case Instruction.Do:
            add = true;
            break;
        case Instruction.Dont:
            add = false;
            break;
    }
}

Console.WriteLine("Part 2: " + sum);

return;

int GetMulResult(Match match)
{
    var parts = match.Value.Split(",");
    var a = Convert.ToInt32(parts[0][4..]);
    var b = Convert.ToInt32(parts[1][..^1]);
    return a * b;
}

Instruction GetInstruction(Match match)
{
    if (match.Groups[1].Success)
    {
        return Instruction.Mul;
    }
    else if (match.Groups[2].Success)
    {
        return Instruction.Do;
    }
    else if (match.Groups[3].Success)
    {
        return Instruction.Dont;
    }
    throw new InvalidOperationException();
}

internal enum Instruction
{
    Mul,
    Do,
    Dont
}