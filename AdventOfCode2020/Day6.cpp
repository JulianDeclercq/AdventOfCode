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
	while (getline(input, line))
	{
		if (!line.empty()) // groups are separated by blank lines
		{
			current += line;
			continue;
		}

		AddGroupAnswer(current);
		current = "";
	}

	// make sure last one gets parsed without input manipulation
	if (!current.empty())
		AddGroupAnswer(current);

	_inputParsed = true;
}

void Day6::AddGroupAnswer(const string& answer)
{
	set<char> groupAnswers;

	for (char c : answer)
		groupAnswers.insert(c);

	_groupAnswers.push_back(groupAnswers);
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
	return 0;
}
