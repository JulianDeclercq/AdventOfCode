#include "Day1.h"

void Day1::Part1()
{
	// Read the input
	const string fileName = "input/day1.txt";
	ifstream input = ifstream(fileName);

	if (input.fail())
	{
		cout << "Failed to open input file " << fileName << endl;
		return;
	}

	int answer = 0;
	string line = "";
	while (input >> line)
		answer += stoi(line);

	cout << "Answer to part 1 is " << answer << '.' << endl;
}

void Day1::Part2()
{
	// Read the input
	const string fileName = "input/day1.txt";
	ifstream input = ifstream(fileName);

	if (input.fail())
	{
		cout << "Failed to open input file " << fileName << endl;
		return;
	}

	int answer = 0;
	string line = "";
	vector<int> frequencies = { answer }, changes = vector<int>();
	changes.reserve(1030);

	// Optimization after first run that gave the answer.
	frequencies.reserve(141208);

	// Read through the input a first time.
	while (input >> line)
	{
		int change = stoi(line);
		changes.push_back(change);

		// Apply the change.
		answer += change;

		// If this frequency already exists, leave the method and output the answer.
		if (find(frequencies.begin(), frequencies.end(), answer) != frequencies.end())
		{
			cout << "Answer to part 2 is " << answer << '.' << endl;
			return;
		}

		// Add the new frequency to the vector of frequencies.
		frequencies.push_back(answer);
	}
	input.close();

	// If the answer hasn't been found yet, repeat applying changes until the answer is found
	for (;;)
	{
		for (const auto change : changes)
		{
			// Apply the change.
			answer += change;

			// If this frequency already exists, leave the method and output the answer.
			if (find(frequencies.begin(), frequencies.end(), answer) != frequencies.end())
			{
				cout << "Answer to part 2 is " << answer << '.' << endl;
				cout << "Debug:: frequencies size: " << frequencies.size() << endl;
				cout << "Debug:: changes size: " << changes.size() << endl;
				return;
			}

			// Add the new frequency to the vector of frequencies.
			frequencies.push_back(answer);
		}
	}
}