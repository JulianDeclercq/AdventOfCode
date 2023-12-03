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
using DeclaredGoodsGroups = vector<set<char>>;

class Day6
{
private:
	DeclaredGoodsGroups _anyoneGroups, _everyoneGroups;

	void AddGroupAnswerAnyone(const string& answer, DeclaredGoodsGroups& groups);
	void AddGroupAnswerEveryone(const string& answer, int memberCount, DeclaredGoodsGroups& groups);
	int TotalGoodsToDeclare(DeclaredGoodsGroups& groups);

public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};
