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
	public delegate void MetadataLoaded();
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
			
			switch(gameSystem)
			{
				case GameSystem.CoD.Changeling:
					Container = new CoDChangelingContainer();
					loadGameSystemMetadata();
					break;
				case GameSystem.CoD.Mortal:
					Container = new CoDCoreContainer();
					loadGameSystemMetadata();
					break;
				case GameSystem.DnD.Fifth:
					Container = new DnDFifthContainer();
					loadGameSystemMetadata();
					break;
				default:
					Container = null;
					break;
			}
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
			CurrentGameSystem = GameSystem.CoD.Changeling;
		else if (tab is MortalSheet)
			CurrentGameSystem = GameSystem.CoD.Mortal;
		else if (tab is DndFifthSheet)
			CurrentGameSystem = GameSystem.DnD.Fifth;
		else
			CurrentGameSystem = String.Empty;
	}
	
	public void loadGameSystemMetadata()
	{
		if(!String.IsNullOrEmpty(CurrentGameSystem))
		{
			var filename = String.Format(FileNameFormat, CurrentGameSystem);
			var path = System.IO.Path.GetFullPath(FileSystemUtilities.DefaultMetadataDirectory + filename);
			var json = FileSystemUtilities.ReadString(path);
			GD.Print("json length: ", json.Length);
			if(!String.IsNullOrEmpty(json) && Container is IMetadataContainer)
			{
				Container.Deserialize(json);
				GD.Print("Is container empty after load? ", Container.IsEmpty());
				EmitSignal(nameof(MetadataLoaded));
			}
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
		CurrentGameSystem = GameSystem.CoD.Changeling;
		if(Container.IsEmpty())
		{
			GD.Print("Should initialize Changeling.");
			Container = CoDChangelingContainer.initializeWithDefaultValues();
			saveGameSystemMetadata();
		}
		
		gameSystem = String.Empty;
		Container = null;
	}
}
