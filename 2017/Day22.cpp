#include "Day22.h"

void Day22::ParseInput(map<Point, bool>* clusterP1, map<Point, State>* clusterP2)
{
	//ifstream input("Example/Day22Part1.txt");
	ifstream input("Input/Day22.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	// Read all lines into a vector
	string str;
	vector<string> lines{};
	while (getline(input, str))
		lines.push_back(str);

	// Set the width and height of the input grid
	_gridWidth = lines.front().size();
	_gridHeight = lines.size();

	// Parse the input into the cluster
	for (size_t i = 0; i < lines.size(); ++i)
	{
		// Add the nodes to the "grid". Offset them from the middle so starting from 0 is easy
		for (size_t j = 0; j < lines[i].size(); ++j)
		{
			int x = -(_gridWidth / 2) + j;
			int y = (_gridHeight / 2) - i;

			if (clusterP1 != nullptr)
			{
				bool infected = (lines[i][j] == '#');
				clusterP1->insert({ Point(x, y), infected });
			}
			else if (clusterP2 != nullptr)
			{
				State infected = (lines[i][j] == '#') ? Infected : Clean;
				clusterP2->insert({ Point(x, y), infected });
			}
		}
	}
}

void Day22::TurnRight()
{
	_facing = static_cast<Facing>((_facing + 1) % 4);
}

void Day22::TurnLeft()
{
	_facing = (_facing == 0) ? West : static_cast<Facing>(_facing - 1);
}

void Day22::Inverse()
{
	switch (_facing)
	{
	case Day22::North: _facing = South; break;
	case Day22::East: _facing = West; break;
	case Day22::South: _facing = North; break;
	case Day22::West: _facing = East; break;
	}
}

void Day22::Part1()
{
	// Initialize the cluster
	map<Point, bool> cluster{};

	// Initialize the virus carrier
	Point virusCarrier = Point(0, 0);

	// Parse the input
	ParseInput(&cluster, nullptr);

	// Count how many bursts infect
	int infectionCtr = 0;
	for (int i = 0; i < 10000; ++i)
	{
		// Turn left or right accordingly
		(cluster[virusCarrier]) ? TurnRight() : TurnLeft();

		// Infect or cure
		cluster[virusCarrier] = !cluster[virusCarrier];

		// If the node is now infected, increment the counter
		if (cluster[virusCarrier])
			++infectionCtr;

		// The virusCarrier moves forward one node in the direction it is facing
		virusCarrier += _forwardMovement.at(_facing);
	}

	cout << "Day 22 Part 1 answer: " << infectionCtr << endl;
}

void Day22::Part2()
{
	// Initialize the cluster
	map<Point, State> cluster{};

	// Parse the input
	ParseInput(nullptr, &cluster);

	// Initialize the virus carrier
	Point virusCarrier = Point(0, 0);

	// Count how many bursts infect
	int infectionCtr = 0;
	for (int i = 0; i < 10000000; ++i)
	{
		State& nodeState = cluster[virusCarrier];

		// Turn left or right accordingly
		switch (nodeState)
		{
		case Clean:TurnLeft(); break;
		case Weakened:break;
		case Infected: TurnRight(); break;
		case Flagged: Inverse();  break;
		}

		// Change the state of the node
		nodeState = (nodeState == Flagged) ? Clean : static_cast<State>(nodeState + 1);

		// If the node is now infected, increment the counter
		if (cluster[virusCarrier] == Infected)
			++infectionCtr;

		// The virusCarrier moves forward one node in the direction it is facing
		virusCarrier += _forwardMovement.at(_facing);
	}

	cout << "Day 22 Part 2 answer: " << infectionCtr << endl;
}