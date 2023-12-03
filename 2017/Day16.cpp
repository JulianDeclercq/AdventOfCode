#include "Day16.h"

std::vector<std::string> Day16::ParseInput(string& programOrder)
{
	// Starting order of the programs
	programOrder.resize(GROUP_SIZE);
	for (int i = 0; i < GROUP_SIZE; ++i)
		programOrder[i] = (static_cast<char>('a' + i));

	// Read commands from file
	ifstream input("Input/Day16.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return{};
	}

	string line;
	getline(input, line);
	return Helpers::Split(line, ',');
}

void Day16::ExecuteMove(string& programOrder, const string& move)
{
	smatch match;
	string newProgramOrder = "";

	// Execute appropriate dance move
	switch (move[0])
	{
	case 's':
	{
		regex_match(move, match, regex(R"(s(\d+))"));

		// Calculate the offset
		int spinOffset = programOrder.size() - stoi(match[1]);

		// Form the new program order
		newProgramOrder = programOrder.substr(spinOffset);
		newProgramOrder += programOrder.substr(0, spinOffset);

		// Update the programOrder
		programOrder = newProgramOrder;
	}
	break;

	case 'x':
	{
		regex_match(move, match, regex(R"(x(\d+)\/(\d+))"));
		swap(programOrder[stoi(match[1])], programOrder[stoi(match[2])]);
	}
	break;

	case 'p':
	{
		regex_match(move, match, regex(R"(p(\w)\/(\w))"));

		// Locate the programs that need to be swapped
		size_t firstProgramPosition = programOrder.find_first_of(match[1]);
		size_t secondProgramPosition = programOrder.find_first_of(match[2]);

		// Swap the programs
		swap(programOrder[firstProgramPosition], programOrder[secondProgramPosition]);
	}
	break;

	default: cout << "Invalid move: " << move[0] << endl; break;
	}
}

void Day16::ExecuteMoveOptimised(string& programOrder, const string& move)
{
	string newOrder;
	newOrder.resize(GROUP_SIZE);

	// Execute appropriate dance move
	switch (move[0])
	{
	case 's':
	{
		// Determine X
		int x = (move.size() == 2) ? (move[1] - '0') : ((move[1] - '0') * 10 + (move[2] - '0'));

		// Move all characters starting from X to the front
		for (int i = 0; i < x; ++i)
			newOrder[i] = programOrder[i + GROUP_SIZE - x];

		// Move all characters that were in the front to the back
		for (int i = 0; i < GROUP_SIZE - x; ++i)
			newOrder[i + x] = programOrder[i];

		// Update the programOrder
		for (int i = 0; i < GROUP_SIZE; ++i)
			programOrder[i] = newOrder[i];
	}
	break;

	case 'x':
	{
		// Determine which programs need to be swapped
		int a, b;
		if (move[2] == '/')
		{
			a = (move[1] - '0');

			// Determine b, knowing that a is only 1 character
			b = (move.size() == 4) ? (move[3] - '0') : ((move[3] - '0') * 10 + (move[4] - '0'));
		}
		else
		{
			a = ((move[1] - '0') * 10 + (move[2] - '0'));

			// Determine b, knowing that a is 2 characters
			b = (move.size() == 5) ? (move[4] - '0') : ((move[4] - '0') * 10 + (move[5] - '0'));
		}

		// Swap the programs
		char temp = programOrder[a];
		programOrder[a] = programOrder[b];
		programOrder[b] = temp;
	}
	break;

	case 'p':
	{
		char a = move[1], b = move[3];
		int aIdx = -1, bIdx = -1;

		// Find the indexes of the programs
		for (int i = 0; aIdx < 0 || bIdx < 0; ++i)
		{
			if (programOrder[i] == a)
				aIdx = i;

			if (programOrder[i] == b)
				bIdx = i;
		}

		// Swap the programs
		char temp = programOrder[aIdx];
		programOrder[aIdx] = programOrder[bIdx];
		programOrder[bIdx] = temp;
	}
	break;

	default: cout << "Invalid move: " << move[0] << endl; break;
	}
}

void Day16::Part1()
{
	// Parse the input
	string programOrder = "";
	vector<string> moves = ParseInput(programOrder);

	// Execute all moves
	for (const string& move : moves)
		ExecuteMoveOptimised(programOrder, move);

	cout << "Day 16 Part 1 answer: " << programOrder << endl;
}

void Day16::Part2()
{
	// Parse the input
	string programOrder = "";
	vector<string> moves = ParseInput(programOrder);

	// Now that the starting order has been set and the input has been parsed,
	// Take a copy to save as starting order
	string startingOrder = string(programOrder);

	// Determine at which step the sequence repeats itself
	int step = 0;
	do
	{
		// Execute all the moves
		for (const string& move : moves)
			ExecuteMoveOptimised(programOrder, move);

		// Increment the step
		++step;
	} while (programOrder.compare(startingOrder) != 0);

	// Execute all moves for one step
	// As steps repeat this means that the order after these iterations will be equal to the order after 1 billion iterations
	for (int i = 0; i < (1000000000 % step); ++i)
	{
		// Execute all the moves
		for (const string& move : moves)
			ExecuteMoveOptimised(programOrder, move);
	}

	cout << "Day 16 Part 2 answer: " << programOrder << endl;
}