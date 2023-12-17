namespace AdventOfCode2016.Days;

public static class Day6
{
    private static int _testCounter = 0;
    public static void Solve()
    {
        AssertEqual(6, DecompressedLength("ADVENT"));
        AssertEqual(7, DecompressedLength("A(1x5)BC"));
        AssertEqual(9, DecompressedLength("(3x3)XYZ"));
        AssertEqual(11, DecompressedLength("A(2x2)BCD(2x2)EFG"));
        AssertEqual(6, DecompressedLength("(6x1)(1x3)A"));
        AssertEqual(18, DecompressedLength("X(8x2)(3x3)ABCY"));
        Console.WriteLine($"{_testCounter} TESTS PASSED.");

        DecompressedLength("(6x9)JUORKH(10x13)LNWIKDMACM(126x14)");
        //Console.WriteLine(DecompressedLength(File.ReadAllText("../../../input/Day6.txt")));
    }

    private static int DecompressedLength(string input)
    {
        var offset = 0;
        for (;;)
        {
            if (offset >= input.Length)
                break;
            
            var workingCopy = new string(input.AsSpan()[offset..]);
            
            var start = workingCopy.IndexOf('(');
            if (start == -1)
                break;

            var end = workingCopy.IndexOf(')');

            var dbg = workingCopy[(start + 1)..end].Split('x');
            
            var markerInfo = workingCopy[(start + 1)..end].Split('x').Select(int.Parse).ToArray();
            var (range, multiplier) = (markerInfo[0], markerInfo[1]);

            var toRepeat = workingCopy.Substring(end + 1, range);
            var cleaned = workingCopy.Remove(start, end - start + 1 + toRepeat.Length);
            
            var repeated = string.Join("", Enumerable.Repeat(toRepeat, multiplier));

            var transformedPart = cleaned.Insert(start, repeated); 
            input = input.ReplaceFirst(workingCopy, transformedPart);

            offset = start + repeated.Length + 1;
        }

        return input.Length;
    }

    private static void AssertEqual<T>(T expected, T value)
    {
        if (!value.Equals(expected))
            throw new Exception($"got: {value}, expected: {expected}");

        _testCounter++;
    }
}

public static class StringExtension
{
    public static string ReplaceFirst(this string text, string search, string replace)
    {
        var pos = text.IndexOf(search, StringComparison.Ordinal);
        return pos < 0 ? text : string.Concat(text.AsSpan(0, pos), replace, text.AsSpan(pos + search.Length));
    }
}