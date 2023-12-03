#pragma once
#include <iostream>
#include <string>
#include <fstream>
#include <utility>
#include <vector>
#include <regex>
#include <tuple>
#include <algorithm>
#include <numeric>
#include <set>

using namespace std;

struct Gate
{
	Gate(pair<int, int> gate) : Ports(gate), PortAvailable({ true, true })
	{
	}

	bool operator==(const Gate& rhs) const
	{
		return (Ports.first == rhs.Ports.first) && (Ports.second == rhs.Ports.second);
	}

	bool operator!=(const Gate& rhs) const
	{
		return !(*this == rhs);
	}

	// Naive implementation for set to work
	bool operator<(const Gate& rhs) const
	{
		return tie(Ports.first, Ports.second) < tie(rhs.Ports.first, rhs.Ports.second);
	}

	pair<int, int> Ports;
	pair<bool, bool> PortAvailable;
};
using Bridge = vector<Gate>;

class Day24
{
private:
	int BridgeStrength(const Bridge& bridge);
	void FindConnection(const vector<Gate>& gates, const Bridge& bridge);

	void ParseInput(vector<Gate>& gates, vector<Gate>& startingGates);
	void CalculateBridges(vector<Gate>& gates, vector<Gate>& startingGates);

	set<Bridge> _bridges{};
public:
	void Part1();
	void Part2();
};
