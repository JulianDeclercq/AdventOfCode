var lines = File.ReadAllLines("input/day1.txt")
    .Select(l => l.Split(' ')
        .Where(l2 => !string.IsNullOrWhiteSpace(l2))
        .Select(int.Parse)
        .ToArray())
    .ToArray();

var lhs = lines.Select(l => l.First()).Order();
var rhs = lines.Select(l => l.Last()).Order();
var answer = lhs.Zip(rhs).Aggregate(0, (total, next) => total + Math.Abs(next.First - next.Second));
Console.WriteLine(answer);