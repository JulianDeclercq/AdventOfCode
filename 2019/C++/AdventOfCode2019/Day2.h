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
	// variables
	int _instructionPointer = 0;

	// methods
	void parse_input();

	vector<int> _intcode = vector<int>();
	vector<int> _originalIntcode = vector<int>();

	const int _desiredOutput = 19690720;

public:
	// constructor
	Day2() {};

	void part1();
	void progress();
	void print();

	void part2();
	void reset();

};
