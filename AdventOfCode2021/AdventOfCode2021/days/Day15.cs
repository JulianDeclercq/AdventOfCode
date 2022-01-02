using System.Text;

namespace AdventOfCode2021.days
{
    public class Day15
    {
        private class Path
        {
            public Path()
            {
                Points = new List<Point>();
            }

            public Path(Point start)
            {
                Points = new List<Point>() {start};
            }

            public void Add(Point p, int risk)
            { 
                Points.Add(p);
                Risk += risk;
            } 
            
            public List<Point> Points { get; private init; }
            public int Risk = 0;

            public Path Copy() => new() { Points = Points.ToList(), Risk = Risk};

            public override string ToString() => new StringBuilder().AppendJoin('|', Points).ToString();
        }

        private List<Path> _allPaths = new();
        private int _lowestRisk = int.MaxValue;
        
        public void Part1()
        {
            var lines = File.ReadAllLines(@"..\..\..\input\day15_example.txt");
            //var lines = File.ReadAllLines(@"..\..\..\input\day15.txt");
            int width = lines[0].Length, height = lines.Length;
            var grid = new Grid<int>(width, height, 
                lines.SelectMany(l => l.ToCharArray()).Select(Helpers.ToInt), int.MinValue);

            var last = new Point(width - 1, height - 1);
            FindPaths(last, new Path(new Point(0, 0)), grid);
            Console.WriteLine($"{_allPaths.Count} paths were found.");
            Console.WriteLine(_allPaths.MinBy(p => p.Risk));
            Console.WriteLine($"Day 15 part 1: {_allPaths.Min(p => p.Risk)}");
        }

        private bool PotentialBestPath(Path p)
        {
            if (!_allPaths.Any())
                return true;

            return p.Risk < _allPaths.Select(x => x.Risk).Min();
        }  

        private void FindPaths(Point target, Path current, Grid<int> grid)
        {
            if (!PotentialBestPath(current))
                return;
            
            var validNeighbours = grid.NeighbouringPoints(current.Points.Last(), false).Except(current.Points).ToList();
            
            foreach (var p in validNeighbours)
            {
                var newPath = current.Copy();
                newPath.Add(p, grid.At(p));
                
                if (p.Equals(target))
                {
                    _allPaths.Add(newPath);
                    Console.WriteLine($"Found path, risk: {newPath.Risk}");
                    //Console.WriteLine(newPath);
                    break;
                }
                FindPaths(target, newPath, grid);
            }
        }
    }
}