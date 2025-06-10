# WIP

This document is a work in progress so look forward to a more well defined guide to contributions in the future.

# Code of Conduct

See [CODE_OF_CONDUCT.md](https://github.com/nemesisx00/ocsm/blob/main/CODE_OF_CONDUCT.md)

# Contributing

TBD

## Discord

If you haven't already, consider joining the [Discord Community](https://discord.gg/jqak5jCB6E). Request the Contributor role to gain access to the development channels where we can all collaborate in real time.

## Report a Bug

TBD

## Request an Enhancement

TBD

# Code Style

## General Style Guide

Regardless of the language, code contributions must follow this style guide. A particular style may be exempted if the given file format or language explicitly disallows it.

- Use tabs for indentation, not spaces
- Place scope open characters on the next line, aligned vertically with their matching scope close character.
	- Exceptions:
		- Short, individual statements that are easy to read on a single line or without an explicitly defined scope
		- Adhoc functions, closures, and lambdas which require multiple lines
```csharp
//Example in C#
namespace Namespace;

public class ClassName(bool initialValue = false)
{
	public bool MemberReadonlyProperty => memberVariable;
	
	private bool memberVariable = initialValue;
	private List<object> collection = [];
	
	public void FunctionName()
	{
		if(memberVariable)
		{
			collection.ForEach(v => {
				if(v is bool b)
					memberVariable = b;
			});
		}
		
		if(!memberVariable)
			doFalse();
	}
	
	private void doFalse() => memberVariable = true;
}
```
- Use `PascalCase` for constants, static variables, and class/enum/object/struct names
	- Otherwise use `camelCase`
- Keep namespace segments to single words as much as possible, only capitalizing the first letter
- Avoid hard-coding raw data
	- Prefer creating immutable variables when necessary

## Rust Style Guide

- Use the naming conventions as defined above
	- Do not use `snake_case`
