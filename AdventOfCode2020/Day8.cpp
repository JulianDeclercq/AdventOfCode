#include "Day8.h"

void Day8::ParseInput()
{
	//ifstream input("input/day8example.txt");
	ifstream input("input/day8.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	regex r("(\\w+) (.)(\\d+)");
	smatch match;
	while (getline(input, line))
	{
		if (!regex_search(line, match, r))
		{
			cout << "Didn't find regex." << endl;
			continue;
		}
		char sign = static_cast<string>(match[2])[0];
		_instructions.push_back({ match[1], stoi(match[3]) * (sign == '-' ? -1 : 1) });
	}
	_visited.resize(_instructions.size(), false);
}

bool Day8::ExecuteInstruction(int idx)
{
	// exit check
	if (_visited[idx])
		return false;

	// mark this instruction as visited
	_visited[idx] = true;

	// process the instruction
	const auto& instruction = _instructions[idx];
	if (instruction.first.compare("nop") == 0)
	{
		++_instructionIdx;
	}
	else if (instruction.first.compare("acc") == 0)
	{
		_accumulator += instruction.second;
		++_instructionIdx;
	}
	else if (instruction.first.compare("jmp") == 0)
	{
		_instructionIdx += instruction.second; //?!?!?
	}
	else
	{
		cout << "Invalid instruction: " << instruction.first << endl;
	}

	return true;
}

int Day8::PartOne()
{
	while (ExecuteInstruction(_instructionIdx));
	return _accumulator;
}

int Day8::PartTwo()
{
	return 0;
}
