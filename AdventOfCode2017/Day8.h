#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <regex>
#include <map>
#include <utility>
#include <algorithm>

using namespace std;

class Day8
{
private:
	map<string, int> _registers = map<string, int>();

	void ParseCommand(const string& command);
	bool EvaluateCondition(const string& op, const string& arg1, const string& arg2);

public:
	void Part1();
	void Part2();
};
