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

local function is_invalid_pattern(id, pattern)
	-- if id length isn't a multiple of the pattern it can't be invalid
	if #id % #pattern ~= 0 then
		return false
	end

	for i = 1, #id, #pattern do
		local temp = id:sub(i, i + #pattern - 1)
		if tonumber(temp) ~= tonumber(pattern) then
			return false
		end
	end

	return true
end

local function is_invalid(id, part)
	if part == 1 then
		local pattern = id:sub(1, #id / 2) -- split halfway
		return #id % 2 == 0 and is_invalid_pattern(id, pattern)
	end

	-- part 2
	local patterns = {}
	for i = 1, #id - 1, 1 do -- and at -1 to avoid full string being a match
		table.insert(patterns, id:sub(1, i))
	end

	for _, pattern in ipairs(patterns) do
		if is_invalid_pattern(id, pattern) then
			return true
		end
	end
	return false
end

local function solve(part)
	-- local line = io.lines("example/day2.txt")() -- () for first line only
	local line = io.lines("input/day2.txt")() -- () for first line only
	local ranges = split(line, ",")
	local answer = 0
	for _, range in ipairs(ranges) do
		local limits = split(range, "-")
		local min, max = limits[1], limits[2]
		for i = min, max, 1 do
			local id = tostring(i)
			if is_invalid(id, part) then
				answer = answer + tonumber(id)
			end
		end
	end
	print("answer " .. answer)
end

solve(1)
solve(2)
