#include "Day6.h"

void Day6::ParseInput()
{
	//ifstream input("input/day6example.txt");
	ifstream input("input/day6.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "", current = "";
	int memberCount = 0;
	while (getline(input, line))
	{
		if (!line.empty()) // groups are separated by blank lines
		{
			current += line;
			++memberCount;
			continue;
		}

		AddGroupAnswerAnyone(current, _anyoneGroups);
		AddGroupAnswerEveryone(current, memberCount, _everyoneGroups);

		memberCount = 0;
		current = "";
	}

	// make sure last one gets parsed without input manipulation
	if (!current.empty())
	{
		AddGroupAnswerAnyone(current, _anyoneGroups);
		AddGroupAnswerEveryone(current, memberCount, _everyoneGroups);
	}
}

void Day6::AddGroupAnswerAnyone(const string& answer, DeclaredGoodsGroups& groups)
{
	set<char> goodsToDeclare;

	// if anyone answered yes, it should be declared
	for (char c : answer)
		goodsToDeclare.insert(c);

	groups.push_back(goodsToDeclare);
}

void Day6::AddGroupAnswerEveryone(const string& answer, int memberCount, DeclaredGoodsGroups& groups)
{
	set<char> goodsToDeclare;

	for (char c : answer)
	{
		// if everyone answered yes
		if (count(answer.begin(), answer.end(), c) == memberCount)
			goodsToDeclare.insert(c);
	}

	groups.push_back(goodsToDeclare);
}

// Calculates the total amount of goods to declare for all groups in the plane
int Day6::TotalGoodsToDeclare(DeclaredGoodsGroups& groups)
{
	int count = 0;

	for (const auto& declaredGoods : groups)
		count += declaredGoods.size();

	return count;
}

int Day6::PartOne()
{
	ParseInput();
	return TotalGoodsToDeclare(_anyoneGroups);
}

int Day6::PartTwo()
{
	ParseInput();
	return TotalGoodsToDeclare(_everyoneGroups);
}
