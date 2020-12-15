#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <regex>
#include <bitset>
#include <map>
#include <numeric>

using namespace std;
using ull = unsigned long long;

class Day14
{
private:
	map<ull, ull> _memory;
	vector<string> _instructions;
	string _mask;
	ull ApplyMask(ull target);
	string ApplyMask2(const string& mask, ull target);
	vector<string> Possibilities(const string& s);
	ull ValueSumAfterExecution(bool part1);

public:
	void ParseInput();
	ull PartOne();
	ull PartTwo();
};
