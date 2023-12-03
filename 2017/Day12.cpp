#include "Day12.h"

void Day12::ParseInput()
{
	//ifstream input("Example/Day12Part1.txt");
	ifstream input("Input/Day12.txt");
	if (input.fail())
	{
		cout << "Failed to open inputstream." << endl;
		return;
	}

	string line;
	regex expression(R"((\d+) <-> (.+))");
	smatch match;
	while (getline(input, line))
	{
		if (!regex_match(line, match, expression))
			return;

		// Parse the connections
		for (const string& connection : Helpers::Split(string(match[2]), ','))
		{
			int lhs = stoi(match[1]);
			int rhs = stoi(connection);

			// Make the connection
			_pipeNetwork[lhs].insert(rhs);

			// Connections are double ended
			_pipeNetwork[rhs].insert(lhs);
		}
	}
}

void Day12::ExploreConnections(set<int>& connections, int pipeID)
{
	// Insert the ID itself
	connections.insert(pipeID);

	// Add all directConnections and explore their connections (= indirect connections)
	for (int directConnection : _pipeNetwork[pipeID])
	{
		// Add the direct connection
		connections.insert(directConnection);

		// Explore the indirect connections
		for (int indirectConnection : _pipeNetwork[directConnection])
		{
			// If the indirect connection has already been explored, skip it
			auto it = connections.find(indirectConnection);
			if (it != connections.end())
				continue;

			// Explore the indirect connection
			ExploreConnections(connections, indirectConnection);
		}
	}
}

void Day12::Part1()
{
	// Parse the input, creates a map with all the direct connections as can be read from the inputfile
	ParseInput();

	// Explore the connections of 0
	set<int> connections = set<int>();
	ExploreConnections(connections, 0);

	cout << "Day 12 Part 1 answer: " << connections.size() << endl;
}

void Day12::Part2()
{
	// Parse the input, creates a map with all the direct connections as can be read from the inputfile
	ParseInput();

	// Loop through the network to form all the groups
	for (size_t i = 0; i < _pipeNetwork.size(); ++i)
	{
		// Check if the group that contains this pipe has been formed before
		bool groupFormed = false;
		for (set<int> group : _groups)
		{
			// If the group contains this element, its group has been formed before
			if (group.find(i) != group.end())
			{
				groupFormed = true;
				break;
			}
		}

		// Skip this group if it has been formed before
		if (groupFormed)
			continue;

		// Explore the connections of this pipe/form its group
		set<int> connections = set<int>();
		ExploreConnections(connections, i);

		// Add the formed group to the list of groups
		_groups.push_back(connections);
	}

	cout << "Day 12 Part 2 answer: " << _groups.size() << endl;
}