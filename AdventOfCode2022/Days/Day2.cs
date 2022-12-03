namespace AdventOfCode2022.Days;

public class Day2
{
    private Dictionary<char, int> shapeScore = new()
    {
        ['X'] = 1, ['A'] = 1, // Rock
        ['Y'] = 2, ['B'] = 2, // Paper
        ['Z'] = 3, ['C'] = 3, // Scissors
    };
   

    private static int OutcomeScorePart1(char myChoice, char enemyChoice)
    {
        // "wins against"
        var lookup = new Dictionary<char, char>()
        {
            ['A'] = 'Z', // Rock wins against scissors
            ['B'] = 'X', // Paper wins against rock
            ['C'] = 'Y', // Scissors wins against paper
        };

        var equivalent = new Dictionary<char, char>()
        {
            ['A'] = 'X',
            ['B'] = 'Y',
            ['C'] = 'Z',
        };
        
        // Draw
        if (myChoice == equivalent[enemyChoice])
           return 3;
            
        var enemyWins = lookup[enemyChoice] == myChoice;
        return enemyWins ? 0 : 6;
    }

    private static int OutcomeScorePart2(char matchResult)
    {
        return matchResult switch
        {
            'Y' => 3,
            'Z' => 6,
            _ => 0
        };
    }

    public void Solve(bool part1 = true)
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day2.txt");

        var score = 0;
        foreach (var line in lines)
        {
            var enemyChoice = line[0];
            var myChoice = part1 ? line[2] : line[2] switch
            {
                // lose
                'X' when enemyChoice is 'A' => 'C', // against rock
                'X' when enemyChoice is 'B' => 'A', // against paper
                'X' when enemyChoice is 'C' => 'B', // against scissors
                
                // draw
                'Y' => enemyChoice,
                
                // win
                'Z' when enemyChoice is 'A' => 'B', // against rock
                'Z' when enemyChoice is 'B' => 'C', // against paper
                'Z' when enemyChoice is 'C' => 'A', // against scissors
            };
            score += shapeScore[myChoice];
            score += part1 ? OutcomeScorePart1(myChoice, enemyChoice) : OutcomeScorePart2(line[2]);
        }
        Console.WriteLine($"Day 2 part {(part1 ? 1 : 2)}: {score}.");
    }
}