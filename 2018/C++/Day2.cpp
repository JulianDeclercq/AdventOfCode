#include "Day2.h"

void Day2::Part1()
{
	// Read the input
	const string fileName = "input/day2.txt";
	ifstream input = ifstream(fileName);

	if (input.fail())
	{
		cout << "Failed to open input file " << fileName << endl;
		return;
	}

	int firstMultiplier = 0;
	int secondMultiplier = 0;
	string line = "";
	map<char, int> occurrences{};

	// Fill the map
	for (char c = 'a'; c <= 'z'; ++c)
		occurrences.insert({ c, 0 });

	// Clean map
	map<char, int> currentOccurences{};
	currentOccurences.insert(occurrences.begin(), occurrences.end());

	while (input >> line)
	{
		// Count all characters in the line
		for (const char c : line)
			++currentOccurences[c];

		// Count the amount of checks passed
		bool twoLetterCheck = false, threeLetterCheck = false;
		for (const auto& occurence : currentOccurences)
		{
			if (occurence.second == 2)
				twoLetterCheck = true;

			if (occurence.second == 3)
				threeLetterCheck = true;

			// If all checks have been passed already, there is no point in further exploring the string
			if (twoLetterCheck && threeLetterCheck)
				break;
		}

		// Increment the counters if the checks passed
		if (twoLetterCheck)
			++firstMultiplier;

		if (threeLetterCheck)
			++secondMultiplier;

		// Reset the occurrences
		currentOccurences.clear();
		currentOccurences.insert(occurrences.begin(), occurrences.end());
	}

	cout << "DAY2: Answer to part 1 is " << firstMultiplier * secondMultiplier << '.' << endl;
}