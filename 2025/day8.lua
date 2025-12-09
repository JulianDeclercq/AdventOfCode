package.loaded["helpers"] = nil
local helpers = require("helpers")
local inspect = require("inspect")
local lines = io.lines("example/day8.txt")
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
	return inspect(sorted) -- use inspect as a general "to string"
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
		return
	end

	-- to ensure consistency in which gets added to which, always move the "bigger one" to the circuit of the "smaller one"  TODO: explain this better
	local sorted = { box1, box2 }
	table.sort(sorted, box_sorter)
	local target = sorted[1] -- TODO: Does this make sense?
	local to_move = sorted[2] -- TODO: Does this make sense?

	-- TODO: What if this circuit has more than just one? do both get joined? I suppose so? if so i need to implement it with a loop
	local target_circuit_id = box_to_circuit_lookup[target.id]
	local from_circuit_id = box_to_circuit_lookup[to_move.id] -- TODO Double check this is not a reference
	box_to_circuit_lookup[to_move.id] = target_circuit_id
	table.insert(circuits[target_circuit_id], to_move.id) -- add the new box id to the circuit

	circuits[from_circuit_id] = nil -- remove the old circuit

	-- dbg_locals()
end

local distances_lookup = {} -- only used to prevent duplicate pairs (a <-> b and b <-> a)
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

	-- traverse the pairs and make circuits
	local AMOUNT = 10
	for i, dist in ipairs(distances_list) do
		print("circuits")
		for key, circuit in pairs(circuits) do
			print(key, " => ", inspect(circuit))
		end

		if i > AMOUNT then
			break
		end

		-- TODO: Check if the other box is already in a non-single circuit instead of always picking the candidate? not sure. need an example
		-- TODO: What happens when both are already in a circuit? do both circuits get merged?
		print("connecting " .. inspect(dist))

		connect(dist.p1, dist.p2)
	end
end

part1()
