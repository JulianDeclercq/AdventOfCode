#include "Day7.h"

void Day7::ParseInput()
{
	//ifstream input("input/day7example.txt");
	//ifstream input("input/day7example2.txt");
	ifstream input("input/day7.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	regex r("(.+) bags contain (.+).");
	smatch match;
	while (getline(input, line))
	{
		if (!regex_search(line, match, r))
		{
			cout << "Didn't find regex." << endl;
			continue;
		}
		ParseBagContents(match[1], match[2]);
	}
}

void Day7::ParseBagContents(const string& bagName, string contentsDescription)
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

int Day7::ContentCount(const string& bagName, bool root)
{
	int total = root ? 0 : 1;
	
	for (const auto& bag : _bags[bagName])
		total += bag.first * ContentCount(bag.second);
	
	return total;
}

int Day7::PartOne()
{
	set<string> parents;
	AddToParentBags("shiny gold", parents);
	return parents.size();
}

int Day7::PartTwo()
{
	return ContentCount("shiny gold", true);
}
