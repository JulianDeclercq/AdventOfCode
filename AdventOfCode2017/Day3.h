#include <string>
#include <iostream>
#include <vector>
#include <map>
#include <tuple>

using namespace std;

struct Point
{
	Point() {}
	Point(int x, int y) : X(x), Y(y) {}
	int X = 0, Y = 0;
};

class Day3
{
private:

	// The puzzle input
	const int _puzzleInput = 361527;

	// A map containing all numbers and their respective positions
	map<int, Point> _numberPositions = map<int, Point>(); // part 1
	map<Point, int> _positionNumbers = map<Point, int>(); // part 2

	// Part 1
	int ManhattanDistance(Point p1, Point p2);
	void GenerateSpiralMemory();

	// Part 2
	int NeighbourValueSum(Point source);
	void GenerateSpiralMemory2();

public:
	void Part1();
	void Part2();
};