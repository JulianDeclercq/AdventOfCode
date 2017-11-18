#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <regex>

using namespace std;

void CreateRect(int x, int y)
{
}

void RotateColumn(int columnNumber, int amount)
{
}

void RotateRow(int rowNumber, int amount)
{
}

enum CommandType
{
	RECT,
	ROTATE_COLUMN,
	ROTATE_ROW
};

CommandType DetermineCommandType(const string& command)
{
	/*
		/ Command examples /
		rect 3x2
		rotate column x=1 by 1
		rotate row y=0 by 4
	*/
	if (command[1] == 'e')
	{
		return RECT;
	}
	else if (command[7] == 'c')
	{
		return ROTATE_COLUMN;
	}
	else if (command[7] == 'r')
	{
		return ROTATE_ROW;
	}
	else
	{
		cout << "INVALID COMMANDTYPE!!" << endl;
	}
}

void ProcessCommand(const string& command)
{
	// Determine which type of command it is
	CommandType commandType = DetermineCommandType(command);

	// Define the expression and a string match object
	// If the command is a rect type, use a different regex than the rotate types
	// Expression using Raw string literals
	regex expression = (commandType == RECT) ? regex(R"(rect (\d+)x(\d+))") : regex(R"(.+=(\d+).+(\d+))");
	smatch match;

	// Check for regex match
	regex_match(command, match, expression);

	// Match error check
	if (match.empty())
	{
		cout << "Error parsing regex.\n";
		return;
	}

	// Extract the arguments from the match
	// match[0] is the full string that matched and thus useless in this case
	int firstArgument = stoi(match[1]);
	int secondArgument = stoi(match[2]);

	// Execute the corresponding methods
	switch (commandType)
	{
	case RECT: CreateRect(firstArgument, secondArgument); break;
	case ROTATE_ROW: RotateRow(firstArgument, secondArgument); break;
	case ROTATE_COLUMN: RotateColumn(firstArgument, secondArgument); break;
	}
}

int main()
{
	//DEBUG

	//ProcessCommand("rect 195x3932");
	ProcessCommand("rotate column x=30 by 1");
	return 0;

	// Open the inputfile
	ifstream input("example.txt");

	// Error check for the input file
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return -1;
	}

	// Retrieve all lines and execute them as commands
	string line;
	while (getline(input, line))
	{
		ProcessCommand(line);
	}

	return 0;
}