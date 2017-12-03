#include <string>
#include <iostream>
#include <vector>
#include <map>

using namespace std;

namespace Day3
{
#pragma region General
	struct Point
	{
		Point() {}
		Point(int x, int y) : X(x), Y(y) {}
		int X = 0, Y = 0;
	};

	// The puzzle input
	const int _puzzleInput = 361527;

	// A map containing all numbers and their respective positions
	map<int, Point> _numberPositions = map<int, Point>();

	int ManhattanDistance(Point p1, Point p2)
	{
		return (abs(p1.X - p2.X) + abs(p1.Y - p2.Y));
	}

	void GenerateSpiralMemory(int maxNumber)
	{
		int step = 1, stepProgression = 0, xCoord = 0, yCoord = 0;
		bool horizontal = true;
		int direction = 1; // (Horizontal: left <-> right. Vertical: up <-> down)
		for (int i = 1; i < maxNumber + 1; ++i)
		{
			// Add the current number on the current position
			_numberPositions.insert({ i, Point(xCoord, yCoord) });

			// Calculate the next numbers position
			(horizontal) ? xCoord += direction : yCoord += direction;

			// Increment step progression
			++stepProgression;

			// If this step has now been completed,
			if (stepProgression == step)
			{
				// Flip the direction
				horizontal = !horizontal;

				// Reset the step progression
				stepProgression = 0;

				// When switched back from vertical to horizontal:
				// * the step should be incremented
				// * the direction changes (Horizontal: left <-> right. Vertical: up <-> down)
				if (horizontal)
				{
					++step;
					direction *= -1;
				}
			}
		}
	}

#pragma endregion

#pragma region Part1

	void Part1()
	{
		GenerateSpiralMemory(_puzzleInput);
		std::cout << "Day 2 Part 1 answer is:" << ManhattanDistance(Point(0, 0), _numberPositions[_puzzleInput]) << std::endl;
	}
#pragma endregion

#pragma region Part2
	void Part2()
	{
		std::cout << "Day 2 Part 2 answer is: " << std::endl;
	}
#pragma endregion
}