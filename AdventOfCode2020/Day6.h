#pragma once

#include <iostream>
#include <regex>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <set>
#include <numeric>

using namespace std;

class Day6
{
private:
	bool _inputParsed = false;
	vector<set<char>> _groupAnswers, _collectiveGroupAnswers;
	void ParseInput();
	void AddGroupAnswer(const string& answer);
	void AddCollectiveGroupAnswer(int memberCount, const string& answer);
public:
	Day6() {};
	int Part1();
	int Part2();
};
