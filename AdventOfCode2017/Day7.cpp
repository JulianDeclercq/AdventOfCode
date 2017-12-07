#include "Day7.h"

void Day7::Part1()
{
	//ifstream input("Example/Day7Part1.txt");
	ifstream input("Input/Day7.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	map<string, Program*> tree = map<string, Program*>();
	// Vector to save the branches.
	// Pair: first = name, second = children's string
	vector < pair<string, string>> branches = vector < pair<string, string>>();

	string line;
	while (getline(input, line))
	{
		// Create a program from each line
		smatch match;
		if (regex_match(line, match, regex(R"((\w+) \((\d+)\)(?: -> (.*))?)")))
		{
			// Parse the regex
			string programName = string(match[1]);
			string childrenString = string(match[3]);

			// Parse the children later
			if (!childrenString.empty())
				branches.push_back(make_pair(programName, childrenString));

			// Insert this program in the tree
			tree.insert(make_pair(programName, new Program(programName, stoi(match[2]), {})));
		}
	}

	// Now that all leaves have been parsed, parse all the branches
	for (const pair<string, string>& branch : branches)
	{
		vector<Program*> children = vector<Program*>();

		// Parse the children
		for (const string& child : Helpers::Split(branch.second, ','))
			children.push_back(tree[child]);

		// Set the children
		tree[branch.first]->Children = children;
	}

	// Bottom program is the program having children but it is no ones child
	for (const auto& element : tree)
	{
		Program* program = element.second;

		// If the program has no children, it's impossible to be the root
		if (program->Children.empty())
			continue;

		// If the program is someones child, it's impossible to be the root
		bool isRoot = true;
		for (const auto& element2 : tree)
		{
			Program* program2 = element2.second;
			if (find(program2->Children.begin(), program2->Children.end(), program) != program2->Children.end())
			{
				isRoot = false;
				break;
			}
		}

		if (isRoot)
			cout << "Day 7 Part 1 Answer: " << program->Name << endl;
	}
}

void Day7::Part2()
{
}