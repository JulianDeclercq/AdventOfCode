using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

// var day = new Day19("input/example/day19e.txt");
var day = new Day19("input/real/day19.txt");
// day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine(watch.Elapsed);