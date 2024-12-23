namespace AdventOfCode2024;

public class Day23
{
    private static void AddConnection(string first, string second, Dictionary<string, HashSet<string>> connections)
    {
        var hashset = connections.TryGetValue(first, out var current) ? current : [];
        hashset.Add(second);
        connections[first] = hashset;
    }
    
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
        
        var lines = File.ReadAllLines("input/day23.txt");
        Dictionary<string, HashSet<string>> connections = [];
        foreach (var line in lines)
        {
            var split = line.Split('-');
            AddConnection(split[0], split[1], connections);
            AddConnection(split[1], split[0], connections);
        }
        
        // find sets of three connected computers where each computer in the set is connected to the other two computers
        HashSet<TriLink> triLinks = [];
        foreach (var computer in connections.Keys)
        {
            foreach (var connections1 in connections[computer])
            {
                foreach (var connections2 in connections[connections1])
                {
                    var connections3 = connections[connections2];
                    if (connections3.Contains(computer))
                    {
                        triLinks.Add(new TriLink
                        {
                            First = computer,
                            Second = connections1,
                            Third = connections2
                        });
                    }
                }
            }
        }
        
        var answer = triLinks.Count(triLink => triLink.HasComputerThatStartsWithT());
        Console.WriteLine(answer);
    }

    private class TriLink : IEquatable<TriLink>
    {
        public string First { get; init; }
        public string Second { get; init; }
        public string Third { get; init; }

        private string Alphabetical => string.Join("", new List<string> { First, Second, Third }.Order());

        public bool HasComputerThatStartsWithT() => First[0] == 't' || Second[0] == 't' || Third[0] == 't';

        public override string ToString() => $"{First},{Second},{Third}";

        public bool Equals(TriLink? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Alphabetical == other.Alphabetical;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TriLink)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Alphabetical);
        }
    }
}