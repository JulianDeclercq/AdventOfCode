using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

var day = new Day14();
day.InitializeFromFile("input/day14.txt");
day.Solve();

watch.Stop();
Console.WriteLine(watch.Elapsed);