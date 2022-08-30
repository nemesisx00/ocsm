using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using OCSM;
using OCSM.Meta;
using OCSM.CoD.Meta;
using OCSM.CoD.CtL.Meta;
using OCSM.DnD.Fifth.Meta;

public class MetadataManager : Node
{
	[Signal]
	public delegate void GameSystemChanged(string gameSystem);
	[Signal]
	public delegate void MetadataLoaded(SignalPayload<List<Metadata>> payload);
	[Signal]
	public delegate void MetadataSaved();
	
	private const string FileNameFormat = "{0}.ocmd";
	
	private string gameSystem;
	public string CurrentGameSystem
	{
		get { return gameSystem; }
		set
		{
			gameSystem = value;
			EmitSignal(nameof(GameSystemChanged), gameSystem);
		}
	}
	
	public IMetadataContainer Container { get; private set; }
	
	public override void _Ready()
	{
		CurrentGameSystem = String.Empty;
		GetNode<TabContainer>(NodePathBuilder.SceneUnique(AppRoot.SheetTabsName, Constants.NodePath.AppRoot)).Connect(Constants.Signal.TabSelected, this, nameof(sheetTabSelected));
	}
	
	private void sheetTabSelected(int tabIndex)
	{
		var tab = GetNode<TabContainer>(NodePathBuilder.SceneUnique(AppRoot.SheetTabsName, Constants.NodePath.AppRoot)).GetTabControl(tabIndex);
		if(tab is ChangelingSheet)
		{
			CurrentGameSystem = GameSystem.CoD.Changeling;
			Container = new CoDChangelingContainer();
		}
		else if (tab is MortalSheet)
		{
			CurrentGameSystem = GameSystem.CoD.Mortal;
			Container = new CoDCoreContainer();
		}
		else if (tab is DndFifthSheet)
		{
			CurrentGameSystem = GameSystem.DnD.Fifth;
			Container = new DnDFifthContainer();
		}
		else
		{
			CurrentGameSystem = String.Empty;
		}
		//TODO: We shouldn't auto-load here
		loadGameSystemMetadata();
		GD.Print(Container);
	}
	
	public void loadGameSystemMetadata()
	{
		var filename = String.Format(FileNameFormat, CurrentGameSystem);
		var path = System.IO.Path.GetFullPath(FileSystemUtilities.DefaultMetadataDirectory + filename);
		var json = FileSystemUtilities.ReadString(path);
		if(!String.IsNullOrEmpty(json) && Container is IMetadataContainer)
		{
			Container.Deserialize(json);
			EmitSignal(nameof(MetadataLoaded), new SignalPayload<IMetadataContainer>(Container));
		}
	}
	
	public void saveGameSystemMetadata()
	{
		if(Container is IMetadataContainer)
		{
			var metadata = Container.Serialize();
			var filename = String.Format(FileNameFormat, CurrentGameSystem);
			var path = System.IO.Path.GetFullPath(FileSystemUtilities.DefaultMetadataDirectory + filename);
			
			FileSystemUtilities.WriteString(path, metadata);
			EmitSignal(nameof(MetadataSaved));
		}
	}
	
	public void initializeGameSystems()
	{
		gameSystem = GameSystem.CoD.Changeling;
		loadGameSystemMetadata();
		if(!(Container is IMetadataContainer) || Container.IsEmpty())
		{
			Container = CoDChangelingContainer.initializeWithDefaultValues();
			saveGameSystemMetadata();
		}
	}
}
