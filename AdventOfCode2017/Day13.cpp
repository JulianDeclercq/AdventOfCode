#include "Day13.h"

void Day13::ParseInput(vector<Layer>& fireWall)
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
	vector<Layer> layers{};
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
}

void Day13::Part1()
{
	// Parse the input
	vector<Layer> fireWall{};
	ParseInput(fireWall);

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

void Day13::Part2()
{
	// Parse the input
	vector<Layer> fireWall{};
	ParseInput(fireWall);

	// Infinite loop
	for (int delay = 0; ; ++delay)
	{
		bool foundSolution = true;

		// Traverse the firewall
		for (Layer layer : fireWall)
		{
			// steps*2 - 2 is when the scanner is on 0
			int stepsNeeded = delay + layer.Depth;
			int scannerZero = (layer.Range * 2 - 2);
			bool detected = stepsNeeded % scannerZero == 0;

			// If the layer is a scanning layer and it detects the package, invalidate this solution
			if (layer.HasScanner && detected)
				foundSolution = false;
		}

		if (foundSolution)
		{
			cout << "Day 13 Part 2 answer: " << delay << endl;
			return;
		}
	}
}