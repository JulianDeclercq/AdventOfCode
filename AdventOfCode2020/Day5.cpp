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
	}
}

int Day5::Part1()
{
	ParseInput();
	int highest = 0;
	for (const string& boardingPass : _boardingPasses)
	{
		int row = Partitioning({ 0, 127 }, boardingPass.substr(0, 7), 'F', 'B');
		int col = Partitioning({ 0, 7 }, boardingPass.substr(7), 'L', 'R');
		int seatID = row * 8 + col;
		if (seatID > highest)
			highest = seatID;
	}
	return highest;
}

int Day5::Part2()
{
	ParseInput();

	auto ids = vector<int>();
	ids.reserve(_boardingPasses.size());
	for (const string& boardingPass : _boardingPasses)
	{
		int row = Partitioning({ 0, 127 }, boardingPass.substr(0, 7), 'F', 'B');
		int col = Partitioning({ 0, 7 }, boardingPass.substr(7), 'L', 'R');
		ids.push_back(row * 8 + col);
	}
	sort(ids.begin(), ids.end());

	int expected = ids[0];
	for (int i = 0; i < ids.size(); ++i, ++expected)
	{
		if (ids[i] != expected)
			return ids[i] - 1;
	}
	cout << "Didn't find solution";
	return -1;
}
