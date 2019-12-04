#include "Day1.h"

void Day1::read_input()
{
	ifstream input("Input/day1.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	while (getline(input, line))
		_masses.push_back(stoi(line));

	cout << "test " << fuel_required(100756) << endl;
}

int Day1::fuel_required(int mass)
{
	// literally following the instructions
	//return int(floor(mass / 3.0) - 2.0);

	// simpler and works too due to int rounding
	return mass / 3 - 2;
}

void Day1::part1()
{
	cout << "Day 1 part 1:" << endl;

	// read the input
	read_input();

	// give the answer required for part 1
	int answer = 0;
	for (int mass : _masses)
		answer += fuel_required(mass);

	cout << "The answer to day 1 part 1 is: " << answer << endl;
}
