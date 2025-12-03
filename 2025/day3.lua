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

local function solve(digits)
	-- local lines = io.lines("example/day3.txt")
	local lines = io.lines("input/day3.txt")
	local answer = 0
	for battery in lines do
		local highest_idx = 0
		local joltage = ""
		for i = 1, digits, 1 do
			local highest, new_highest_idx = find_highest(battery, highest_idx + 1, #battery - (digits - i))
			if new_highest_idx == nil then
				return
			end
			highest_idx = new_highest_idx
			joltage = joltage .. highest
		end
		answer = answer + tonumber(joltage)
	end
	return answer
end

print(solve(2))
print(string.format("%.0f", solve(12)))
-- vim.api.nvim_buf_set_lines(0, -1, -1, false, { "-- part2: " .. string.format("%.0f", solve(12)) })
