#include "Helpers.h"

void Helpers::StopWatch::Start()
{
	if (_running)
	{
		std::cout << "Stopwatch already running, can't start." << std::endl;
		return;
	}
	_start = std::chrono::high_resolution_clock::now();
	_running = true;
}

void Helpers::StopWatch::Stop()
{
	if (!_running)
	{
		std::cout << "Stopwatch was not running, can't stop." << std::endl;
		return;
	}
	_elapsed = std::chrono::high_resolution_clock::now() - _start;
	_running = false;
}

void Helpers::StopWatch::Reset()
{
	_running = false;
	_elapsed = std::chrono::duration<long long, std::nano>::zero();
}

std::string Helpers::StopWatch::Formatted()
{
	if (_elapsed == std::chrono::duration<long long, std::nano>::zero())
		return "StopWatch::Formatted error: no time has elapsed.";

	// cast to microseconds and divide rather than cast to seconds to avoid losing precision
	// casting to seconds with any amount under 1 second would lead to "0 seconds"
	std::string s = "\t| ";
	s += std::to_string(std::chrono::duration_cast<std::chrono::microseconds>(_elapsed).count() / 1000000.0);
	s += "s (";
	s += std::to_string(std::chrono::duration_cast<std::chrono::milliseconds>(_elapsed).count());
	s += "ms) |";
	return s;
}