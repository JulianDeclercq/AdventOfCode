namespace AdventOfCode2022.Days;

public class Day2
{
    // "wins against"
    private Dictionary<char, char> lookup = new()
    {
        ['A'] = 'Z', // Rock wins against scissors
        ['B'] = 'X', // Paper wins against rock
        ['C'] = 'Y', // Scissors wins against paper
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
            var enemyWins = lookup[enemyChoice] == myChoice;
            if (!enemyWins)
                score += 6;
        }
        Console.WriteLine(score);
    }
}