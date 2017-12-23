#include "Day22.h"

void Day22::TurnRight()
{
	_facing = static_cast<Facing>((_facing + 1) % 4);
}

void Day22::TurnLeft()
{
	_facing = (_facing == 0) ? West : static_cast<Facing>(_facing - 1);
}

void Day22::Part1()
{
	//ifstream input("Example/Day22Part1.txt");
	ifstream input("Input/Day22.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	vector<string> lines{};

	// Read all lines into a vector
	string str;
	while (getline(input, str))
		lines.push_back(str);

	// Initialize the cluster
	map<Point, bool> cluster{};

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
			bool infected = (lines[i][j] == '#');
			cluster.insert({ Point(x, y), infected });
		}
	}

	// Initialize the virus carrier
	Point virusCarrier = Point(0, 0);

	// Define the forward movement for each facing direction
	map <Facing, Point> forwardMovement{ {North, Point(0, 1)},  {East, Point(1, 0)}, {South, Point(0, -1)}, {West, Point(-1, 0)} };

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
		virusCarrier += forwardMovement[_facing];
	}

	cout << "Day 22 Part 1 answer: " << infectionCtr << endl;
}