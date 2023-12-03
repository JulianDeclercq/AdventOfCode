#include <iostream>
#include <vector>
#include <fstream>
#include <string>
#include "save_png.h"

enum CommandType
{
	NONE,
	TURNON,
	TURNOFF,
	TOGGLE
};

struct Command
{
	Command() //default constructor
	{
		xBegin = 0;
		yBegin = 0;
		xEnd = 0;
		yEnd = 0;
		cmdType = CommandType::NONE;
	};

	Command(int xMin, int yMin, int xMax, int yMax, CommandType command)
	{
		xBegin = xMin;
		yBegin = yMin;
		xEnd = xMax;
		yEnd = yMax;
		cmdType = command;
	}

	CommandType cmdType;
	size_t xBegin = 0, yBegin = 0, xEnd = 0, yEnd = 0;
};

struct Pixel
{
	Pixel() {}; //default constructor
	Pixel(unsigned char b, unsigned char g, unsigned char r) : m_b(b), m_g(g), m_r(r)
	{
	}
	unsigned char m_b;
	unsigned char m_g;
	unsigned char m_r;
};

void ReadFromFile(std::string path, std::vector<std::string> &commands);
void ExecuteCommandOnRegion(Command cmd);
Command StringToCommand(std::string &str);
void CheckLights();
void WritePng(int number);

int m_Dim = 1000;
std::vector<bool> lights;
std::vector<int> lightsBrightness;

int main()
{
	lights.reserve(m_Dim * m_Dim);
	for (int i = 0; i < (m_Dim * m_Dim); ++i)
	{
		lights.push_back(false);
	}

	lightsBrightness.reserve(m_Dim * m_Dim);
	for (int i = 0; i < (m_Dim * m_Dim); ++i)
	{
		lightsBrightness.push_back(0);
	}

	std::vector<std::string> commands;
	ReadFromFile("./input.txt", commands);

	//std::string input = "toggle 0,0 through 999,999";
	//ExecuteCommandOnRegion(StringToCommand(input));

	for (std::string str : commands)
	{
		ExecuteCommandOnRegion(StringToCommand(str));
	}

	CheckLights();
	WritePng(1);

	system("pause");
	return 0;
}

void ReadFromFile(std::string path, std::vector<std::string> &commands)
{
	std::ifstream input;
	input.open(path);
	if (input.fail())
	{
		std::cout << "Failed to open file from path: " << path << std::endl;
	}

	while (!input.eof())
	{
		std::string extractedLine;
		std::getline(input, extractedLine);
		commands.push_back(extractedLine);
	}

	std::cout << "File has been loaded." << std::endl;
	input.close();
}

void ExecuteCommandOnRegion(Command cmd)
{
	int ctr = 0;
	int nrOfLights = ((cmd.xEnd - cmd.xBegin) + 1) * ((cmd.yEnd - cmd.yBegin) + 1);
	//for (size_t i = 0; (int)i < (cmd.yEnd - cmd.yBegin) + 1; ++i)
	for (size_t i = cmd.yBegin; i <= cmd.yEnd; ++i)
	{
		for (size_t j = cmd.xBegin; (int)j < cmd.xEnd + 1; ++j)
		{
			switch (cmd.cmdType)
			{
			case CommandType::NONE:
				std::cout << "Invalid command for ExecuteCommandOnRegion. (None)" << std::endl;
				break;
			case CommandType::TURNON:
				lights[j + (i * m_Dim)] = true;
				++lightsBrightness[j + (i * m_Dim)];
				break;
			case CommandType::TURNOFF:
				lights[j + (i * m_Dim)] = false;
				if (lightsBrightness[j + (i * m_Dim)] > 0)
				{
					--lightsBrightness[j + (i * m_Dim)];
				}
				break;
			case CommandType::TOGGLE:
				lights[j + (i * m_Dim)] = !lights[j + (i * m_Dim)];
				lightsBrightness[j + (i * m_Dim)] += 2;
				break;
			default: std::cout << "Invalid command for ExecuteCommandOnRegion. (Default)" << std::endl;
				break;
			}
			++ctr;
		}
	}

	//Info
	/*if (ctr == nrOfLights)
	{
		std::cout << "Counter matches." << std::endl;
	}
	else
	{
		std::cout << "Counter does not match." << std::endl;
	}

	//std::cout << "Region contains " << nrOfLights << " lights." << std::endl;*/
}

Command StringToCommand(std::string &str)
{
	//Command
	CommandType cmdType = CommandType::NONE;

	size_t space1 = str.find(' ');
	size_t space2 = str.substr(space1 + 1, std::string::npos).find(' ') + space1;
	size_t lastSpace = str.rfind(' ');

	if (str.substr(0, space1).compare("toggle") == 0)
	{
		cmdType = CommandType::TOGGLE;
	}
	else
	{
		if (str.substr(space1 + 1, space2 - space1).compare("off") == 0)
		{
			cmdType = CommandType::TURNOFF;
		}
		else
		{
			cmdType = CommandType::TURNON;
		}
	}

	//Coordinates
	int xMin = 0, yMin = 0, xMax = 0, yMax = 0;
	size_t firstComma = str.find(',');
	size_t lastComma = str.rfind(',');
	size_t space3 = str.substr(0, 20).rfind(' ');

	if (cmdType == CommandType::TOGGLE) //after first space
	{
		xMin = std::stoi(str.substr(space1 + 1, firstComma - space1 - 1));
		yMin = std::stoi(str.substr(firstComma + 1, space2 - firstComma));
	}
	else
	{
		xMin = std::stoi(str.substr(space2 + 2, firstComma - space2 - 2));
		yMin = std::stoi(str.substr(firstComma + 1, space3 - firstComma - 1));
	}

	xMax = std::stoi(str.substr(lastSpace + 1, lastComma - lastSpace - 1));
	yMax = std::stoi(str.substr(lastComma + 1));

	return Command(xMin, yMin, xMax, yMax, cmdType);
}

void CheckLights()
{
	int lightsOn = 0, lightsOff = 0;
	for (bool b : lights) 
	{
		if (b)
		{
			++lightsOn;
		}
		else
		{
			++lightsOff;
		}
	}
	std::cout << "Lights on:  " << lightsOn << std::endl;
	std::cout << "Lights off: " << lightsOff << std::endl;

	int ctr = 0;
	for (int b : lightsBrightness)
	{
		ctr += b;
	}
	std::cout << "Brightness is " << ctr << std::endl;
}

void WritePng(int number)
{
	int pxlsNeeded = m_Dim * m_Dim;
	std::vector<Pixel> pixelVector;
	pixelVector.reserve(pxlsNeeded);
	for (bool b : lights)
	{
		if (b)
		{
			pixelVector.push_back(Pixel(0, 220, 220));
		}
		else
		{
			pixelVector.push_back(Pixel(0, 0, 0));
		}
	}

	save_png(("./output/visualisation" + std::to_string(number) + ".png"), pixelVector.data(), m_Dim, m_Dim);
}