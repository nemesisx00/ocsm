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
namespace NameSpace
{
	public class ClassName
	{
		public void functionName()
		{
			if(isTrue)
			{
				collection.ForEach(v => {
					if(v is bool)
						isFalse = isFalse && v;
				});
			}
			
			if(isFalse)
				doFalse();
		}
		
		public void doFalse() { isTrue = false; }
	}
}
```
- Use `PascalCase` for constants, static variables, class/enum/object/struct names, and namespaces
- Use `camelCase` for functions, non-constant and non-static variable names
- Prefer iterating with a guaranteed end condition
- Avoid hard-coding raw data
	- Prefer creating immutable variables when necessary

## C# Style Guide

- Use `PascalCase` for Properties
