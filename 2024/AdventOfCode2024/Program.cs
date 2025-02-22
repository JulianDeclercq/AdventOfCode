using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

var day = new Day12();
day.Solve();

// Day7.Solve(part: 1);
// Day7.Solve(part: 2);

watch.Stop();
Console.WriteLine(watch.Elapsed);