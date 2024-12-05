
var haystack = File.ReadLines(@"..\..\..\input.txt")
        .Select(line => line.ToCharArray().ToList())
        .ToList();

var needle = new List<char> { 'X', 'M', 'A', 'S' };

var directions = new List<(int, int)>
{
    (0, 1),
    (0, -1),
    (1, 0),
    (-1, 0),
    (1, 1),
    (-1, -1),
    (1, -1),
    (-1, 1)
};

var allMatchedPoints = new List<(int, int)> { };

var matchCount = directions.Select(GetMatchesForDirection).Sum();
Console.WriteLine(matchCount);
//PrintHaystack();

// Part 2

allMatchedPoints.Clear();

var needles2 = new List<List<List<char>>> {
    new() {
        new List<char> { 'M', '.', 'S' },
        new List<char> { '.', 'A', '.' },
        new List<char> { 'M', '.', 'S' }
    },
    new() {
        new List<char> { 'S', '.', 'S' },
        new List<char> { '.', 'A', '.' },
        new List<char> { 'M', '.', 'M' }
    },
    new() {
        new List<char> { 'S', '.', 'M' },
        new List<char> { '.', 'A', '.' },
        new List<char> { 'S', '.', 'M' }
    },
    new() {
        new List<char> { 'M', '.', 'M' },
        new List<char> { '.', 'A', '.' },
        new List<char> { 'S', '.', 'S' }
    },
};

var matchCount2 = GetAllPointsInHaystack().FindAll(MatchesAt2).Count;
Console.WriteLine(matchCount2);

return;

List<(int, int)> GetAllPointsInHaystack()
{
    var points = new List<(int, int)>();

    for (var x = 0; x < haystack.Count; x++)
    {
        for (var y = 0; y < haystack[x].Count; y++)
        {
            points.Add((x, y));
        }
    }

    return points;
}

bool MatchesAt2((int, int) position)
{
    var points = GetPointsAt2(position);
    for (var i = 0; i < needles2.Count; i++)
    {
        var matches = 0;
        var currenNeedle = needles2[i];
        for (var j = 0; j < points.Count; j++)
        {
            var x = points[j].Item1;
            var y = points[j].Item2;

            if (x < 0 || x >= haystack.Count || y < 0 || y >= haystack[x].Count)
            {
                break;
            }

            var actual = haystack[x][y];
            if (actual == currenNeedle[j / 3][j % 3] || currenNeedle[j / 3][j % 3] == '.')
            {
                matches++;
                if (matches == 9)
                {
                    allMatchedPoints.AddRange(points);
                    return true;
                }
            }
            else
            {
                break;
            }
        }
    }

    return false;
}

int GetMatchesForDirection((int, int) direction)
{
    return GetAllPointsInHaystack().Select(p => GetMatchesAt(p, direction)).Sum();
}

int GetMatchesAt((int, int) position, (int, int) direction)
{
    var matches = 0;
    
    var points = GetPointsInDirection(position, direction);
    for (var i = 0; i < points.Count; i++)
    {
        var x = points[i].Item1;
        var y = points[i].Item2;
        
        if (x < 0 || x >= haystack.Count || y < 0 || y >= haystack[x].Count)
        {
            break;
        }
        
        var actual = haystack[x][y];
        if (actual == needle[i])
        {
            if (i == needle.Count - 1)
            {
                matches++;
                allMatchedPoints.AddRange(points);
            }
        }
        else
        {
            break;
        }
    }

    return matches;
}

List<(int, int)> GetPointsAt2((int, int) position)
{
    return new List<(int, int)>()
    {
        position,
        (position.Item1, position.Item2 + 1),
        (position.Item1, position.Item2 + 2),
        (position.Item1 + 1, position.Item2),
        (position.Item1 + 1, position.Item2 + 1),
        (position.Item1 + 1, position.Item2 + 2),
        (position.Item1 + 2, position.Item2),
        (position.Item1 + 2, position.Item2 + 1),
        (position.Item1 + 2, position.Item2 + 2),
    };
}

List<(int, int)> GetPointsInDirection((int, int) start, (int, int) direction)
{
    var points = new List<(int, int)>();

    var current = start;
    for (var i = 0; i < needle.Count; i++)
    {
        points.Add(current);
        current = (current.Item1 + direction.Item1, current.Item2 + direction.Item2);
    }

    return points;
}

void PrintHaystack()
{
    for (var x = 0; x < haystack.Count; x++)
    {
        for (var y = 0; y < haystack[x].Count; y++)
        {
            Console.Write(allMatchedPoints.Contains((x, y)) ? haystack[x][y] : '.');
        }
        Console.WriteLine();
    }
}
