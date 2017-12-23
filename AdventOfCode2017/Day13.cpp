#include "Day13.h"

void Day13::Part1()
{
	//ifstream input("Example/Day13Part1.txt");
	ifstream input("Input/Day13.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	string line;
	smatch match;
	vector<Layer> fireWall{}, layers{};
	while (getline(input, line))
	{
		if (!regex_match(line, match, regex(R"((\d+): (\d+))")))
		{
			cout << "Regex didn't match: " << line << endl;
			return;
		}

		// Add to the layers
		// Don't add to the firewall yet as layers with no range will be filled in
		layers.push_back(Layer(stoi(match[1]), stoi(match[2])));
	}

	// Sort the layers on range
	sort(layers.begin(), layers.end());

	// Fill in the fireWall
	size_t insertedIdx = 0;

	// Loop for as many layers as should be in the firewall
	for (int i = 0; i <= layers.back().Depth; ++i)
	{
		// If the layer with this range exists, add it to the firewall
		if (i == layers[insertedIdx].Depth /*== rangeInserted*/)
		{
			fireWall.push_back(layers[insertedIdx]);
			++insertedIdx;
		}
		else // Insert an empty layer into the firewall
		{
			fireWall.push_back(Layer(i, 0));
		}
	}

	// Travel with the packet through all layers of the firewall
	int packet = -1;
	int tripSeverity = 0;
	for (size_t i = 0; i < fireWall.size(); ++i)
	{
		// Move the packet one layer forward
		++packet;

		// Check if the firewall catches the packet
		if (fireWall[i].ScannerIdx == 0)
			tripSeverity += fireWall[i].Depth * fireWall[i].Range;

		// Move the scanners one step
		for (Layer& layer : fireWall)
			layer.MoveScanner();
	}

	cout << "Day 13 Part 1 answer: " << tripSeverity << endl;
}