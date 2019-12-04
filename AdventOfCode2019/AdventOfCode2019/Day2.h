#pragma once
#include <iostream>
#include <string>
#include <fstream>
#include <regex>
#include <vector>

using namespace std;

class Day2
{
private:
	// structs
	struct Operation 
	{
		Operation(int opcode, int idx1, int idx2, int targetIdx)
			: Opcode(opcode), Index1(idx1), Index2(idx2), TargetIndex(targetIdx)
		{

		}

		int Opcode = -1;
		int Index1 = -1;
		int Index2 = -1;
		int TargetIndex = -1;
	};

	// variables
	int _idx = 0;

	// methods
	void parse_input();

	vector<int> _intcode = vector<int>();

public:
	// constructor
	Day2() {};

	void part1();
	void progress();

	void part2();
};
