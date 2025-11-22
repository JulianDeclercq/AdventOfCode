using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

// var day = new Day20("input/example/day20e.txt");
var day = new Day20("input/real/day20.txt");
day.Solve(part1: true);
day.Solve(part1: false);

watch.Stop();
Console.WriteLine(watch.Elapsed);
