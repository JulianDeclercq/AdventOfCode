local helpers = require("helpers")
-- local lines = io.lines("example/day5.txt")
local function part_1()
	local lines = io.lines("input/day5.txt")
	local pre_empty = true
	local ranges = {}
	local ingredients = {}
	local fresh = {}
	for line in lines do
		if line == "" then
			pre_empty = false
			goto continue
		end
		if pre_empty then
			local split = helpers.split(line, "-")
			table.insert(ranges, {
				min = tonumber(split[1]),
				max = tonumber(split[2]),
			})
		else
			table.insert(ingredients, tonumber(line))
		end
		::continue::
	end

	local answer = 0
	for _, range in ipairs(ranges) do
		for _, ingredient in ipairs(ingredients) do
			if ingredient >= range.min and ingredient <= range.max then
				-- avoid double counting ingredients
				if fresh[ingredient] == nil then
					fresh[ingredient] = true
					answer = answer + 1
				end
			end
		end
	end
	print(answer)

	-- for i, r in ipairs(ranges) do
	-- 	print(string.format("range %d: min=%d, max=%d", i, r.min, r.max))
	-- end

	-- print(table.concat(ingredients, "\n"))
end

-- part_1()

local function part_2()
	local ranges = {}
	local fresh = {}
	local lines = io.lines("example/day5.txt")
	for line in lines do
		if line == "" then
			break
		end

		local split = helpers.split(line, "-")
		table.insert(ranges, {
			min = tonumber(split[1]),
			max = tonumber(split[2]),
		})
	end

	for i, r in ipairs(ranges) do
		print(string.format("range %d: min=%d, max=%d", i, r.min, r.max))
	end

	-- sort the ranges by min so you can always compare "the next one with the previous one" instead of having to try in both directions
	local function range_sorting(lhs, rhs)
		return lhs.min < rhs.min
	end
	table.sort(ranges, range_sorting)

	-- combine the ranges
	local combined_ranges = {}

	for i, range in ipairs(ranges) do
		print("range " .. range.min .. " " .. range.max)
	end

	local search = true
	while search do
		for i = 1, #ranges - 1 do
			local current = ranges[i]
			local next = ranges[i + 1]
			print("range " .. ranges[i].min .. " " .. ranges[i].max)
		end

		-- somewhere here put search to false
	end
end

part_2()
