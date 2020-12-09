#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <regex>

using namespace std;
using instruction = pair<string, int>;

class Day8
{
private:
	vector<instruction> _instructions;
	vector<bool> _visited;
	int _accumulator = 0;
	int _instructionIdx = 0;
	int _repeatedInstructionIdx = -1;

	bool ExecuteInstruction(int instructionIdx);
public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};
