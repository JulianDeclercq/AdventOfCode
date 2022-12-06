namespace AdventOfCode2022.Days;

public class Day6
{
    public void Solve()
    {
        // var input = "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw";
        var input = File.ReadAllLines(@"..\..\..\input\day6.txt").Single();

        var different = new List<char>();
        for (var i = 0; i < input.Length; ++i)
        {
            var idx = different.FindIndex(x => x == input[i]);
            
            if (idx != -1)
                different = different.Skip(idx + 1).ToList();
            
            different.Add(input[i]);

            if (different.Count != 4)
                continue;
            
            Console.WriteLine(string.Join("", different));
            Console.WriteLine(i + 1);
            break;
        }
    }
}