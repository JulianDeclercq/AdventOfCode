#include "Day4.h"

void Day4::Part1()
{
	ifstream input("Input/Day4.txt");
	//ifstream input("Example/Day4Part1.txt");
	if (input.fail())
	{
		cout << "Failed to open input file\n";
		return;
	}

	string line;
	int validPasswords = 0;
	while (getline(input, line))
	{
		// Retrieve the words that are split by a space
		vector<string> words = Helpers::Split(line, ' ');

		// Sort the words
		sort(words.begin(), words.end());

		// After sorting, search for adjacent elements
		auto it = adjacent_find(words.begin(), words.end());

		// If no adjacent elements were found, it means there are no duplicates
		if (it == words.end())
			++validPasswords;
	}

	cout << "Day 4 Part 2 answer is: " << validPasswords << endl;
}

void Day4::Part2()
{
}