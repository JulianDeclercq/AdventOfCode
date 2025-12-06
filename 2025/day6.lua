-- local lines = io.lines("example/day6.txt")
local lines = io.lines("input/day6.txt")
local numbers = {} -- a table of tables
local operators = {}
for line in lines do
	local first_character = line:sub(1, 1)
	local is_operator = first_character == '*' or first_character == '+'
	local index = 1
	for word in line:gmatch("%S+") do -- match all non-space characters
		if is_operator then
			table.insert(operators, word)
			goto continue
		end

		if numbers[index] == nil then
			numbers[index] = {}
		end

		table.insert(numbers[index], tonumber(word))

		index = index + 1

	    ::continue::
	end
end

local answer = 0
for i, inner in ipairs(numbers) do
		local operator = operators[i] -- not sure about this
		local sum_or_product = operator == '*' and 1 or 0
	for _, number in ipairs(inner) do
		if operator == '*' then
			sum_or_product = sum_or_product * number
		elseif operator == '+' then
			sum_or_product = sum_or_product + number
		end
	end
	answer = answer + sum_or_product
end
print(answer)
vim.api.nvim_buf_set_lines(0, -1, -1, false, { "-- answer: " .. string.format("%.0f", answer) })
