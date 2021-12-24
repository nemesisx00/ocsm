
export function normalizeClassNames(className, ...additions)
{
	let out = []
	if(className && typeof className === 'string')
		className.split(' ').forEach(word => out.push(word.trim()))
	
	if(additions)
		additions.forEach(word => {
			if(!!word)
				word.split(' ').forEach(w => out.push(w.trim()))
		})
	return out.filter(val => val.length > 0).join(' ').trim()
}

export function equals(v1, v2)
{
	let ret = false
	if(typeof v1 === 'object')
	{
		if(Array.isArray(v1) && Array.isArray(v2))
			ret = v1.reduce((prev, curr, i) => prev && equals(curr, v2[i]))
		else if(typeof v2 === 'object')
			ret = Object.entries(v1).reduce((prev, curr) => prev && equals(curr[1], v2[curr[0]]))
	}
	else if(typeof v2 !== 'object')
		ret = v1 == v2
	return ret
}

export function compareStrings(s1, s2)
{
	//Make empty strings go to the end of the list
	if(typeof s1 === 'string' && typeof s2 === 'string')
	{
		if(s1.length < 1 && s2.length > 0)
			return 1
		if(s1.length > 0 && s2.length < 1)
			return -1
	}
	
	if(s1 < s2)
		return -1
	if(s1 > s2)
		return 1
	return 0
}
