local helpers = require("helpers")
local inspect = require("inspect")

local function solve()
	-- local lines = io.lines("example/day5.txt")
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

	-- sort the ranges by min so you can always compare "the next one with the previous one" instead of having to try in both directions
	-- meaning you know that next.min is going to be greater than current.min
	table.sort(ranges, function(lhs, rhs)
		return lhs.min < rhs.min
	end)

	::again::
	for i = 1, #ranges - 1 do
		local current = ranges[i]
		local next = ranges[i + 1]

		-- if current contains next in range, merge ranges
		if next.min >= current.min and next.min <= current.max then
			-- next is fully contained by current, can you just remove it
			if next.max <= current.max then
				table.remove(ranges, i + 1)
				goto again -- restart the loop to try again since it has now changed
			end
			-- next starts within current but is bigger, so expand current and remove next
			if next.max > current.max then
				ranges[i].max = next.max
				table.remove(ranges, i + 1)
				goto again -- restart the loop to try again since it has now changed
			end
		end
	end

	local part_1_answer = 0
	for _, ingredient in ipairs(ingredients) do
		for _, range in ipairs(ranges) do
			if ingredient >= range.min and ingredient <= range.max then
				part_1_answer = part_1_answer + 1
				break
			end
		end
	end
	print("part 1 answer", helpers.long_print(part_1_answer))

	local part_2_answer = 0
	for _, range in ipairs(ranges) do
		part_2_answer = part_2_answer + (range.max - range.min) + 1
	end
	print("part 2 answer", helpers.long_print(part_2_answer))
end

solve()
