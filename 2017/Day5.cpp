#include "Day5.h"

int Day5::Solve(bool part2)
{
	//fstream input("Example/Day5Part1.txt");
	fstream input("Input/Day5.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile.'n";
		return -1;
	}

	vector<int> instructions = vector<int>();
	string line;

	// Parse the number from the string
	// If the number starts with the minus character, only parse from behind that and multiply by -1
	while (getline(input, line))
		instructions.push_back((line[0] == '-') ? stoi(line.substr(1)) *-1 : stoi(line));

	int index = 0, stepsTaken = 0;
	// As long as the index is valid
	while (index >= 0 && index < (int)instructions.size())
	{
		// Save the old index to increment after jump
		int oldIndex = index;

		// Execute the jump
		index += instructions[index];

		// Increment the instruction that was just executed
		// or decrement it if it is part2 and the current instruction has a value of 3 or more
		(part2 && instructions[oldIndex] >= 3) ? --instructions[oldIndex] : ++instructions[oldIndex];

		// Increment the step counter
		++stepsTaken;
	}

	// Return the amount of steps taken
	return stepsTaken;
}

void Day5::Part1()
{
	cout << "Day 6 Part 1 answer is: " << Solve() << endl;
}

void Day5::Part2()
{
	cout << "Day 6 Part 2 answer is: " << Solve(true) << endl;
}