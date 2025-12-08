-- package.loaded["helpers"] = nil
local helpers = require("helpers")
local inspect = require("inspect")
local lines = io.lines("example/day8.txt")
local boxes = {}
for line in lines do
	local split = helpers.split(line, ",")
	table.insert(boxes, { x = split[1], y = split[2], z = split[3] })
end

local function euclidean_distance(p1, p2)
	return math.sqrt(math.pow(p1.x - p2.x, 2) + math.pow(p1.y - p2.y, 2) + math.pow(p1.z - p2.z, 2))
end

local function part1()
	for amount = 1, 10 do
		local lowest_distance = 99999999999999
		local lhs, rhs = {}, {}
		for i, outer in ipairs(boxes) do
			for j, inner in ipairs(boxes) do
				if inner ~= outer then
					local dist = euclidean_distance(outer, inner)
					if dist < lowest_distance then
						lowest_distance = dist
						lhs = inner
						rhs = outer
					end
				end
			end
		end

		print(inspect(lhs))
		print(inspect(rhs))
	end
end

part1()
