#include "Day7.h"

void Day7::ExecuteCommand(const string& command)
{
	smatch match;

	// Check which command it is using regular expressions
	// Check for NOT pattern
	// e.g. "NOT e -> f"
	if (regex_match(command, match, regex(R"(NOT (\w) -> (\w))")))
	{
		// Group 2 gets the bitwise complement of the value from wire group 1 assigned
		int copy = _wireMapping[string(match[1])];
		_wireMapping[string(match[2])] = ~copy;
	}
	// Check for AND, OR, LSHIFT or RSHIFT patterns
	else if (regex_match(command, match, regex(R"((\w) (\w+) (\w) -> (\w))")))
	{
		string type(match[2]);

		// Bitwise AND between group 1 and group 3 is assigned to group 4
		// e.g. "x AND y -> d"
		if (type.compare("AND") == 0)
		{
			_wireMapping[string(match[4])] = _wireMapping[string(match[1])] & _wireMapping[string(match[3])];
		}
		// Bitwise OR between group 1 and group 3 is assigned to group 4
		// e.g. "x OR y -> d"
		else if (type.compare("OR") == 0)
		{
			_wireMapping[string(match[4])] = _wireMapping[string(match[1])] | _wireMapping[string(match[3])];
		}
		// Value from wire group 1 is left shifted by integer value of group 3 and assigned to group 4
		// e.g. "p LSHIFT 2 -> q"
		else if (type.compare("LSHIFT") == 0)
		{
			_wireMapping[string(match[4])] = _wireMapping[string(match[1])] << stoi(match[3]);
		}
		// Value from wire group 1 is right shifted by integer value of group 3 and assigned to group 4
		// e.g. "p RSHIFT 2 -> q"
		else if (type.compare("RSHIFT") == 0)
		{
			_wireMapping[string(match[4])] = _wireMapping[string(match[1])] >> stoi(match[3]);
		}
		else cout << "Invalid command: " << command << endl;
	}
	// Check for simple assignment commands
	else if (regex_match(command, match, regex(R"((\d+) -> (\w))")))
	{
		// Make sure to check this as last, as this pattern will match the last part of SHIFT commands
		// Value of group 1 is assigned to group 2
		// e.g. "123 -> x"
		_wireMapping[string(match[2])] = stoi(match[1]);
	}
}

void Day7::Part1()
{
	//	ifstream input("Example/Day7Part1.txt");
	ifstream input("Input/Day7.txt");
	if (input.fail())
	{
		cout << "Failed to open input file.\n";
		return;
	}

	string line;
	while (getline(input, line))
		ExecuteCommand(line);

	for (const auto& wire : _wireMapping)
		cout << wire.first << ' ' << wire.second << endl;

	cout << "Day 7 Part 1 answer: " << _wireMapping["a"] << endl;
}

void Day7::Part2()
{
}