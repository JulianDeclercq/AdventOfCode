#include "Day5.h"

void Day5::Part1()
{
	//fstream input("Example/Day5Part1.txt");
	fstream input("Input/Day5.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile.'n";
		return;
	}

	vector<int> instructions = vector<int>();
	string line;

	// Parse the number from the string
	// If the number starts with the minus character, only parse from behind that and multiply by -1
	while (getline(input, line))
		instructions.push_back((line[0] == '-') ? stoi(line.substr(1)) *-1 : stoi(line));

	int index = 0, stepsTaken = 0;
	// As long as the index is valid
	while (index >= 0 && index < instructions.size())
	{
		// Save the old index to increment after jump
		int oldIndex = index;

		// Execute the jump
		index += instructions[index];

		// Increment the instruction that was just executed
		++instructions[oldIndex];

		// Increment the step counter
		++stepsTaken;
	}

	cout << "Day 6 Part 1 answer is: " << stepsTaken << endl;
}

void Day5::Part2()
{
}