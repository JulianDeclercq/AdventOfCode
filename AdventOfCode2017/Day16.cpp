#include "Day16.h"

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