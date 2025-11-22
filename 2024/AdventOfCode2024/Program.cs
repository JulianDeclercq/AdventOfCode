using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

// var day = new Day20("input/example/day20e.txt");
var day = new Day20("input/real/day20.txt");
day.Part1(100); // Find all cheats that save at least 100 picoseconds

watch.Stop();
Console.WriteLine(watch.Elapsed);
