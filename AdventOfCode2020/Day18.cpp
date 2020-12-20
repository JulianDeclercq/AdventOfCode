#include "Day18.h"

void Day18::ParseInput()
{
	//ifstream input("input/day17example.txt");
	/*ifstream input("input/day17.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	while (getline(input, line))
	{
	}*/

	const string s("1 + (2 * 3) + (4 * (5 + 6))");
	vector<string> results;
	for (size_t i = 0; i < s.size(); ++i)
	{
		if (s[i] == '(')
		{
			_parentheses.push({s[i], i});
		}
		else if (s[i] == ')')
		{
			if (!_parentheses.empty() && _parentheses.top().first == '(')
			{
				results.push_back(s.substr(_parentheses.top().second + 1, i - _parentheses.top().second - 1));
				_parentheses.pop();
			}
		}
	}

	int brkpt = 6;
}

int Day18::PartOne()
{
	return 0;
}

int Day18::PartTwo()
{
	return 0;
}
