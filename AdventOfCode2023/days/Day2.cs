using System.Text.RegularExpressions;

namespace AdventOfCode2023.days;

public class Day2
{
    // Cut out the "Game X:" and add a trailing ";" to make parsing easier
    private readonly string[] _lines =
        File.ReadAllLines("../../../input/Day2.txt").Select(l => $"{l[(l.IndexOf(':') + 1)..]};").ToArray();
    private readonly Regex _setPattern = new("([^;]+);");
    private readonly Regex _cubePattern = new(@"(\d+) (\w+)");

    public void Part1()
    {
        var games = ParseGames();
        var loadedConfig = new CubeSet { Red = 12, Green = 13, Blue = 14 };

        Console.WriteLine(games.Where(g => g.CubeSets.All(cs => cs.FitsConfig(loadedConfig))).Sum(g => g.Number));
    }

    public void Part2()
    {
        var games = ParseGames();
        Console.WriteLine(games.Select(g => g.MinimumSet()).Sum(s => s.Power));
    }

    private IReadOnlyCollection<Game> ParseGames()
    {
        var games = new List<Game>();
        for (var i = 0; i < _lines.Length; ++i)
        {
            var game = new Game(i + 1);
            var sets = _setPattern.Matches(_lines[i]);
            var cubesPerSet = sets.Select(match => _cubePattern.Matches(match.Groups[1].ToString()));

            foreach (var cubeSet in cubesPerSet)
            {
                var set = new CubeSet();
                foreach (Match cubeMatch in cubeSet)
                {
                    var amount = int.Parse(cubeMatch.Groups[1].ToString());
                    switch (cubeMatch.Groups[2].ToString())
                    {
                        case "red": set.Red += amount; break;
                        case "blue": set.Blue += amount; break;
                        case "green": set.Green += amount; break;
                    } 
                }
                game.CubeSets.Add(set);
            }
            games.Add(game);
        }

        return games;
    }

    private class Game
    {
        public Game(int number)
        {
            Number = number;
        }
        
        public readonly int Number = 0;
        public readonly List<CubeSet> CubeSets = new();

        public CubeSet MinimumSet()
        {
            return new CubeSet
            {
                Red = CubeSets.Max(cs => cs.Red),
                Green = CubeSets.Max(cs => cs.Green),
                Blue = CubeSets.Max(cs => cs.Blue),
            };
        }
    }

    private class CubeSet
    {
        public int Red = 0;
        public int Green = 0;
        public int Blue = 0;

        public int Power => Red * Green * Blue;
        
        public bool FitsConfig(CubeSet config)
        {
            return Red <= config.Red && Green <= config.Green && Blue <= config.Blue;
        }
    }
}

