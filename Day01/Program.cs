
var contents = File.ReadLines(@"..\..\..\input.txt");

var left = new List<int>();
var right = new List<int>();

foreach (var line in contents)
{
    var parts = line.Split("   ");
    left.Add(Convert.ToInt32(parts[0]));
    right.Add(Convert.ToInt32(parts[1]));
}

left.Sort();
right.Sort();

var sum = 0;

for (var i = 0; i < left.Count; i++)
{
    sum += Math.Abs(left[i] - right[i]);
}

Console.WriteLine(sum);

// Part 2

var sum2 = left.Sum(l => right.FindAll(r => r == l).Count * l);

Console.WriteLine(sum2);