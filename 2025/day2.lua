local function split(str, delimiter)
	local result = {}
	local from = 1
	local delim_from, delim_to = string.find(str, delimiter, from, true)
	while delim_from do
		table.insert(result, string.sub(str, from, delim_from - 1))
		from = delim_to + 1
		delim_from, delim_to = string.find(str, delimiter, from)
	end
	table.insert(result, string.sub(str, from))
	return result
end

local function is_valid_id(id)
	local first_str = id:sub(0, #id / 2)
	local first = tonumber(first_str)
	local second_str = id:sub((#id / 2) + 1)
	local second = tonumber(second_str)
	-- print("id" .. id)
	-- print("id_number " .. tonumber(id))

	-- avoid problems with leading zeroes in second_str
	if #first_str == #second_str then
		-- numeric equality
		if first == second then
			-- print("match " .. first_str .. " " .. second_str)
			return true
		end
	end
end

local function part1()
	-- local input = io.lines("example/day2.txt")
	local input = io.lines("input/day2.txt")
	for line in input do -- theres only one line so clean up this loop
		local ranges = split(line, ",")
		-- print(table.concat(ranges, "\n"))
		local answer = 0
		for _, range in ipairs(ranges) do
			-- print(range)
			local limits = split(range, "-")
			local min, max = limits[1], limits[2]
			for i = min, max, 1 do
				local id = tostring(i)
				if is_valid_id(id) then
					answer = answer + tonumber(id)
				end
			end
		end
		print(answer)
	end
end

part1()
