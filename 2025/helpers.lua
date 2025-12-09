local helpers = {}

function helpers.split(str, delimiter)
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

function helpers.dump(object)
	if type(object) == "table" then
		local s = "{ "
		for k, v in pairs(object) do
			if type(k) ~= "number" then
				k = '"' .. k .. '"'
			end
			s = s .. "[" .. k .. "] = " .. helpers.dump(v) .. ","
		end
		return s .. "} "
	else
		return tostring(object)
	end
end

math.randomseed(os.time())
local random = math.random
function helpers.uuid()
	local template = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx"
	return string.gsub(template, "[xy]", function(c)
		local v = (c == "x") and random(0, 0xf) or random(8, 0xb)
		return string.format("%x", v)
	end)
end

function helpers.short_id()
	local uuid = helpers.uuid()
	return uuid:sub(1, 8)
end

return helpers
