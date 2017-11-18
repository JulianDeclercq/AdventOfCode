#include <iostream>
#include <string>
#include <array>
#include <fstream>
#include <regex>

using namespace std;

const int GRID_WIDTH = 50, GRID_HEIGHT = 6;
//const int GRID_WIDTH = 7, GRID_HEIGHT = 3;
array<bool, GRID_WIDTH * GRID_HEIGHT> Lights = array<bool, GRID_WIDTH * GRID_HEIGHT>();

enum CommandType
{
	RECT,
	ROTATE_COLUMN,
	ROTATE_ROW
};

void CreateRect(int x, int y)
{
	// Command "rect X*Y" turns on all of the pixels in a rectangle at the top-left of the screen which is X wide and Y tall.
	for (int column = 0; column < x; ++column)
	{
		for (int row = 0; row < y; ++row)
		{
			Lights[GRID_WIDTH * row + column] = true;
		}
	}
}

// Shifts a column once
void ShiftColumn(int columnNumber)
{
	// Save the state before shifting
	array<bool, GRID_HEIGHT> startState = array<bool, GRID_HEIGHT>();
	for (int i = 0; i < GRID_HEIGHT; ++i)
	{
		startState[i] = Lights[columnNumber + (i * GRID_WIDTH)];
	}

	// Now that they have all been saved, set the next one to the previous one.
	for (int i = 0; i < GRID_HEIGHT; ++i)
	{
		if (i == GRID_HEIGHT - 1)
		{
			Lights[columnNumber] = startState[i];
		}
		else
		{
			Lights[columnNumber + ((i + 1) * GRID_WIDTH)] = startState[i];
		}
	}
}

void RotateColumn(int columnNumber, int amount)
{
	for (int i = 0; i < amount; ++i)
	{
		ShiftColumn(columnNumber);
	}
}

// Shifts a row once
void ShiftRow(int rowNumber)
{
	// Save the state before shifting
	array<bool, GRID_WIDTH> startState = array<bool, GRID_WIDTH>();
	for (int i = 0; i < GRID_WIDTH; ++i)
	{
		startState[i] = Lights[GRID_WIDTH * rowNumber + i];
	}

	// Now that they have all been saved, set the next one to the previous one.
	// Important to do this in a next loop otherwise all numbers would be the same
	for (int i = 0; i < GRID_WIDTH; ++i)
	{
		// Calculate the index. If I is the last in the row, idx should be reset in order to loop
		size_t idx = (i == GRID_WIDTH - 1) ? GRID_WIDTH * rowNumber : GRID_WIDTH * rowNumber + i + 1;

		// Update the light
		Lights[idx] = startState[i];
	}
}

void RotateRow(int rowNumber, int amount)
{
	// Rotate row y = A by B shifts all of the pixels in row A(0 is the top row) right by B pixels. Pixels that would fall off the right end appear at the left end of the row.
	for (int i = 0; i < amount; ++i)
	{
		ShiftRow(rowNumber);
	}
}

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

void PrintMonitor()
{
	// Print the current state of the lights
	for (int i = 0; i < Lights.size(); ++i)
	{
		// Take a new line for each row
		if (i != 0 && i % GRID_WIDTH == 0)
		{
			cout << endl;
		}

		cout << ((Lights[i]) ? '#' : '.');
	}
	cout << endl << endl;
}

int main()
{
	// Example
/*
	ProcessCommand("rect 3x2");
	PrintMonitor();
	ProcessCommand("rotate column x=1 by 1");
	PrintMonitor();
	ProcessCommand("rotate row y=0 by 4");
	PrintMonitor();
	ProcessCommand("rotate column x=1 by 1");
	PrintMonitor();
	ProcessCommand("rotate column x=1 by 4");
	PrintMonitor();
	cin.get();
	return 0;*/

	// Open the inputfile
	ifstream input("input.txt");

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
		cout << line << endl;
		PrintMonitor();
	}

	int activeCtr = 0;
	for (bool b : Lights)
		if (b) ++activeCtr;

	cout << "There are " << activeCtr << " lights active.\n";

	cin.get();

	return 0;
}