#pragma once
#include <iostream>
#include <string>
#include <fstream>
#include <regex>
#include <vector>
#include <algorithm>
#include <tuple> // for std::tie
#include <map>
#include "Helpers.h"

using namespace std;

struct Particle
{
	Particle() : ID(-1), Position(Helpers::Point3D()), Velocity(Helpers::Point3D()), Acceleration(Helpers::Point3D())
	{
	}

	Particle(int id, Helpers::Point3D position, Helpers::Point3D velocity, Helpers::Point3D acceleration) : ID(id), Position(position), Velocity(velocity), Acceleration(acceleration)
	{
	}

	// Overload comparison operator
	bool operator<(const Particle& rhs) const
	{
		// Variables need to be used because std::tie does not accept the direct syntax
		int aManhattan = Acceleration.Manhattan();
		int vManhattan = Velocity.Manhattan();
		int pManhattan = Position.Manhattan();

		int rhsAManhattan = rhs.Acceleration.Manhattan();
		int rhsVManhattan = rhs.Velocity.Manhattan();
		int rhsPManhattan = rhs.Position.Manhattan();

		// Compares absolute acceleration first, then initial velocity and finally position
		return tie(aManhattan, vManhattan, pManhattan) < tie(rhsAManhattan, rhsVManhattan, rhsPManhattan);
	}

	// Use a Point3D to store velocity as well, for compactness
	Helpers::Point3D Position, Velocity, Acceleration;
	int ID;

public:
	void Update()
	{
		// Increase the X velocity by the X acceleration.
		Velocity.X += Acceleration.X;
		// Increase the Y velocity by the Y acceleration.
		Velocity.Y += Acceleration.Y;
		// Increase the Z velocity by the Z acceleration.
		Velocity.Z += Acceleration.Z;
		// Increase the X position by the X velocity.
		Position.X += Velocity.X;
		// Increase the Y position by the Y velocity.
		Position.Y += Velocity.Y;
		// Increase the Z position by the Z velocity.
		Position.Z += Velocity.Z;
	}
};

class Day20
{
private:
	void ParseInput(vector<Particle>& particles);
public:
	void Part1();
	void Part2();
};
