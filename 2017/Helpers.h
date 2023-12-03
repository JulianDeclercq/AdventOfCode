#pragma once
#include <vector>
#include <sstream>
#include <string>
#include <iterator>
#include <tuple>

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

	// Point struct
	struct Point
	{
	public:
		Point() : X(0), Y(0) {}
		Point(int x, int y) : X(x), Y(y)
		{
		}
		int X;
		int Y;

	public:
		Point& operator+=(const Point& rhs)
		{
			X += rhs.X;
			Y += rhs.Y;
			return *this; // return the result by reference
		}

		Point& operator-=(const Point& rhs)
		{
			X -= rhs.X;
			Y -= rhs.Y;
			return *this; // return the result by reference
		}

		friend Point operator+(Point lhs, const Point& rhs)
		{
			return lhs += rhs;
		}

		friend Point operator-(Point lhs, const Point& rhs)
		{
			return lhs -= rhs;
		}

		// Naive implementation for map
		bool operator<(const Point& rhs) const
		{
			return std::tie(X, Y) < std::tie(rhs.X, rhs.Y);
		}
	};

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

		bool operator==(const Point3D& rhs) const
		{
			return (X == rhs.X) && (Y == rhs.Y) && (Z == rhs.Z);
		}

		// Naive implementation for map to work
		bool operator<(const Point3D& rhs) const
		{
			return std::tie(X, Y, Z) < std::tie(rhs.X, rhs.Y, rhs.Z);
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