namespace AdventOfCode2022.Days;

public class Day13
{
    private class ListElement
    {
        public int Value = 0;
        public List<ListElement> Contents = new();
        public ListElementType Type = ListElementType.None;
    }

    private enum ListElementType
    {
        None = 0,
        Integer = 1,
        List = 2
    }
    
    public void Solve()
    {
        var pairs = File.ReadAllLines(@"..\..\..\input\day13_example.txt").Where(l => !string.IsNullOrEmpty(l)).Chunk(2);
        foreach (var pair in pairs)
        {
            var left = pair.First();
            var right = pair.Last();
            Console.WriteLine($"{left} | {right}");
        }
        
        // list element could be an integer or another list

    }

    public void ParseList(string list)
    {
        var kek = new Stack<char>();
    }
}