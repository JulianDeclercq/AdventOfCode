package.loaded["helpers"] = nil
local helpers = require("helpers")
local inspect = require("inspect")
local lines = io.lines("example/day8.txt")
local boxes = {}
local circuits = {}
for line in lines do
	local split = helpers.split(line, ",")
	local circuit_id = helpers.uuid()
	local box = { x = tonumber(split[1]), y = tonumber(split[2]), z = tonumber(split[3]), circuit = circuit_id }
	table.insert(boxes, box)
	circuits[circuit_id] = box
end

local function euclidean_distance(p1, p2)
	return math.sqrt(math.pow(p1.x - p2.x, 2) + math.pow(p1.y - p2.y, 2) + math.pow(p1.z - p2.z, 2))
end

local function box_sorter(lhs, rhs)
	if lhs.x ~= rhs.x then
		return lhs.x < rhs.x
	elseif lhs.y ~= rhs.y then
		return lhs.y < rhs.y
	else
		return lhs.z < rhs.z
	end
end

-- to ensure consistency in which gets added to which, sort and always pick the first one
-- TODO: Explain better in comment
local function circuit_candidate(p1, p2)
	local sorted = { p1, p2 }
	table.sort(sorted, box_sorter)
	return sorted[1]
end

local function unique_key(p1, p2)
	local sorted = { p1, p2 }
	table.sort(sorted, box_sorter)
	return inspect(sorted) -- use inspect as a general "to string"
end

local distances_lookup = {}
local distances_list = {}
local function part1()
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

	-- sort by distance
	table.sort(distances_list, function(lhs, rhs)
		return lhs.distance < rhs.distance
	end)

	-- traverse the pairs and start making circuits
	for i, dist in ipairs(distances_list) do
		-- TODO: Check if the other box is already in a non-single circuit instead of always picking the candidate? not sure. need an example
		-- TODO: What happens when both are already in a circuit? do both circuits get merged?

		-- TODO: How am i going to update the circuit on all distances lmaoo I need references
		print(inspect(dist))
		break
	end

	-- for _, circuit in pairs(circuits) do
	-- 	print(inspect(circuit))
	-- end
end

part1()
