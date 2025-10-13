using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

// var day = new Day18("input/example/day18e.txt");
var day = new Day19("input/real/day18.txt");
day.Part1();

watch.Stop();
Console.WriteLine(watch.Elapsed);