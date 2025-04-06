using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

var day = new Day12("input/day12e.txt");
day.SolvePart(part: 1);
day.SolvePart(part: 2);

Console.WriteLine("is this debug working or nah");

watch.Stop();
Console.WriteLine(watch.Elapsed);
