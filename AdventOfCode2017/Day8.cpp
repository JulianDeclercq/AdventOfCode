#include "Day8.h"

bool Day8::EvaluateCondition(const string& operatorString, const string& arg1, const string& arg2)
{
	if (operatorString.compare("==") == 0) return _registers[arg1] == stoi(arg2);
	if (operatorString.compare("<=") == 0) return _registers[arg1] <= stoi(arg2);
	if (operatorString.compare(">=") == 0) return _registers[arg1] >= stoi(arg2);
	if (operatorString.compare("!=") == 0) return _registers[arg1] != stoi(arg2);
	if (operatorString.compare(">") == 0)  return _registers[arg1] > stoi(arg2);
	if (operatorString.compare("<") == 0)  return _registers[arg1] < stoi(arg2);

	cout << "Invalid operator: " << operatorString << endl;
	return false;
}

void Day8::ParseCommand(const string& command)
{
	smatch match;
	if (!regex_match(command, match, regex(R"((\w+) (\w+) (-?\d+) if (\w+) (.+) (-?\d+))")))
	{
		cout << "Invalid command: " << command << endl;
		return;
	}

	// Parse and evaluate the condition
	if (!EvaluateCondition(match[5], match[4], match[6]))
		return;

	// Execute the increment or decrement
	(match[2].compare("inc") == 0) ? _registers[match[1]] += stoi(match[3]) : _registers[match[1]] -= stoi(match[3]);
}

void Day8::Part1()
{
	//	ifstream input("Example/Day8Part1.txt");
	ifstream input("Input/Day8.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	string line;
	while (getline(input, line))
		ParseCommand(line);

	// Find the register with the largest value by sorting descending on value
	int highest = 0;
	for (const auto& reg : _registers)
	{
		if (reg.second > highest)
			highest = reg.second;
	}

	cout << "Day 8 Part 1 Answer: " << highest << endl;
}

void Day8::Part2()
{
}