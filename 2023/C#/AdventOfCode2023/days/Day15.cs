using System.Text.RegularExpressions;
using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public partial class Day15
{
    private record Lens(string Label, int FocalLength);

    public void Solve()
    {
        var helper = new RegexHelper(Pattern(), "label", "operation", "focallength");
        var input = File.ReadAllLines("../../../input/Day15.txt").First().Split(',');
        var boxes = new Dictionary<int, List<Lens>>();
        foreach (var line in input)
        {
            if (!helper.Match(line))
                throw new Exception($"Couldn't match {line}");

            var label = helper.Get("label");
            var operation = helper.Get("operation").First();

            switch (operation)
            {
                case '-':
                {
                    if (boxes.TryGetValue(Hash(label), out var lenses))
                    {
                        var idx = lenses.FindIndex(l => l.Label.Equals(label));
                        if (idx != -1)
                            lenses.RemoveAt(idx);
                    }
                    break;
                }
                case '=':
                {
                    var focalLength = helper.GetInt("focallength");
                    var labelHash = Hash(label);
                    if (boxes.TryGetValue(labelHash, out var lenses))
                    {
                        var idx = lenses.FindIndex(l => l.Label.Equals(label));
                        if (idx != -1)
                        {
                            lenses[idx] = lenses[idx] with { FocalLength = focalLength };
                        }
                        else
                        {
                            lenses.Add(new Lens(label, focalLength));
                        }
                    }
                    else
                    {
                        boxes[labelHash] = new List<Lens> { new(label, focalLength) };
                    }
                    break;
                }
            }
        }

        var configurationFocusingPower = 0;
        foreach (var (hash, lenses) in boxes)
        {
            for (var i = 0; i < lenses.Count; ++i)
            {
                var focusingPower = 1 + hash;
                focusingPower *= i + 1;
                focusingPower *= lenses[i].FocalLength;
                configurationFocusingPower += focusingPower;
            }
        }
        
        Console.WriteLine(input.Sum(Hash));
        Console.WriteLine(configurationFocusingPower);
    }

    private static int Hash(string input)
    {
        var hash = 0;
        foreach (var c in input)
        {
            hash += c;
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }

    [GeneratedRegex(@"(\w+)([=-])(\d+)?")]
    private static partial Regex Pattern();
}