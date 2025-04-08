using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

var day = new Day12("input/day12.txt");
Console.WriteLine(day.SolvePart(part: 1));
Console.WriteLine(day.SolvePart(part: 2));

watch.Stop();
Console.WriteLine(watch.Elapsed);