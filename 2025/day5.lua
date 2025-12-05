local helpers = require("helpers")
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
