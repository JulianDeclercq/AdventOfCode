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

		// parse the instruction type
		InstructionType type = InstructionType::INVALID;
		if (match[1].compare("acc") == 0)
		{
			type = InstructionType::ACC;
		}
		else if (match[1].compare("jmp") == 0)
		{
			type = InstructionType::JMP;
		}
		else if (match[1].compare("nop") == 0)
		{
			type = InstructionType::NOP;
		}

		// check the sign for the argument
		char sign = static_cast<string>(match[2])[0];

		// add the instruction
		_instructions.push_back({ type, stoi(match[3]) * (sign == '-' ? -1 : 1) });
	}
	_visited.resize(_instructions.size(), false);
}

void Day8::ExecuteNext()
{
	// exit check
	if (_visited[_instructionIdx])
	{
		_repeatedInstruction = true;
		return;
	}

	// mark this instruction as visited
	_visited[_instructionIdx] = true;

	// process the instruction
	const auto& instruction = _instructions[_instructionIdx];
	switch (instruction.first)
	{
		case InstructionType::ACC:
			_accumulator += instruction.second;
			++_instructionIdx;
			break;

		case InstructionType::JMP:
			_instructionIdx += instruction.second;
			break;

		case InstructionType::NOP:
			++_instructionIdx;
			break;

		case InstructionType::INVALID:
		default: cout << "Invalid instruction type" << endl;
			break;
	}
}

void Day8::Reset()
{
	_accumulator = 0;
	_instructionIdx = 0;
	_repeatedInstruction = false;
	for (size_t i = 0; i < _visited.size(); ++i)
		_visited[i] = false;
}

int Day8::PartOne()
{
	Reset();

	while (!_repeatedInstruction)
		ExecuteNext();

	return _accumulator;
}

int Day8::PartTwo()
{
	Reset();

	for (auto& instruction : _instructions)
	{
		// ignore acc operations
		if (instruction.first == InstructionType::ACC)
			continue;

		// save the original instruction
		_originalType = instruction.first;

		// switch out the instruction with the counterpart
		instruction.first = (_originalType == InstructionType::JMP) ? InstructionType::NOP : InstructionType::JMP;

		// reset
		Reset();

		// run the program
		while (!_repeatedInstruction)
		{
			ExecuteNext();

			// SUCCESS! If the program has terminated successfully, return the accumulator value
			if (_instructionIdx == _instructions.size())
				return _accumulator;
		}

		// a repeated instruction was found, swap the original instruction back in and continue
		instruction.first = _originalType;
	}
	return -1;
}
