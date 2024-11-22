using System;
using System.Collections.Generic;

namespace Ocsm.Meta;

public class GameSystem()
{
	public string Name { get; set; }
	public Type MetadataContainerType { get; set; }
	public List<string> MetadataTypes { get; set; } = [];
	public string SheetDefaultName { get; set; }
	public string SheetResourcePath { get; set; }
}
