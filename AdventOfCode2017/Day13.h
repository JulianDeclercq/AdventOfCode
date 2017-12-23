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
	Layer(int depth, int range) : Depth(depth), Range(range), HasScanner(range > 0), ScannerIdx(0)
	{
	}

	void MoveScanner()
	{
		// There is no scanner of the Range is 0
		if (!HasScanner)
			return;

		// Progress the scanner
		(scannerForward) ? ++ScannerIdx : --ScannerIdx;

		// Check if a boundary has been reached and turn around if it has
		if (ScannerIdx == 0 || ScannerIdx == (Range - 1))
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
	bool HasScanner;

private:
	bool scannerForward = true;
};

class Day13
{
private:
	void ParseInput(vector<Layer>& fireWall);
public:
	void Part1();
	void Part2();
};
