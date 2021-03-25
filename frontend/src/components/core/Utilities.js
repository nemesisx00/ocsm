
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
