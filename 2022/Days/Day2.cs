﻿namespace AdventOfCode2022.Days;

public class Day2
{
    private static readonly Dictionary<char, int> ShapeScore = new()
    {
        ['X'] = 1, ['A'] = 1, // Rock
        ['Y'] = 2, ['B'] = 2, // Paper
        ['Z'] = 3, ['C'] = 3, // Scissors
    };
    
    private static int OutcomeScorePart1(char myChoice, char enemyChoice)
    {
        // Draw
        var equivalent = enemyChoice switch
        {
            'A' => 'X',
            'B' => 'Y',
            'C' => 'Z',
        };
        
        if (myChoice == equivalent)
           return 3;
        
        // Loss or Win
        var enemyWinsAgainst = enemyChoice switch
        {
            'A' => 'Z', // Rock wins against scissors
            'B' => 'X', // Paper wins against rock
            'C' => 'Y', // Scissors wins against paper
        };
        return enemyWinsAgainst == myChoice ? 0 : 6;
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

    public static void Solve(bool part1 = true)
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
            score += ShapeScore[myChoice];
            score += part1 ? OutcomeScorePart1(myChoice, enemyChoice) : OutcomeScorePart2(line[2]);
        }
        Console.WriteLine($"Day 2 part {(part1 ? 1 : 2)}: {score}.");
    }
}