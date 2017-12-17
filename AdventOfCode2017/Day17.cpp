#include "Day17.h"

void Day17::Part1()
{
	const int input = 366;

	vector<int> buffer = vector<int>{ 0 };
	int currentPosition = 0;

	for (int i = 1; i < 2018; ++i)
	{
		// Step forward, calculate the next idx
		currentPosition = (currentPosition + input) % buffer.size();

		// Insert the new number
		buffer.insert(buffer.begin() + currentPosition + 1, i);

		// Set the current position to the newly inserted item
		++currentPosition;
	}

	cout << "Day 17 Part 1 answer: " << buffer[currentPosition + 1] << endl;
}