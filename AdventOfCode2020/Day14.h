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
	vector<int> CalculateAddresses(const string& s);
	vector<string> Possibilities(const string& s);

public:
	void ParseInput();
	ull PartOne();
	ull PartTwo();
};
