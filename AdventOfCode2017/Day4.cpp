#include "Day4.h"

int Day4::Solve(bool part2)
{
	ifstream input("Input/Day4.txt");
	if (input.fail())
	{
		cout << "Failed to open input file\n";
		return -1;
	}

	string line;
	int validPasswords = 0;
	while (getline(input, line))
	{
		// Retrieve the words that are split by a space
		vector<string> words = Helpers::Split(line, ' ');

		// If part 2, sort the characters of the word to find anagrams
		if (part2)
		{
			for (string& word : words)
				sort(word.begin(), word.end());
		}

		// Sort the words
		sort(words.begin(), words.end());

		// After sorting, search for adjacent elements
		auto it = adjacent_find(words.begin(), words.end());

		// If no adjacent elements were found, it means there are no duplicates
		if (it == words.end())
			++validPasswords;
	}

	return validPasswords;
}

void Day4::Part1()
{
	int validPasswords = Solve();
	cout << "Day 4 Part 1 answer is: " << validPasswords << endl;
}

void Day4::Part2()
{
	int validPasswords = Solve(true);
	cout << "Day 4 Part 2 answer is: " << validPasswords << endl;
}