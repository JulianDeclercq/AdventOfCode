-- local lines = io.lines("example/day3.txt")
local lines = io.lines("input/day3.txt")
local answer = 0

local function find_highest(str, start_idx, end_idx)
	local highest, highest_idx = -1, -1
	for i = start_idx, end_idx, 1 do
		local current = tonumber(str:sub(i, i))
		if current == nil then
			return
		end

		if current > highest then
			highest = current
			highest_idx = i
		end
	end
	return highest, highest_idx
end

for battery in lines do
	local highest, highest_idx = find_highest(battery, 1, #battery - 1)
	local next_highest = find_highest(battery, highest_idx + 1, #battery)
	answer = answer + tonumber(highest .. next_highest)
end

print(answer)
