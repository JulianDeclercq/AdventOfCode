package.loaded["helpers"] = nil
local helpers = require("helpers")
local inspect = require("inspect")

-- local input_file = "example/day8.txt"
local input_file = "input/day8.txt"
local lines = io.lines(input_file)
local boxes = {}
local circuits = {}
local box_to_circuit_lookup = {}
for line in lines do
	local split = helpers.split(line, ",")
	local box_id = "box-" .. helpers.short_id()
	local circuit_id = "circuit-" .. helpers.short_id()
	local box = { id = box_id, x = tonumber(split[1]), y = tonumber(split[2]), z = tonumber(split[3]) }
	table.insert(boxes, box)
	circuits[circuit_id] = { box_id }
	box_to_circuit_lookup[box_id] = circuit_id
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

local function unique_key(p1, p2)
	local sorted = { p1, p2 }
	table.sort(sorted, box_sorter)
	return sorted[1].id .. "-" .. sorted[2].id
end

local function dbg_locals()
	print("debugging locals!")
	for i = 1, math.huge do
		local name, value = debug.getlocal(2, i)
		if not name then
			break
		end
		print(i, inspect(name), inspect(value))
	end
end

local function connect(box1, box2)
	-- if already in same circuit, move on
	local box1_circuit = box_to_circuit_lookup[box1.id]
	local box2_circuit = box_to_circuit_lookup[box2.id]

	if box1_circuit == box2_circuit then
		-- print("DIDNT MAKE A CONNECTION")
		return
	end

	-- to ensure consistency in which gets added to which, always move the "bigger one" to the circuit of the "smaller one"  TODO: explain this better and make sure this is logical lmao
	local sorted = { box1, box2 }
	table.sort(sorted, box_sorter)
	local target = sorted[1]
	local to_move = sorted[2]

	local target_circuit_id = box_to_circuit_lookup[target.id]
	local source_circuit_id = box_to_circuit_lookup[to_move.id]
	local source_circuit = circuits[source_circuit_id]
	local target_circuit = circuits[target_circuit_id]
	-- dbg_locals()

	for _, box_id in ipairs(source_circuit) do
		box_to_circuit_lookup[box_id] = target_circuit_id -- update the lookup
		table.insert(target_circuit, box_id) -- add the new box id to the circuit
	end

	circuits[source_circuit_id] = nil -- remove the old circuit entirely
end

local distances_lookup = {} -- only used to prevent duplicate pairs (a <-> b and b <-> a)
local distances_list = {}
local function part1()
	for _, outer in ipairs(boxes) do
		for _, inner in ipairs(boxes) do
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

	-- print("INITIAL circuits")
	-- for key, circuit in pairs(circuits) do
	-- 	print(key, " => ", inspect(circuit))
	-- end

	-- traverse the pairs and make circuits
	local CONNECTIONS_TO_MAKE = string.find(input_file, "example") and 10 or 1000
	for i, dist in ipairs(distances_list) do
		if i > CONNECTIONS_TO_MAKE then
			break
		end
		-- print("loop", i)

		connect(dist.p1, dist.p2)

		-- for key, circuit in pairs(circuits) do
		-- 	print(key, " => ", inspect(circuit))
		-- end
	end

	local circuit_counts = {}
	for _, circuit in pairs(circuits) do
		table.insert(circuit_counts, helpers.table_length(circuit))
	end

	table.sort(circuit_counts, function(lhs, rhs)
		return lhs > rhs
	end)
	print("Part 1 answer", circuit_counts[1] * circuit_counts[2] * circuit_counts[3])
end

part1()
