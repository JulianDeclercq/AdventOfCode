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
	vector<Point> directionsToFollow = vector<Point>();
	vector<string> parsed = Helpers::Split(input, ',');
	for (size_t i = 0; i < parsed.size(); ++i)
		directionsToFollow.push_back(_directions[parsed[i]]);

	Point origin = Point(0, 0);
	for (size_t i = 0; i < directionsToFollow.size(); ++i)
		origin += directionsToFollow[i];

	return origin;
}

void Day11::CalculateNextStep(vector<Point>& route, const Point& destination, Point currentPosition)
{
	// Start setp as the total needed difference (how much X and how much Y still has to be moved to reach the destination)
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

	// Calculate the next step if the destination hasn't been reached yet
	CalculateNextStep(route, destination, currentPosition);
}

void Day11::Part1()
{
	// Follow the path to the destination
	const string input = ParseInput();
	Point destination = FollowPath(input);

	vector<Point> route = vector<Point>();
	CalculateNextStep(route, destination, Point(0, 0));
	cout << "Day 11 Part 1 answer: " << route.size() << endl;
}