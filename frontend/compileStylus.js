const dree = require('dree')
const { exec } = require('child_process')

const generateCommand = (path) => `stylus --compress stylus/components/${path} --out src/components/${path}`

const tree = dree.scan('./stylus/components', {
	normalize: true,
	depth: 10,
	descendants: true,
	descendantsIgnoreDirectories: true,
	extensions: [ 'styl' ]
})

const fillCommandList = (obj, commandList) => {
	for(let child of obj.children)
	{
		if(child.type === 'directory')
		{
			let command = generateCommand(child.relativePath)
			commandList.push(command)
		}
		
		if(child.children && child.children.length > 0)
			fillCommandList(child, commandList)
	}
}

let commands = [ `stylus --compress stylus/core --out src/` ]
fillCommandList(tree, commands)
//console.log(commands)

for(let command of commands)
{
	exec(command, (err, stdout, stderr) => {
		if(err)
			console.error(err)
		else
			console.log(stdout)
	})
}
