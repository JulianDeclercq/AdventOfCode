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
	static const int GROUP_SIZE = 16;
	vector<string> ParseInput(string& programOrder);
	void ExecuteMove(string& programOrder, const string& move); // Runs part 2 in approx 2192ms
	void ExecuteMoveOptimised(string& programOrder, const string& move); // Runs part 2 in approx 52ms, wrote this before I realized i could cut a huge corner by calculating the step first

public:
	void Part1();
	void Part2();
};