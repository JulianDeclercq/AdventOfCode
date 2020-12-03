#pragma once

#include <iostream>
#include <regex>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;

class Day3
{
private:
	struct Vector2
	{
		Vector2(int x, int y) : X(x), Y(y) {}
		int X = 0;
		int Y = 0;

		string ToString()
		{
			return to_string(X) + ", " + to_string(Y);
		}

		Vector2& operator+=(const Vector2& rhs)
		{ 
			X += rhs.X;
			Y += rhs.Y;
			return *this; // return the result by reference
		}
	};

	bool _inputParsed = false;
	string _treeChart = "";
	int _width = 0, _height = 0;

	void ParseInput();
	char AtCoord(Vector2 p);
	bool IsTree(Vector2 p);

public:
	Day3(){}
	int Part1();

};
