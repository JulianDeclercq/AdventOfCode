using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

var day = new Day16("input/real/day16.txt");
day.Solve();

watch.Stop();
Console.WriteLine(watch.Elapsed);