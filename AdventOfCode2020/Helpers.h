#pragma once
#include <chrono>
#include <string>
#include <iostream>
#include <cmath>

namespace Helpers
{
    class StopWatch
    {
    private:
        bool _running = false;
        std::chrono::steady_clock::time_point _start;
        std::chrono::duration<long long, std::nano> _elapsed = std::chrono::duration<long long, std::nano>::zero();
    public:
        void Start();
        void Stop();
        void Reset();
        std::string Formatted();
    };

	struct point
	{
		point(int x, int y) : X(x), Y(y) {}
		int X = 0;
		int Y = 0;

		point(){}

		std::string ToString()
		{
			return std::to_string(X) + ", " + std::to_string(Y);
		}

		static int ManhattanDistance(const point& lhs, const point& rhs)
		{
			return abs(lhs.X - rhs.X) + abs(lhs.Y - rhs.Y);
		}

		point& operator+=(const point& rhs)
		{
			X += rhs.X;
			Y += rhs.Y;
			return *this; // return the result by reference
		}

		friend point operator* (const int lhs, const point& p)
		{
			return point(lhs * p.X, lhs * p.Y);
		}

		friend bool operator<(const point& lhs, const point& rhs)
		{
			return (lhs.X < rhs.X) || (lhs.X == rhs.X && lhs.Y < rhs.Y);
		}
	
	};

}
