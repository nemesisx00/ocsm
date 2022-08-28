using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using OCSM;
using OCSM.Meta;

public class MetadataManager : Node
{
	[Signal]
	public delegate void MetadataLoaded(List<Metadata> metadata);
	[Signal]
	public delegate void MetadataSaved();
	
	private const string FileNameFormat = "{0}.ocmd";
	
	public string CurrentGameSystem { get; set; } = String.Empty;
	public List<Metadata> MetaData { get; private set; } = new List<Metadata>();
	
	public override void _Ready()
	{
		//
	}
	
	public void loadGameSystemMetadata(string gameSystem)
	{
		var filename = String.Format(FileNameFormat, gameSystem);
		var path = System.IO.Path.GetFullPath(FileSystemUtilities.DefaultMetadataDirectory + filename);
		var json = FileSystemUtilities.ReadString(path);
		MetaData = JsonSerializer.Deserialize<List<Metadata>>(json);
		
		EmitSignal(nameof(MetadataLoaded), MetaData);
	}
	
	public void saveGameSystemMetadata(string gameSystem)
	{
		var metadata = JsonSerializer.Serialize(MetaData);
		var filename = String.Format(FileNameFormat, gameSystem);
		var path = System.IO.Path.GetFullPath(FileSystemUtilities.DefaultMetadataDirectory + filename);
		FileSystemUtilities.WriteString(path, metadata);
		
		EmitSignal(nameof(MetadataSaved));
	}
}
