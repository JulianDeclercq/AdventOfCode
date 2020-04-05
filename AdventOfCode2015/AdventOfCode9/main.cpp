#include <vector>
#include <iostream>
#include <fstream>
#include <string>
#include <cmath>
#include <algorithm>

struct Link
{
	Link(){}
	Link(std::string first, std::string second, int dist) : firstCity(first), secondCity(second), distance(dist)
	{

	}

	std::string firstCity;
	std::string secondCity;
	int distance;
};

//Overloading the stream operator for easier debugging
std::ostream &operator << (std::ostream &stream, const Link &link)
{
	stream << "Link between: " << link.firstCity << " and " << link.secondCity << std::endl;
	return stream;
}

bool operator==(const Link &a, const Link &b)
{
	if ((a.firstCity.compare(b.firstCity) == 0) && (a.secondCity.compare(b.secondCity) == 0))
	{
		return true;
	}
	if ((a.firstCity.compare(b.secondCity) == 0) && (a.secondCity.compare(b.firstCity) == 0))
	{
		return true;
	}
	return false;
}

void ReadFromFile(std::string path, std::vector<std::string> &vec);
void InterpretInformation(std::string &infoStr);
void CalculatePossibleRoutes(int startingIdx);
int CalculateRouteDistance(const std::vector<Link> &route);
Link SwapLink(const Link& lnk);

std::vector<std::string> m_Cities;
std::vector<std::string> m_CitiesVisited;
std::vector<Link> m_Links;
std::vector<int> m_Distances;

int main()
{
	//input & output
	std::vector<std::string> inputLines;
	ReadFromFile("./input.txt", inputLines);

	for (std::string str : inputLines)
	{
		InterpretInformation(str);
	}

	for (size_t i = 0; i < m_Links.size(); ++i)
	{
		CalculatePossibleRoutes(i);
	}

	//CalculateRouteDistance test. WORKS
	/*Link a = Link("Dublin", "Belfast", 141);
	Link b = Link("Belfast", "London", 518);
	std::vector<Link> customRoute = { a, b };
	std::cout << "Total route distance is: " << CalculateRouteDistance(customRoute) << std::endl;*/

	std::sort(m_Distances.begin(), m_Distances.end());
	for (int i : m_Distances)
	{
		std::cout << i << std::endl;
	}
	std::cout << "Answer 1: " << m_Distances[1] << std::endl;
	std::cout << "Answer 2: " << m_Distances[m_Distances.size() - 1] << std::endl;

	system("pause");
	return 0;
}

void CalculatePossibleRoutes(int startingIdx)
{
	std::vector<Link> route;
	Link startLink = m_Links[startingIdx];
	
	route.push_back(startLink); //starting value
	std::cout << "Starting value is: " << startLink;
	m_CitiesVisited.push_back(startLink.firstCity);

	std::string firstValue, secondValue, swappedFirst;

	for (size_t i = 0; i < m_Links.size(); ++i)
	{
		firstValue = m_Links[i].firstCity;
		secondValue = route[route.size() - 1].secondCity;
		swappedFirst = SwapLink(m_Links[i]).firstCity;
		//std::cout << "Comparing /" << firstValue << "/ to: /" << secondValue << "/" << std::endl;
		if (firstValue.compare(secondValue) == 0)
		{
			if (std::find(route.begin(), route.end(), m_Links[i]) == route.end())
			{
				route.push_back(m_Links[i]);
				m_CitiesVisited.push_back(secondValue);
				//std::cout << "Link added (No swap)." << std::endl;
			}
		}
		else if (swappedFirst.compare(secondValue) == 0)
		{
			if (std::find(route.begin(), route.end(), SwapLink(m_Links[i])) == route.end())
			{
				route.push_back(SwapLink(m_Links[i]));
				m_CitiesVisited.push_back(secondValue);
				//std::cout << "Link added (Swap)." << std::endl;
			}
		}
	}
	//std::cout << "route.size() = " << route.size() << std::endl;
	std::cout << "The routes are: " << std::endl;
	for (Link lnk : route)
	{
		std::cout << lnk;
	}
	std::cout << CalculateRouteDistance(route) << std::endl;
	m_Distances.push_back(CalculateRouteDistance(route));
	//std::cout << route.size() << std::endl;
}

int CalculateRouteDistance(const std::vector<Link> &route)
{
	int totDist = 0;
	for (Link lnk : route)
	{
		totDist += lnk.distance;
	}
	return totDist;
}

Link SwapLink(const Link& lnk)
{
	Link swapped;
	swapped.firstCity = lnk.secondCity;
	swapped.secondCity = lnk.firstCity;
	swapped.distance = lnk.distance;
	return swapped;
}

void ReadFromFile(std::string path, std::vector<std::string> &vec)
{
	std::ifstream inputFile;
	inputFile.open(path);
	if (inputFile.fail())
	{
		std::cout << "Failed to open inputfile." << std::endl;
	}

	while (!inputFile.eof())
	{
		std::string extractedLine = "";
		std::getline(inputFile, extractedLine);
		vec.push_back(extractedLine);
	}

	inputFile.close();
}

void InterpretInformation(std::string &infoStr)
{
	size_t firstSpace = infoStr.find(' ');
	size_t secondSpace = infoStr.substr(firstSpace + 1).find(' ') + firstSpace + 1;
	size_t thirdSpace = infoStr.substr(secondSpace + 1).find(' ') + secondSpace + 1;
	size_t lastSpace = infoStr.rfind(' ');

	std::string firstWord = infoStr.substr(0, firstSpace);
	std::string secondWord = infoStr.substr(firstSpace + 1, secondSpace - firstSpace - 1);
	std::string thirdWord = infoStr.substr(secondSpace + 1, thirdSpace - secondSpace - 1);
	int lastWord = std::stoi(infoStr.substr(lastSpace + 1));

	std::vector<std::string> alpha = { firstWord, thirdWord };
	std::sort(alpha.begin(), alpha.end());

	Link lnk = Link(alpha[0], alpha[1], lastWord);
	if (std::find(m_Links.begin(), m_Links.end(), lnk) == m_Links.end())
	{
		m_Links.push_back(lnk);
	}

	//Pushing back the different cities
	if (std::find(m_Cities.begin(), m_Cities.end(), firstWord) == m_Cities.end())
	{
		m_Cities.push_back(firstWord);
	}
	if (std::find(m_Cities.begin(), m_Cities.end(), thirdWord) == m_Cities.end())
	{
		m_Cities.push_back(thirdWord);
	}
}
