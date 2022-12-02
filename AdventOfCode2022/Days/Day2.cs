namespace AdventOfCode2022.Days;

public class Day2
{
    // "wins against"
    private Dictionary<char, char> winsAgainst = new()
    {
        ['X'] = 'C', // Rock wins against scissors
        ['Y'] = 'A', // Paper wins against rock
        ['Z'] = 'B', // Scissors wins against paper
    };

    private Dictionary<char, int> shapeScore = new()
    {
        ['X'] = 1, // Rock
        ['Y'] = 2, // Paper
        ['Z'] = 3, // Scissors
    };

    private Dictionary<char, char> equivalent = new()
    {
        ['A'] = 'X',
        ['B'] = 'Y',
        ['C'] = 'Z',
    };

    public void Solve()
    {
        //var lines = File.ReadAllLines(@"..\..\..\input\day2_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day2.txt");

        var score = 0;
        foreach (var line in lines)
        {
            var enemyChoice = line[0];
            var myChoice = line[2];

            score += shapeScore[myChoice];

            // Draw
            if (myChoice == equivalent[enemyChoice])
            {
                score += 3;
                continue;
            }
            
            // You win
            var win = winsAgainst[myChoice] == enemyChoice;
            if (win)
                score += 6;
        }
        Console.WriteLine(score);
    }
}