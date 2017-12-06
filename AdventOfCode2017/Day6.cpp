#include "Day6.h"

std::string Day6::State(const vector<int>& memoryBanks)
{
	string state = "";
	for (int bank : memoryBanks)
		state += to_string(bank) + " ";

	return state;
}

void Day6::Part1()
{
	//	ifstream input("Example/Day6Part1.txt");
	ifstream input("Input/Day6.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile.\n";
		return;
	}

	string line;
	getline(input, line);

	// Parse the input to a vector of int
	vector<int> memoryBanks = vector<int>();
	for (const string& memoryBankString : Helpers::Split(line, ' '))
		memoryBanks.push_back(stoi(memoryBankString));

	// Create a vector of all states that will be achieved
	vector<string> states = vector<string>{ State(memoryBanks) };

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
			//for (const auto& lel : states) cout << lel << endl;

			cout << "Day 6 Part 1 Answer: " << cycleCounter << endl;
			return;
		}

		// Add the new state to the list
		states.push_back(newState);
	}
}

void Day6::Part2()
{
}