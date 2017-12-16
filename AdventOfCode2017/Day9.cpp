#include "Day9.h"

std::string Day9::ParseInput()
{
	ifstream input("Input/Day9.txt");
	if (input.fail())
	{
		cout << "failed inputfile" << endl;
		return "";
	}

	string line;
	getline(input, line);

	return line;
}

void Day9::RemoveIgnores(string& str)
{
	// Find the index of the ignore character
	size_t idx = str.find('!');
	while (idx != string::npos)
	{
		// Erase the ignore character and the following character
		str.erase(idx, 2);

		// Update the index
		idx = str.find('!');
	}
}

void Day9::RemoveJunk(string& str)
{
	// Find the index of the open junk character
	size_t junkOpen = str.find('<');
	while (junkOpen != string::npos)
	{
		// Find the index of the close junk character
		size_t junkClose = str.find('>');

		// Erase everything between junk open and junk close character
		str = str.erase(junkOpen, junkClose - junkOpen + 1);

		// Update the index
		junkOpen = str.find('<');
	}
}

void Day9::Part1()
{
	string input = ParseInput();
	vector<CharacterGroup*> groups = vector<CharacterGroup*>();

	// Remove all the "cleaned" parts
	RemoveIgnores(input);

	// Remove all junk from the string
	RemoveJunk(input);

	// Find the closing character of a group
	size_t closedIdx = input.find('}');
	size_t openIdx = 0;
	while (closedIdx != string::npos)
	{
		// Find the last closing index until the point of the opening index
		openIdx = input.substr(0, closedIdx).rfind('{');

		// Create a new group with these boundaries
		CharacterGroup* group = new CharacterGroup(openIdx, closedIdx);

		// Add the group to the vector
		groups.push_back(group);

		// Replace the bound characters with dummy characters so they don't disturb the other searches
		input.replace(openIdx, 1, "@");
		input.replace(closedIdx, 1, "@");

		// Update the index to find the next group
		closedIdx = input.find('}');
	}

	// Nest loop through the groups and check their boundaries.
	// Add those that lie within others to the others' children
	for (CharacterGroup* group1 : groups)
	{
		for (CharacterGroup* group2 : groups)
		{
			if (group1->StartIdx < group2->StartIdx && group2->EndIdx < group1->EndIdx)
			{
				group1->Children.push_back(group2);
			}
		}
	}

	// Find the main/outer group, which is the group with the most children
	CharacterGroup* mainGroup = groups[0];
	for (int i = 1; i < groups.size(); ++i)
	{
		if (groups[i]->Children.size() > mainGroup->Children.size())
			mainGroup = groups[i];
	}

	// Calculate the score for all groups
	mainGroup->CalculateScores();

	// Calculate the sum of scores
	int sum = 0;
	for (CharacterGroup* group : groups)
		sum += group->Score;

	cout << "answer is " << sum << endl;
}