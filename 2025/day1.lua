-- local lines = io.lines("example/day1.txt")
local lines = io.lines("input/day1.txt")
local dial = 50
local answer = 0
for line in lines do
	local direction = line:sub(1, 1)
	local diff = math.abs(tonumber(line:sub(2)) or 0) % 100
	if direction == "L" then
		dial = dial - diff
		if dial < 0 then
			dial = 100 - math.abs(dial)
		end
	elseif direction == "R" then
		dial = (dial + diff) % 100
	end
	if dial == 0 then
		answer = answer + 1
	end
end

print("Day 1 part 1: " .. answer)
