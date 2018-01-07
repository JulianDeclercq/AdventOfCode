#pragma once
#include <iostream>
#include <string>
#include <fstream>
#include <vector>
#include <regex>
#include <algorithm>
#include "Helpers.h"

using namespace std;

class Day16
{
private:
	void ExecuteMove(string& programOrder, const string& move);

	// Part 2
	static const int GROUP_SIZE = 16;
	string _programOrder;

	void ExecuteMove2(const string& move);
	void PrintProgramOrder();

public:
	void Part1();
	void Part1Test();
	void Part2();
};