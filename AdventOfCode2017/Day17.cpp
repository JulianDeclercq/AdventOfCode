#include "Day17.h"

void Day17::Part1()
{
	const int input = 366;

	vector<int> buffer = vector<int>{ 0 };
	int currentPosition = 0;

	// Reserve space in the buffer
	buffer.reserve(2018);

	for (int i = 1; i <= 2017; ++i)
	{
		// Calculate the new index
		currentPosition = (currentPosition + input + 1) % buffer.size();

		// Insert the new number
		buffer.insert(buffer.begin() + currentPosition, i);
	}

	// For part 1, the answer is the next number after the last inserted value
	cout << "Day 17 Part 1 answer: " << buffer[currentPosition + 1] << endl;
}

void Day17::Part2()
{
	const int input = 366;

	vector<int> buffer = vector<int>{ 0 };
	int answer = 0;
	int currentPosition = 0;
	for (int i = 1; i <= 50000000; ++i)
	{
		// Calculate the new index
		currentPosition = (currentPosition + input + 1) % i;

		// If the current position is 0, update the answer
		if (currentPosition == 0)
			answer = i;
	}
	cout << "Day 17 Part 2 answer: " << answer << endl;
}