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

	-- combine the ranges
	local combined_ranges = {}
	for _, range in ipairs(ranges) do
		local got_combined = false
		for _, combined_range in ipairs(combined_ranges) do
			-- if the min of the new range is INSIDE the combined range
			if range.min >= combined_range.min and range.min <= combined_range.max then
				-- combine the ranges if it's now bigger
				if range.max > combined_range.max then
					combined_range.max = range.max
					got_combined = true
					break
				end
			end
		end

		-- add as new combined range since it wasn't combinable with any existing
		if not got_combined then
			table.insert(combined_ranges, range)
		end
	end
	print(combined_ranges.min)
	print(combined_ranges.max)
end

part_2()
