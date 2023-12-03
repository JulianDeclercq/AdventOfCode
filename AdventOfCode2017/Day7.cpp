#include "Day7.h"

void Day7::ParseTree()
{
	//ifstream input("Example/Day7Part1.txt");
	ifstream input("Input/Day7.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

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
			_tree.insert(make_pair(programName, new Program(programName, stoi(match[2]), {})));
		}
	}

	// Now that all leaves have been parsed, parse all the branches
	for (const pair<string, string>& branch : branches)
	{
		vector<Program*> children = vector<Program*>();

		// Parse the children
		for (const string& child : Helpers::Split(branch.second, ','))
			children.push_back(_tree[child]);

		// Set the children
		_tree[branch.first]->Children = children;
	}
}

Program* Day7::FindRoot()
{
	// Bottom program is the program having children but it is no ones child
	for (const auto& element : _tree)
	{
		Program* program = element.second;

		// If the program has no children, it's impossible to be the root
		if (program->Children.empty())
			continue;

		// If the program is someones child, it's impossible to be the root
		bool isRoot = true;
		for (const auto& element2 : _tree)
		{
			Program* program2 = element2.second;
			if (find(program2->Children.begin(), program2->Children.end(), program) != program2->Children.end())
			{
				isRoot = false;
				break;
			}
		}

		if (isRoot)
			return program;
	}

	return nullptr;
}

int Day7::FindWeightCorrection(Program* wrongWeightProgram)
{
	// Clear the tower weights
	vector<int> towerWeights = vector<int>();

	// All the children's sums must match
	for (Program* child : wrongWeightProgram->Children)
		towerWeights.push_back(child->CalculateTowerWeight());

	// Count the occurences
	map<int, int> occurences = map<int, int>();
	for_each(towerWeights.begin(), towerWeights.end(), [&occurences](int weight) {occurences[weight]++; });

	for (auto it = occurences.begin(); it != occurences.end(); ++it)
	{
		// Find the odd one
		if (it->second != 1)
			continue;

		// Calculate the weight difference that is needed to balance
		int wrongWeight = it->first;

		// Check the first element except if the wrong one is the first element, then check the second
		int rightWeight = (it == occurences.begin()) ? (++occurences.begin())->first : occurences.begin()->first;

		// Calculate the difference (doesn't matter if positive or negative, it's going to be added anyways)
		int difference = rightWeight - wrongWeight;

		// Find the child with the wrong weight and return the corrected weight
		for (const Program* child : wrongWeightProgram->Children)
		{
			if (child->TowerWeight == wrongWeight)
				return child->Weight + difference;
		}
	}

	return 0;
}

void Day7::Part1()
{
	ParseTree();
	cout << "Day 7 Part 1 Answer: " << FindRoot()->Name << endl;
}

void Day7::Part2()
{
	ParseTree();

	// Traverse the tree
	Program* program = nullptr;
	for (const auto& element : _tree)
	{
		// Loop until the unbalanced program is found
		program = element.second;
		if (!program->IsBalanced())
			break;
	}

	// Once the unbalanced program is found, calculate the weight correction for it
	cout << "Day 7 Part 2 Answer: " << FindWeightCorrection(program) << endl;
}