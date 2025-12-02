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

local function is_invalid(id)
	local first_str = id:sub(0, #id / 2)
	local first = tonumber(first_str)
	local second_str = id:sub((#id / 2) + 1)
	local second = tonumber(second_str)

	-- avoid problems with leading zeroes in second_str
	if #first_str == #second_str then
		if first == second then
			return true
		end
	end
end

local function is_invalid_pattern(id, pattern)
	-- if id length isn't a multiple of the pattern it can't be invalid
	if #id % #pattern ~= 0 then
		return false
	end

	for i = 1, #id, #pattern do -- not sure about this for loop hihi
		local temp = id:sub(i, i + #pattern - 1)
		-- print("temp " .. temp)
		if temp ~= pattern then
			return false
		end
	end

	return true
end

local function is_invalid_part2(id)
	local patterns = {}

	-- and at -1 to avoid full string being a match
	for i = 1, #id - 1, 1 do
		table.insert(patterns, id:sub(1, i))
	end

	-- print(table.concat(patterns, "\n"))

	for _, pattern in ipairs(patterns) do
		if is_invalid_pattern(id, pattern) then
			-- print("invalid pattern found for " .. id .. ":" .. pattern)
			return true
		end
	end
	return false
end

local function solve()
	-- local line = io.lines("example/day2.txt")() -- () for first line only
	local line = io.lines("input/day2.txt")() -- () for first line only
	local ranges = split(line, ",")
	-- print(table.concat(ranges, "\n"))
	local answer = 0
	for _, range in ipairs(ranges) do
		-- print(range)
		local limits = split(range, "-")
		local min, max = limits[1], limits[2]
		for i = min, max, 1 do
			local id = tostring(i)
			if is_invalid_part2(id) then
				-- print("invalid " .. id)
				answer = answer + tonumber(id)
			end
		end
	end
	print(answer)
end

solve()
-- is_invalid_part2("696969")
