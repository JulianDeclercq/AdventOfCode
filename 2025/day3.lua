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

local function part1()
	-- local lines = io.lines("example/day3.txt")
	local lines = io.lines("input/day3.txt")
	local answer = 0
	for battery in lines do
		local highest, highest_idx = find_highest(battery, 1, #battery - 1)
		local next_highest = find_highest(battery, highest_idx + 1, #battery)
		answer = answer + tonumber(highest .. next_highest)
	end
	return answer
end

local function part2()
	-- local lines = io.lines("example/day3.txt")
	local lines = io.lines("input/day3.txt")
	local answer = 0
	for battery in lines do
		local numbers = {}
		local highest_idx = 0
		local joltage = ""
		for i = 1, 12, 1 do
			local highest, new_highest_idx = find_highest(battery, highest_idx + 1, #battery - (12 - i))
			if new_highest_idx == nil then
				return
			end
			highest_idx = new_highest_idx
			table.insert(numbers, highest)
			joltage = joltage .. highest
		end
		answer = answer + tonumber(joltage)
	end
	return answer
end

print(part1())
vim.api.nvim_buf_set_lines(0, -1, -1, false, { "-- part2: " .. string.format("%.0f", part2()) })
