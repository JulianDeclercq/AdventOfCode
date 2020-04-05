#include <iostream>
#include <vector>
#include <string>
#include <fstream>

struct Cell
{
	Cell() {}; //default constructor
	Cell(bool aliveNow)
	{
		m_CurrAlive = aliveNow;
	}

	void Update()
	{
		m_CurrAlive = m_NextAlive;
	}

	bool m_CurrAlive = false;
	bool m_NextAlive = false;
};

enum class GridBorderType
{
	ALWAYSALIVE,
	ALWAYSDEAD,
	AOC18
};

enum class CheckBorder
{
	LEFT,
	RIGHT,
	TOP,
	BOTTOM,
	ANY
};

GridBorderType m_BorderType = GridBorderType::ALWAYSALIVE;
int m_Width = 0, m_Height = 0, m_NrOfGens = 0;
int m_PxCellSize = 0;

void ReadFromFile(std::vector<Cell> &cellVec);
void SetBorderType(GridBorderType borderType);
int  NeighbourAliveCount(std::vector<Cell> &cellVecRef, size_t idx);
void AliveNextGeneration(std::vector<Cell> &cellVecRef, size_t idx);
bool IsOnBorder(size_t cellIndex, CheckBorder borderCheck);
void CalcNextGen(std::vector<Cell> &cellVec, int genNr); //genNr is only for information printing
void PrintGen(const std::vector<Cell> &cellVec, int outputNr);

int main()
{
	m_NrOfGens = 100;

	std::vector<Cell> cellVec;
	cellVec.reserve(m_Width * m_Height);

	SetBorderType(GridBorderType::AOC18); //stated different in the assignment
	ReadFromFile(cellVec);
	PrintGen(cellVec, 0);

	for (int generations = 0; generations < m_NrOfGens; ++generations)
	{
		CalcNextGen(cellVec, generations);
		PrintGen(cellVec, generations + 1);
	}

	system("pause");
	return 0;
}

void ReadFromFile(std::vector<Cell> &cellVec)
{
	std::ifstream input("./input.txt");
	if (input.fail())
	{
		std::cout << "Failed to open the file." << std::endl;
	}

	//set position in buffer at end
	input.seekg(0, input.end);
	size_t lengthOfFile = (size_t)input.tellg();
	input.seekg(0, input.beg);

	std::vector<char> movementList(lengthOfFile);
	input.read(movementList.data(), lengthOfFile);

	cellVec.clear();

	size_t lineCtr = 1, charCtr = 0; //getting the dimensions of the grid
	for (char c : movementList)
	{
		if (c == '#')
		{
			cellVec.push_back(Cell(true));
		}
		else if (c == '.')
		{
			cellVec.push_back(Cell(false));
		}
		else if (c == '\0')
		{
			++lineCtr;
			--charCtr;
		}
		++charCtr;
	}

	m_Height = lineCtr;
	m_Width = charCtr / lineCtr;

	//Part 2
	cellVec[0].m_CurrAlive = true;
	cellVec[m_Width - 1].m_CurrAlive = true;
	cellVec[(m_Width * (m_Height - 1))].m_CurrAlive = true;
	cellVec[(m_Width * m_Height) - 1].m_CurrAlive = true;
}

void SetBorderType(GridBorderType borderType)
{
	switch (borderType)
	{
	case GridBorderType::ALWAYSALIVE:
		m_BorderType = GridBorderType::ALWAYSALIVE;
		std::cout << "BorderType set to always alive." << std::endl;
		break;
	case GridBorderType::ALWAYSDEAD:
		m_BorderType = GridBorderType::ALWAYSDEAD;
		std::cout << "BorderType set to always dead." << std::endl;
		break;
	case GridBorderType::AOC18:
		m_BorderType = GridBorderType::AOC18;
		std::cout << "BorderType set to AOC18." << std::endl;
		break;
	default:
		std::cout << "Hitting default in switch in SetBorderType." << std::endl;
		break;
	}
}

bool IsOnBorder(size_t cellIndex, CheckBorder borderCheck)
{
	const bool rightBorder = cellIndex % m_Width == m_Width - 1;
	const bool leftBorder = cellIndex % m_Width == 0;
	const bool topBorder = (int)cellIndex < m_Width;
	const bool bottomBorder = (int)cellIndex > m_Width * (m_Height - 1);

	switch (borderCheck) //needed for toroidal
	{
	case CheckBorder::LEFT:
		return leftBorder;
		break;

	case CheckBorder::RIGHT:
		return rightBorder;
		break;

	case CheckBorder::TOP:
		return topBorder;
		break;

	case CheckBorder::BOTTOM:
		return bottomBorder;
		break;

	case CheckBorder::ANY:
		if (rightBorder || leftBorder || topBorder || bottomBorder)
		{
			return true;
		}
		else
		{
			return false;
		}
		break;
	}

	std::cout << "Something went wrong in IsOnBorder. Returning false." << std::endl;
	return false;
}

void AliveNextGeneration(std::vector<Cell> &cellVecRef, size_t idx)
{
	const int neighboursAlive = NeighbourAliveCount(cellVecRef, idx);
	bool _bAOC18 = false;

	//Info
	/*if (cellVecRef[idx].m_CurrAlive)
	{
		std::cout << "Neighbours alive count is: " << neighboursAlive << std::endl;
		console << "Alive, neighbours alive count is: " << neighboursAlive << std::endl;
	}
	else
	{
		std::cout << "Neighbours alive count is: " << neighboursAlive << std::endl;
		console << "Dead, neighbours alive count is: " << neighboursAlive << std::endl;

	}*/

	if (idx == 0 || idx == m_Width - 1 || idx == (m_Width * (m_Height - 1)) || idx == (m_Width * m_Height) - 1) // Part 2 AoC
	{
		cellVecRef[idx].m_NextAlive = true;
		return;
	}

	if (IsOnBorder(idx, CheckBorder::ANY))
	{
		switch (m_BorderType)
		{
		case GridBorderType::ALWAYSALIVE:
			cellVecRef[idx].m_NextAlive = true;
			break;
		case GridBorderType::ALWAYSDEAD:
			cellVecRef[idx].m_NextAlive = false;
			break;
		case GridBorderType::AOC18: //I do it this way so my program can still do what the school assignment was. (I did delete the Toroidal part though because that was really long.)
			_bAOC18 = true;
			break;
		}
	}
	if (!IsOnBorder(idx, CheckBorder::ANY) || _bAOC18)
	{
		if (cellVecRef[idx].m_CurrAlive)
		{
			if (neighboursAlive == 2 || neighboursAlive == 3)
			{
				cellVecRef[idx].m_NextAlive = true;
			}
			else
			{
				cellVecRef[idx].m_NextAlive = false;
			}
		}
		else //if currently dead
		{
			if (neighboursAlive == 3)
			{
				cellVecRef[idx].m_NextAlive = true;
			}
			else
			{
				cellVecRef[idx].m_NextAlive = false;
			}
		}
	}
}

int NeighbourAliveCount(std::vector<Cell> &cellVecRef, size_t idx)
{
	int aliveCtr = 0;

	const bool hasLeftRow = idx % m_Width != 0;
	const bool hasUpperRow = (int)idx > (m_Width - 1);
	const bool hasRightRow = (int)idx % m_Width != (m_Width - 1);
	const bool hasBottomRow = (int)idx < m_Width * (m_Height - 1);

	//checking all of the valid neighbours.
	if (hasLeftRow)
	{
		if (cellVecRef[idx - 1].m_CurrAlive)
		{
			++aliveCtr;
		}
		if (hasUpperRow)
		{
			if (cellVecRef[idx - m_Width - 1].m_CurrAlive)
			{
				++aliveCtr;
			}
		}
		if (hasBottomRow)
		{
			if (cellVecRef[idx + m_Width - 1].m_CurrAlive)
			{
				++aliveCtr;
			}
		}
	}
	if (hasRightRow)
	{
		if (cellVecRef[idx + 1].m_CurrAlive)
		{
			++aliveCtr;
		}
		if (hasUpperRow)
		{
			if (cellVecRef[idx - m_Width + 1].m_CurrAlive)
			{
				++aliveCtr;
			}
		}
		if (hasBottomRow)
		{
			if (cellVecRef[idx + m_Width + 1].m_CurrAlive)
			{
				++aliveCtr;
			}
		}
	}
	if (hasBottomRow)
	{
		if (cellVecRef[idx + m_Width].m_CurrAlive)
		{
			++aliveCtr;
		}
	}
	if (hasUpperRow)
	{
		if (cellVecRef[idx - m_Width].m_CurrAlive)
		{
			++aliveCtr;
		}
	}
	return aliveCtr;
}

void CalcNextGen(std::vector<Cell> &cellVec, int genNr)
{
	for (size_t i = 0; i < cellVec.size(); ++i)
	{
		AliveNextGeneration(cellVec, i); //calculating what they will be
	}

	for (size_t i = 0; i < cellVec.size(); ++i)
	{
		cellVec[i].Update(); //calculating whether the cells will live in the next gen
	}
}

void PrintGen(const std::vector<Cell> &cellVec, int outputNr)
{
	std::ofstream output("./output/output" + std::to_string(outputNr) + ".txt");

	int result = 0;
	for (size_t i = 0; i < cellVec.size(); ++i)
	{
		if (cellVec[i].m_CurrAlive)
		{
			output << '#';
			++result;
		}
		else
		{
			output << '.';
		}

		if ((i + 1) % m_Width == 0)
		{
			output << "\n";
		}
	}
	std::cout << "Answer for gen " << outputNr << " is: " << result << std::endl;
}
