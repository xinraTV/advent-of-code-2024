

using PagePair = (int, int);
using Update = System.Collections.Generic.List<int>;

var input = File.ReadAllLines(@"..\..\..\input.txt");

var rules = input
    .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
    .Select(line => line.Split("|").Select(int.Parse).ToArray())
    .Select(parts => (parts[0], parts[1]))
    .ToList();

var updates = input
    .Skip(rules.Count + 1)
    .Select(line => line.Split(",").Select(int.Parse).ToList())
    .ToList();

var result1 = updates
    .FindAll(IsValid)
    .Select(GetMiddlePage)
    .Sum();

Console.WriteLine("Part 1: " + result1);

// Part 2

var longestChain = 0;

var result2 = updates
    .FindAll(u => !IsValid(u))
    .Select(MakeValid)
    .Select(GetMiddlePage)
    .Sum();

Console.WriteLine("Part 2: " + result2);

return;

Update MakeValid(Update update)
{
    Console.WriteLine("Making valid: " + string.Join(",", update));
    // zu langsam
    // return GetAllPossiblePermutations(update).First(IsValid);

    longestChain = 0;
    Console.WriteLine("Target length: " + update.Count);
    return GetValidPermutations(update, update.Count).First();
}

List<Update> GetValidPermutations(Update update, int targetLength)
{
    if (update.Count == 1)
    {
        return [update];
    }
    var validPermutations = new List<Update>();
    for (var i = 0; i < update.Count; i++)
    {
        var start = update[i];
        var rest = update.Where((_, index) => index != i).ToList();
        var restPermutations = GetValidPermutations(rest, targetLength);
        foreach (var restPermutation in restPermutations)
        {
            var permutation = restPermutation.Prepend(start).ToList();
            if (IsValid(permutation))
            {
                if (permutation.Count == targetLength)
                {
                    return [permutation];
                }
                if (permutation.Count > longestChain)
                {
                    longestChain = permutation.Count;
                    Console.WriteLine("Longest chain: " + longestChain);
                }
                validPermutations.Add(permutation);
            }
        }
    }
    return validPermutations;
}

List<Update> GetAllPossiblePermutations(Update update)
{
    if (update.Count == 1)
    {
        return [update];
    }
    var permutations = new List<Update>();
    for (var i = 0; i < update.Count; i++)
    {
        var start = update[i];
        var rest = update.Where((_, index) => index != i).ToList();
        var restPermutations = GetAllPossiblePermutations(rest);
        foreach (var restPermutation in restPermutations)
        {
            permutations.Add(restPermutation.Prepend(start).ToList());
        }
    }
    return permutations;
}

bool IsValid(Update update)
{
    for (var i = 0; i < update.Count; i++)
    {
        if (!IsPageValid(update, i))
        {
            return false;
        }
    }
    return true;
}

bool IsPageValid(Update update, int pageIndex)
{
    for (var i = 0; i < update.Count; i++)
    {
        if (i == pageIndex)
        {
            continue;
        }

        var currentPair = i < pageIndex ? (update[i], update[pageIndex]) : (update[pageIndex], update[i]);
        if (ViolatesAnyRule(currentPair))
        {
            return false;
        }
    }
    return true;
}

bool ViolatesAnyRule(PagePair pages)
{
    return rules.Any(r => r.Item1 == pages.Item2 && r.Item2 == pages.Item1);
}

int GetMiddlePage(Update update)
{
    return update[update.Count / 2];
}
