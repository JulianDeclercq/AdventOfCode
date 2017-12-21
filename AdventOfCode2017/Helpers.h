#pragma once
#include <vector>
#include <sstream>
#include <string>
#include <iterator>

namespace Helpers
{
	template<typename T>
	void Split(const std::string &s, char delim, T result)
	{
		std::stringstream ss(s);
		std::string item;
		while (std::getline(ss, item, delim))
			*(result++) = item;
	}

	std::vector<std::string> Split(const std::string &s, char delim);

	// Point3D struct
	struct Point3D
	{
	public:
		Point3D() : X(0), Y(0), Z(0) {}
		Point3D(int x, int y, int z) : X(x), Y(y), Z(z)
		{
		}
		int X, Y, Z;

	public:
		Point3D& operator+=(const Point3D& rhs)
		{
			X += rhs.X;
			Y += rhs.Y;
			Z += rhs.Z;
			return *this; // return the result by reference
		}

		Point3D& operator-=(const Point3D& rhs)
		{
			X -= rhs.X;
			Y -= rhs.Y;
			Z -= rhs.Z;
			return *this; // return the result by reference
		}

		friend Point3D operator+(Point3D lhs, const Point3D& rhs)
		{
			return lhs += rhs;
		}

		friend Point3D operator-(Point3D lhs, const Point3D& rhs)
		{
			return lhs -= rhs;
		}

		int Manhattan() const
		{
			return abs(X) + abs(Y) + abs(Z);
		}
	};
}