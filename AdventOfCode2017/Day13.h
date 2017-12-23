#pragma once
#include <iostream>
#include <fstream>
#include <string>
#include <regex>
#include <vector>
#include <algorithm>

using namespace std;

struct Layer
{
	Layer() : Depth(0), ScannerIdx(0)
	{
	}

	Layer(int depth, int range) : Depth(depth), Range(range), ScannerIdx(0)
	{
	}

	void MoveScanner()
	{
		if (Depth == 6)
			int brkpt = 5;

		// Progress the scanner
		(scannerForward) ? ++ScannerIdx : --ScannerIdx;

		// Check if a boundary has been reached and turn around if it has
		if (ScannerIdx == 0 || ScannerIdx == Range - 1)
			scannerForward = !scannerForward;
	}

	// Depth sort
	bool operator<(const Layer& rhs) const
	{
		return Depth < rhs.Depth;
	}

	int Depth;
	int Range;
	int ScannerIdx;

private:
	bool scannerForward = true;
};

class Day13
{
public:
	void Part1();
};
