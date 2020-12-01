#pragma once

#include <string>
#include <iostream>
#include <fstream>
#include <vector>

using namespace std;

class Day1
{
private:
	bool _inputParsed = false;
	vector<int> _expenses = vector<int>();

	void ParseInput();

public:
	Day1(){}
	int Part1();
	int Part2();
};
