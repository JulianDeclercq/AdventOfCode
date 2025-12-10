package.loaded["helpers"] = nil
local helpers = require("helpers")
local inspect = require("inspect")
--
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

	-- local lines = io.lines("input/day5.txt")
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

	-- sort the ranges by min so you can always compare "the next one with the previous one" instead of having to try in both directions
	table.sort(ranges, function(lhs, rhs)
		return lhs.min < rhs.min
	end)

	-- combine the ranges
	local combined_ranges = {}

	for loops = 1, 10000000 do -- TODO: Calculate instead
		for i = 1, #ranges - 1 do
			-- print("PRINTING RANGES")
			-- for idx, r in ipairs(ranges) do
			-- 	print(string.format("range %d: min=%d, max=%d", idx, r.min, r.max))
			-- end

			local current = ranges[i]
			local next = ranges[i + 1]
			-- print("LOOP " .. i .. " range " .. ranges[i].min .. " " .. ranges[i].max)

			-- if in range, merge ranges
			if next.min >= current.min and next.min <= current.max then -- TODO: Edge cases?
				-- print("will update range on index " .. i .. "max from " .. ranges[i].max .. " to " .. next.max)
				-- print("BEFORE", inspect(ranges[i]))
				ranges[i].max = next.max -- TODO: Check if max is in range ? Actually don't think we need that validation
				table.remove(ranges, i + 1)
				-- print("AFTER", inspect(ranges[i]))
				break -- restart the loop to try again since it has now changed
				-- goto again
			end
		end
	end

	local answer = 0
	for _, range in ipairs(ranges) do
		-- print(inspect(range))
		answer = answer + (range.max - range.min) + 1
	end
	print("part 2 answer", helpers.long_print(answer))
	-- 441990622241300 is too high
	-- 422614199994505
	-- 324853356712071 is too low
end

-- part_1()
part_2()
