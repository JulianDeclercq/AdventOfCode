using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

var day = new Day14();
day.Initialize(["p=2,4 v=2,-3"], example: true);
day.Solve();

watch.Stop();
Console.WriteLine(watch.Elapsed);