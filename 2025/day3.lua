-- local lines = io.lines("example/day3.txt")
local lines = io.lines("input/day3.txt")
local answer = 0
for battery in lines do
	local highest, highest_idx, next_highest = -1, -1, -1
	for i = 1, #battery - 1, 1 do -- ignore last time
		local current = tonumber(battery:sub(i, i))
		if current == nil then
			return
		end

		if current > highest then
			highest = current
			highest_idx = i
		end
	end
	for i = highest_idx + 1, #battery, 1 do
		local current = tonumber(battery:sub(i, i))
		if current == nil then
			return
		end

		if current > next_highest then
			next_highest = current
		end
	end
	answer = answer + tonumber(highest .. next_highest)
end

print(answer)
