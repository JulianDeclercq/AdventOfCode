#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <regex>

using namespace std;

enum class InstructionType
{
	INVALID,
	ACC,
	JMP,
	NOP
};
using instruction = pair<InstructionType, int>;

class Day8
{
private:

	vector<instruction> _instructions;
	vector<bool> _visited;
	int _instructionIdx = 0;
	int _accumulator = 0;
	bool _repeatedInstruction = false;
	InstructionType _originalType = InstructionType::INVALID;

	void ExecuteNext();
	void Reset();
public:

	void ParseInput();
	int PartOne();
	int PartTwo();
};
