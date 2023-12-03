#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <stack>

using namespace std;

class Day18
{
private:
	stack<pair<char, size_t>> _parentheses;
public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};