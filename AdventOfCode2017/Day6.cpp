#include "Day6.h"

std::string Day6::State(const vector<int>& memoryBanks)
{
	string state = "";
	for (int bank : memoryBanks)
		state += to_string(bank) + " ";

	return state;
}

int Day6::Solve(bool part2 /*= false*/)
{
	//	ifstream input("Example/Day6Part1.txt");
	ifstream input("Input/Day6.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile.\n";
		return -1;
	}

	string line;
	getline(input, line);

	// Parse the input to a vector of int
	vector<int> memoryBanks = vector<int>();
	for (const string& memoryBankString : Helpers::Split(line, ' '))
		memoryBanks.push_back(stoi(memoryBankString));

	// Create a vector of all states that will be achieved
	vector<string> states = vector<string>{ State(memoryBanks) };

	// Bool for part 2
	bool hasReset = false;

	// As long as the newState has never been reached before, loop
	string newState = "";
	int cycleCounter = 0;
	for (;;)
	{
		// Increment the cycle counter
		++cycleCounter;

		// Find the largest memory block
		auto max = max_element(memoryBanks.begin(), memoryBanks.end());

		// Take all the blocks from the maximum element
		int blocksLeft = *max;
		*max = 0;

		// Distribute all the blocks
		// Start the distribution at the element after max.
		// Check if max was the last element in the list, if it was start at the beginning of the list
		auto it = (max == memoryBanks.end() - 1) ? memoryBanks.begin() : max + 1;
		while (blocksLeft > 0)
		{
			// If the end of the vector has reached, go back to the start
			if (it == memoryBanks.end())
				it = memoryBanks.begin();

			// Give 1 block to this bank
			--blocksLeft;
			*it += 1;

			// Increment the iterator
			++it;
		}

		// Add this state to the states
		newState = State(memoryBanks);

		// If the new state already existed, answer has been found
		if (find(states.begin(), states.end(), newState) != states.end())
		{
			// For part 1, just return the cycle counter
			if (!part2)
				return cycleCounter;

			if (hasReset)
				return cycleCounter;

			// Reset the cycle counter and the states to calculate when it is going to reproduce itself
			cycleCounter = 0;
			states.clear();
			hasReset = true;
		}

		// Add the new state to the list
		states.push_back(newState);
	}
}

void Day6::Part1()
{
	cout << "Day 6 Part 1 Answer: " << Solve() << endl;
}

void Day6::Part2()
{
	cout << "Day 6 Part 2 Answer: " << Solve(true) << endl;
}