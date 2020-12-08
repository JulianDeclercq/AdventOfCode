#include "Day7.h"

void Day7::ParseInput()
{
	if (_inputParsed)
		return;

	//ifstream input("input/day7example.txt");
	ifstream input("input/day7.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	while (getline(input, line))
	{
		regex r("(.+) bags contain (.+).");
		smatch match;
		if (!regex_search(line, match, r))
		{
			cout << "Didn't find regex." << endl;
			continue;
		}
		string mainBag = match[1];
		string contents = static_cast<string>(match[2]);
		ParseBagContents(match[1], contents);
	}

	_inputParsed = true;
}

void Day7::ParseBagContents(const string& bagName, string& contentsDescription)
{
	auto contents = vector<pair<int, string>>();
	if (contentsDescription.compare("contain no other bags") != 0)
	{
		regex r("(\\d+) (.+?) bag");
		auto match = smatch();

		// default constructor = end of sequence
		const auto rend = regex_token_iterator<string::iterator>();
		const int submatches[] = { 1, 2 };
		regex_token_iterator<string::iterator> it(contentsDescription.begin(), contentsDescription.end(), r, submatches);
		while (it != rend)
		{
			// save the amount from current iterator
			int amount = stoi(*it);

			// move iterator to next submatch, the bag name itself
			it++;
			_parentBags[*it].push_back(bagName);

			// add entry
			contents.push_back(make_pair(amount, *it));

			// move iterator to next submatch, the new key if it exists
			it++;
		}
	}
	_bags[bagName] = contents;
}

void Day7::AddToParentBags(const string& bagName, set<string>& parentBags)
{
	for (const auto& parent : _parentBags[bagName])
	{
		parentBags.insert(parent);
		AddToParentBags(parent, parentBags); // recursion
	}
}

int Day7::PartOne()
{
	ParseInput();
	set<string> parents;

	AddToParentBags("shiny gold", parents);
	return parents.size();
}

int Day7::PartTwo()
{
	ParseInput();
	return 0;
}
