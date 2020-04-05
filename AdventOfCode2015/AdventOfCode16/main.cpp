#include <iostream>
#include <fstream>
#include <string>
#include <regex>
#include <vector>

struct Aunt
{
	Aunt() {}; //default constructor
	Aunt(int children, int cats, int samoyeds, int pomeranians, int akitas, int vizslas, int goldfish, int trees, int cars, int perfumes) :
		m_Children(children), m_Cats(cats), m_Samoyeds(samoyeds), m_Pomeranians(pomeranians), m_Akitas(akitas),  m_Vizslas(vizslas), m_Goldfish(goldfish), m_Trees(trees), m_Cars(cars), m_Perfumes(perfumes)
	{
	}
	
	void SetValue(std::string valueName, int value)
	{
		if (valueName.compare("children") == 0)
		{
			m_Children = value;
		}
		else if (valueName.compare("cats") == 0)
		{
			m_Cats = value;
		}
		else if (valueName.compare("samoyeds") == 0)
		{
			m_Samoyeds = value;
		}
		else if (valueName.compare("pomeranians") == 0)
		{
			m_Pomeranians = value;
		}
		else if (valueName.compare("akitas") == 0)
		{
			m_Akitas = value;
		}
		else if (valueName.compare("vizslas") == 0)
		{
			m_Vizslas = value;
		}
		else if (valueName.compare("goldfish") == 0)
		{
			m_Goldfish = value;
		}
		else if (valueName.compare("trees") == 0)
		{
			m_Trees = value;
		}
		else if (valueName.compare("cars") == 0)
		{
			m_Cars = value;
		}
		else if (valueName.compare("perfumes") == 0)
		{
			m_Perfumes = value;
		}
		else
		{
			std::cout << "Tried to set an inexisting value. \n";
		}
	}

	//Part 1
	/*int Compare(const Aunt &other) //Part 1
	{
		int compareCtr = 0;
		if (m_Children == other.m_Children)
		{
			++compareCtr;
		}
		if (m_Cats == other.m_Cats)
		{
			++compareCtr;
		}
		if (m_Samoyeds == other.m_Samoyeds)
		{
			++compareCtr;
		}
		if (m_Pomeranians == other.m_Pomeranians)
		{
			++compareCtr;
		}
		if (m_Akitas == other.m_Akitas)
		{
			++compareCtr;
		}
		if (m_Vizslas == other.m_Vizslas)
		{
			++compareCtr;
		}
		if (m_Goldfish == other.m_Goldfish)
		{
			++compareCtr;
		}
		if (m_Trees == other.m_Trees)
		{
			++compareCtr;
		}
		if (m_Cars == other.m_Cars)
		{
			++compareCtr;
		}
		if (m_Perfumes == other.m_Perfumes)
		{
			++compareCtr;
		}
		return compareCtr;
	}*/

	//Part 2
	int Compare(const Aunt &other)
	{
		int compareCtr = 0;
		if (m_Children == other.m_Children)
		{
			++compareCtr;
		}
		if (m_Cats < other.m_Cats)
		{
			++compareCtr;
		}
		if (m_Samoyeds == other.m_Samoyeds)
		{
			++compareCtr;
		}
		if (m_Pomeranians > other.m_Pomeranians)
		{
			++compareCtr;
		}
		if (m_Akitas == other.m_Akitas)
		{
			++compareCtr;
		}
		if (m_Vizslas == other.m_Vizslas)
		{
			++compareCtr;
		}
		if (m_Goldfish > other.m_Goldfish)
		{
			++compareCtr;
		}
		if (m_Trees < other.m_Trees)
		{
			++compareCtr;
		}
		if (m_Cars == other.m_Cars)
		{
			++compareCtr;
		}
		if (m_Perfumes == other.m_Perfumes)
		{
			++compareCtr;
		}
		return compareCtr;
	}

	void Print()
	{
		std::cout << "Children: " << m_Children << "\n";
		std::cout << "Cats: " << m_Cats << "\n";
		std::cout << "Samoyeds: " << m_Samoyeds << "\n";
		std::cout << "Pomeranians: " << m_Pomeranians << "\n";
		std::cout << "Akitas: " << m_Akitas << "\n";
		std::cout << "Vizslas: " << m_Vizslas << "\n";
		std::cout << "Goldfish: " << m_Goldfish << "\n";
		std::cout << "Trees: " << m_Trees << "\n";
		std::cout << "Cars: " << m_Cars << "\n";
		std::cout << "Perfumes: " << m_Perfumes << "\n\n";
	}

	int m_Children = 0, m_Cats = 0, m_Samoyeds = 0, m_Pomeranians = 0, m_Akitas = 0, m_Vizslas = 0, m_Goldfish = 0, m_Trees = 0, m_Cars = 0, m_Perfumes = 0;
};

Aunt targetAunt = {3, 7, 2, 3, 0, 0, 5, 3, 2, 1};

void ReadFromFile(std::string path, std::vector<Aunt> &aunts);

int main()
{
	std::vector<Aunt> AuntList;
	AuntList.resize(501);

	ReadFromFile("./input.txt", AuntList);

	size_t matchingIdx = 0, highestScore = 0;
	for (size_t i = 0; i < AuntList.size(); ++i)
	{
		if (targetAunt.Compare(AuntList[i]) > highestScore)
		{
			highestScore = targetAunt.Compare(AuntList[i]);
			matchingIdx = i;
		}
	}

	std::cout << "Answer is: " << matchingIdx << std::endl;

	system("pause");
	return 0;
}

void ReadFromFile(std::string path, std::vector<Aunt> &aunts)
{
	std::ifstream input(path);
	if (input.fail())
	{
		std::cout << "Failed to open the input file. \n";
	}

	std::smatch _smatch;
	std::regex _regex("([[:w:]]+) ([[:digit:]]+): ([[:w:]]+): ([[:digit:]]+), ([[:w:]]+): ([[:digit:]]+), ([[:w:]]+): ([[:digit:]])");

	std::string extractedLine;
	int matchCtr = 0;
	while (!input.eof())
	{
		std::getline(input, extractedLine);
		bool search = std::regex_search(extractedLine, _smatch, _regex);
		if (search)
		{
			size_t index = std::stoi(_smatch[2]);
			aunts[index].SetValue(_smatch[3], std::stoi(_smatch[4]));
			aunts[index].SetValue(_smatch[5], std::stoi(_smatch[6]));
			aunts[index].SetValue(_smatch[7], std::stoi(_smatch[8]));
		}
	}
}