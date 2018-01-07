#include "Day16.h"
#include <time.h>

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
void Day16::ExecuteMove2(const string& move)
{
	char newOrder[GROUP_SIZE];

	// Execute appropriate dance move
	switch (move[0])
	{
	case 's':
	{
		// Determine X
		int x = (move.size() == 2) ? (move[1] - '0') : ((move[1] - '0') * 10 + (move[2] - '0'));

		// Move all characters starting from X to the front
		for (int i = 0; i < x; ++i)
			newOrder[i] = _programOrder[i + GROUP_SIZE - x];

		// Move all characters that were in the front to the back
		for (int i = 0; i < GROUP_SIZE - x; ++i)
			newOrder[i + x] = _programOrder[i];

		// Update the programOrder
		for (int i = 0; i < GROUP_SIZE; ++i)
			_programOrder[i] = newOrder[i];
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
		char temp = _programOrder[a];
		_programOrder[a] = _programOrder[b];
		_programOrder[b] = temp;
	}
	break;

	case 'p':
	{
		char a = move[1], b = move[3];
		int aIdx = -1, bIdx = -1;

		// Find the indexes of the programs
		for (int i = 0; aIdx < 0 || bIdx < 0; ++i)
		{
			if (_programOrder[i] == a)
				aIdx = i;

			if (_programOrder[i] == b)
				bIdx = i;
		}

		// Swap the programs
		char temp = _programOrder[aIdx];
		_programOrder[aIdx] = _programOrder[bIdx];
		_programOrder[bIdx] = temp;
	}
	break;

	default: cout << "Invalid move: " << move[0] << endl; break;
	}
}

void Day16::Part1()
{
	// Starting state of the programs
	string programOrder = "";
	for (int i = 0; i <= 15; ++i)
		programOrder += (static_cast<char>('a' + i));

	// Read commands from file
	//ifstream input("Example/Day16Part1.txt");
	ifstream input("Input/Day16.txt");

	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	string line;
	getline(input, line);
	vector<string> moves = Helpers::Split(line, ',');

	for (const string& move : moves)
		ExecuteMove(programOrder, move);

	cout << "Day 16 Part 1 answer: " << programOrder << endl;
}

void Day16::PrintProgramOrder()
{
	for (char c : _programOrder)
		cout << c;

	cout << endl;
}

void Day16::Part2()
{
	cout << "Group size is: " << GROUP_SIZE << endl;
	// Starting state of the programs
	for (int i = 0; i < GROUP_SIZE; ++i)
		_programOrder[i] = (static_cast<char>('a' + i));

	// Read commands from file
	//ifstream input("Example/Day16Part1.txt");
	ifstream input("Input/Day16.txt");

	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	string line;
	getline(input, line);
	vector<string> moves = Helpers::Split(line, ',');

	clock_t clkStart = clock();
	for (int i = 0; i < 1000000000; ++i)
	{
		if (i % 100000 == 0)
			cout << "Execution time for " << i << " iterations is: " << (clock() - clkStart) / 1000.0f << " seconds" << endl;

		for (const string& move : moves)
			ExecuteMove2(move);
	}

	cout << "OPTIMIZED answer: ";
	PrintProgramOrder();
}

void Day16::Part1Test()
{
	cout << "Number of iterations: " << ITERATIONS << endl;
	// Starting state of the programs
	string programOrder = "";
	for (int i = 0; i < GROUP_SIZE; ++i)
		programOrder += (static_cast<char>('a' + i));

	for (int i = 0; i < ITERATIONS; ++i)
		ExecuteMove(programOrder, COMMAND);

	cout << "answer: " << programOrder << endl;
}