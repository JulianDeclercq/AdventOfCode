#include "Day6.h"

void Day6::ParseInput()
{
	if (_inputParsed)
		return;

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

		AddGroupAnswer(current);
		AddCollectiveGroupAnswer(memberCount, current);

		memberCount = 0;
		current = "";
	}

	// make sure last one gets parsed without input manipulation
	if (!current.empty())
	{
		AddGroupAnswer(current);
		AddCollectiveGroupAnswer(memberCount, current);
	}

	_inputParsed = true;
}

void Day6::AddGroupAnswer(const string& answer)
{
	set<char> groupAnswers;

	for (char c : answer)
		groupAnswers.insert(c);

	_groupAnswers.push_back(groupAnswers);
}

void Day6::AddCollectiveGroupAnswer(int memberCount, const string& answer)
{
	set<char> groupAnswers;

	for (char c : answer)
	{
		// if everyone answered yes
		if (count(answer.begin(), answer.end(), c) == memberCount)
			groupAnswers.insert(c);
	}

	_collectiveGroupAnswers.push_back(groupAnswers);
}

int Day6::Part1()
{
	ParseInput();
	int count = 0;
	for (const auto& answer : _groupAnswers)
		count += answer.size();
	
	return count;
}

int Day6::Part2()
{
	ParseInput();
	int count = 0;
	for (const auto& answer : _collectiveGroupAnswers)
		count += answer.size();

	return count;
}
