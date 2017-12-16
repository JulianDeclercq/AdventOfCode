#include "Day11.h"

string Day11::ParseInput()
{
	ifstream input("Input/Day11.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return "";
	}

	string line;
	getline(input, line);
	return line;
}

Point Day11::FollowPath(const string& input)
{
	// Create a vector of directions to follow
	vector<Point> directionsToFollow = vector<Point>();
	for (const string& direction : Helpers::Split(input, ','))
		directionsToFollow.push_back(_directions[direction]);

	// Follow that path
	Point origin = Point(0, 0);
	for (size_t i = 0; i < directionsToFollow.size(); ++i)
	{
		origin += directionsToFollow[i];

		// optimization: if only part 1 is run, don't do the calculations for part 2
		if (_part1)
			continue;

		// Part 2: calculate the furthest steps away
		vector<Point> route = vector<Point>();
		CalculateShortestRoute(route, origin, Point(0, 0));

		// Update the furthest steps away if this one was further
		if (route.size() > _furthestStepsAway)
			_furthestStepsAway = route.size();
	}

	return origin;
}

void Day11::CalculateShortestRoute(vector<Point>& route, const Point& destination, Point currentPosition)
{
	// Start step as the total needed difference (how much X and how much Y still has to be moved to reach the destination)
	Point step = destination - currentPosition;

	// Calculate which direction gets most result in 1 step
	step.X = (step.X == 0) ? 0 : destination.X / abs(destination.X);
	step.Y = (step.Y == 0) ? 0 : destination.Y / abs(destination.Y);

	// Add the step to the path
	route.push_back(step);

	// Update the current position
	currentPosition += step;

	// Check if the destination has been reached
	if (currentPosition.X == destination.X && currentPosition.Y == destination.Y)
		return;

	// Calculate the next step of the route if the destination hasn't been reached yet
	CalculateShortestRoute(route, destination, currentPosition);
}

void Day11::Part1()
{
	_part1 = true;

	// Follow the path to determine the destination
	Point destination = FollowPath(ParseInput());

	// Calculate the shortest route to the destination
	vector<Point> route = vector<Point>();
	CalculateShortestRoute(route, destination, Point(0, 0));
	cout << "Day 11 Part 1 answer: " << route.size() << endl;
}

void Day11::Part2()
{
	_part1 = false;

	// Follow the path
	Point destination = FollowPath(ParseInput());
	cout << "Day 11 Part 2 answer: " << _furthestStepsAway << endl;
}