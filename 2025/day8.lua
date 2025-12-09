package.loaded["helpers"] = nil
local helpers = require("helpers")
local inspect = require("inspect")
local lines = io.lines("example/day8.txt")
local boxes = {}
for line in lines do
	local split = helpers.split(line, ",")
	table.insert(
		boxes,
		{ x = tonumber(split[1]), y = tonumber(split[2]), z = tonumber(split[3]), circuit = helpers.uuid() }
	)
end

local function euclidean_distance(p1, p2)
	return math.sqrt(math.pow(p1.x - p2.x, 2) + math.pow(p1.y - p2.y, 2) + math.pow(p1.z - p2.z, 2))
end

local function unique_key(p1, p2)
	local sorted = { p1, p2 }
	table.sort(sorted, function(lhs, rhs)
		if lhs.x ~= rhs.x then
			return lhs.x < rhs.x
		elseif lhs.y ~= rhs.y then
			return lhs.y < rhs.y
		else
			return lhs.z < rhs.z
		end
	end)
	return inspect(sorted) -- use inspect as a general "to string"
end

local distances_lookup = {}
local distances_list = {}
local function part1()
	for amount = 1, 10 do
		local lowest_distance = 99999999999999
		local lhs, rhs = {}, {}
		for i, outer in ipairs(boxes) do
			for j, inner in ipairs(boxes) do
				if inner ~= outer then
					local key = unique_key(outer, inner)
					if distances_lookup[key] == nil then
						local distance = euclidean_distance(outer, inner)
						local box = { p1 = outer, p2 = inner, distance = distance }
						distances_lookup[key] = box
						table.insert(distances_list, box)
					end
				end
			end
		end
	end

	-- sort by distance
	table.sort(distances_list, function(lhs, rhs)
		return lhs.distance < rhs.distance
	end)

	-- traverse the pairs and start making circuits
	local circuits = {}
	for _, dist in ipairs(distances_list) do
		print(inspect(dist))
		-- break
	end
end

part1()
