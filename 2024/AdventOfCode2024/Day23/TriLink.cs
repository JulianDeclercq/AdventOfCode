namespace AdventOfCode2024.Day23;
public class TriLink : IEquatable<TriLink>
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
        return Alphabetical.GetHashCode();
    }
}