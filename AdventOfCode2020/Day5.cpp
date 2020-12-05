#include "Day5.h"

void Day5::ParseInput()
{
	if (_inputParsed)
		return;

	//ifstream input("input/day5example.txt");
	ifstream input("input/day5.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "", current = "";
	while (getline(input, line))
		_boardingPasses.push_back(line);

	_inputParsed = true;
}

int Day5::Partitioning(const range& range, const string& operation, const char lower, const char upper)
{
	if (operation.length() == 1) // last operation
		return operation[0] == lower ? min(range.first, range.second) : max(range.first, range.second);

	if (operation[0] == lower)
	{
		// integer division so rounded down by truncation
		return Partitioning({ range.first, (range.second + range.first) / 2 }, operation.substr(1), lower, upper);
	}
	else if (operation[0] == upper)
	{
		// add +1 to round up
		return Partitioning({ (range.second + range.first + 1) / 2, range.second }, operation.substr(1), lower, upper);
	}
	else
	{
		cout << "invalid operation " << operation[0] << endl;
		return -1;
	}
}

void Day5::CalculateBoardingIDs(const vector<string>& passes, vector<int>& ids)
{
	if (!ids.empty())
	{
		cout << "Id's have already been calculated." << endl;
		return;
	}

	ids.reserve(passes.size());
	for (const string& pass : passes)
	{
		int row = Partitioning({ 0, 127 }, pass.substr(0, 7), 'F', 'B');
		int col = Partitioning({ 0, 7 }, pass.substr(7), 'L', 'R');
		ids.push_back(row * 8 + col); // calculate id and add it to the list
	}
}

int Day5::Part1()
{
	ParseInput();
	CalculateBoardingIDs(_boardingPasses, _boardingIDs);
	return *max_element(_boardingIDs.begin(), _boardingIDs.end());
}

int Day5::Part2()
{
	ParseInput();
	CalculateBoardingIDs(_boardingPasses, _boardingIDs);
	sort(_boardingIDs.begin(), _boardingIDs.end());

	int expected = _boardingIDs[0];
	for (size_t i = 0; i < _boardingIDs.size(); ++i, ++expected)
	{
		if (_boardingIDs[i] != expected)
			return _boardingIDs[i] - 1;
	}
	cout << "Didn't find solution";
	return -1;
}
