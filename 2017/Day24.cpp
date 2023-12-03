#include "Day24.h"
int Day24::BridgeStrength(const Bridge& bridge)
{
	int bridgeStrength = 0;

	for (const Gate& gate : bridge)
		bridgeStrength += gate.Ports.first + gate.Ports.second;

	return bridgeStrength;
}

void Day24::FindConnection(const vector<Gate>& availableGates, const Bridge& bridge)
{
	// Find a connection
	for (auto it = availableGates.begin(); it != availableGates.end(); ++it)
	{
		Gate gate = *it;
		bool canConnect = false;
		bool firstPortConnected = false;
		Gate connectTo = bridge.back();
		// If the first port has been used already, use the second and vice-versa
		int portToConnect = (connectTo.PortAvailable.first) ? connectTo.Ports.first : connectTo.Ports.second;

		// Check for a connection with the first port
		if (gate.PortAvailable.first && gate.Ports.first == portToConnect)
		{
			canConnect = true;
			firstPortConnected = true;
		}
		else if (gate.PortAvailable.second && gate.Ports.second == portToConnect)
		{
			canConnect = true;
			firstPortConnected = false;
		}

		// If no connections can be made, look for the next gate
		if (!canConnect)
			continue;

		// Mark the used ports as used
		(portToConnect == connectTo.Ports.first ? connectTo.PortAvailable.first : connectTo.PortAvailable.second) = false;
		(firstPortConnected ? gate.PortAvailable.first : gate.PortAvailable.second) = false;

		// Find the next connection
		vector<Gate> newAvailableGates{};
		copy_if(availableGates.begin(), availableGates.end(), back_inserter(newAvailableGates), [gate](const Gate& availableGate)
		{
			return availableGate != gate;
		});

		// Add the new connection to the bridge, make a copy to avoid messing with the local bridge
		Bridge newBridge = bridge;
		newBridge.push_back(gate);

		// Insert the bridge with the new connection as a new bridge
		// Even though this one will never have a higher score, do it for consistency
		_bridges.insert(newBridge);

		// Find the next connection
		FindConnection(newAvailableGates, newBridge);
	}

	_bridges.insert(bridge);
}

void Day24::ParseInput(vector<Gate>& gates, vector<Gate>& startingGates)
{
	//ifstream input("Example/Day24Part1.txt");
	ifstream input("Input/Day24.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	string line;
	smatch match;
	while (getline(input, line))
	{
		if (!regex_match(line, match, regex(R"((\d+)\/(\d+))")))
			continue;

		// Add the gate to the corresponding vector
		Gate gate = Gate({ stoi(match[1]), stoi(match[2]) });
		(stoi(match[1]) == 0) ? startingGates.push_back(gate) : gates.push_back(gate);
	}
}

void Day24::CalculateBridges(vector<Gate>& gates, vector<Gate>& startingGates)
{
	// Find all bridges
	for (Gate startingGate : startingGates)
	{
		// Disable the starting '0' port
		startingGate.PortAvailable.first = false;

		// One port counts as a bridge itself too
		_bridges.insert({ startingGate });

		// Add the startingGate itself
		Bridge bridge{};
		bridge.push_back(startingGate);

		// Find connections
		FindConnection(gates, bridge);
	}
}

void Day24::Part1()
{
	vector<Gate> gates{};
	vector<Gate> startingGates{};

	// Parse the input
	ParseInput(gates, startingGates);

	// Find all bridges
	CalculateBridges(gates, startingGates);

	// Find the bridge with the highest strength
	const auto highestStrengthBridge = max_element(_bridges.begin(), _bridges.end(), [this](const Bridge& lhs, const Bridge& rhs)
	{
		return BridgeStrength(lhs) < BridgeStrength(rhs);
	});

	cout << "Day 24 Part 1 answer: " << BridgeStrength(*highestStrengthBridge) << endl;
}

void Day24::Part2()
{
	vector<Gate> gates{};
	vector<Gate> startingGates{};

	// Parse the input
	ParseInput(gates, startingGates);

	// Find all bridges
	CalculateBridges(gates, startingGates);

	// Find the longest bridge
	const auto longestBridgeElement = max_element(_bridges.begin(), _bridges.end(), [](const Bridge& lhs, const Bridge& rhs)
	{
		return lhs.size() < rhs.size();
	});

	// Extract all longest bridges
	vector<Bridge> longestBridges{};
	int longestBridgeSize = (*longestBridgeElement).size();
	copy_if(_bridges.begin(), _bridges.end(), back_inserter(longestBridges), [longestBridgeSize](const Bridge& bridge)
	{
		return bridge.size() == longestBridgeSize;
	});

	// Find the (longest) bridge with the highest strength
	const auto highestStrengthBridge = max_element(longestBridges.begin(), longestBridges.end(), [this](const Bridge& lhs, const Bridge& rhs)
	{
		return BridgeStrength(lhs) < BridgeStrength(rhs);
	});

	cout << "Day 24 Part 2 answer: " << BridgeStrength(*highestStrengthBridge) << endl;
}