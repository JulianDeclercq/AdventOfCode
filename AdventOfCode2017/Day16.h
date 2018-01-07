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
	char _programOrder[GROUP_SIZE];

	void ExecuteMove2(const string& move);
	void PrintProgramOrder();

	int ITERATIONS = 10000000;
	string COMMAND = "pe/d";
public:
	void Part1();
	void Part1Test();
	void Part2();
};