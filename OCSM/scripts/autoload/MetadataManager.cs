using Godot;
using System;
using Ocsm.Meta;
using Ocsm.Cofd.Meta;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes.Cofd.Sheets;
using Ocsm.Nodes.Dnd.Sheets;

namespace Ocsm.Nodes.Autoload;

public partial class MetadataManager : Node
{
	[Signal]
	public delegate void GameSystemChangedEventHandler(string gameSystem);
	[Signal]
	public delegate void MetadataLoadedEventHandler();
	[Signal]
	public delegate void MetadataSavedEventHandler();
	
	private const string FileNameFormat = "{0}" + Constants.MetadataFileExtension;
	
	private string gameSystem = String.Empty;
	
	public string CurrentGameSystem
	{
		get { return gameSystem; }
		set
		{
			if(!gameSystem.Equals(value))
			{
				gameSystem = value;
				EmitSignal(nameof(GameSystemChanged), gameSystem);
				
				switch(gameSystem)
				{
					case Constants.GameSystem.Cofd.Changeling:
						Container = new CofdChangelingContainer();
						loadGameSystemMetadata();
						break;
					case Constants.GameSystem.Cofd.Mortal:
						Container = new CofdCoreContainer();
						loadGameSystemMetadata();
						break;
					case Constants.GameSystem.Dnd.Fifth:
						Container = new DndFifthContainer();
						loadGameSystemMetadata();
						break;
					default:
						Container = null;
						break;
				}
			}
		}
	}
	
	public IMetadataContainer Container { get; private set; }
	
	private TabContainer sheetTabs;
	
	public override void _Ready()
	{
		sheetTabs = GetNode<TabContainer>(AppRoot.NodePath.SheetTabs);
		
		CurrentGameSystem = String.Empty;
		sheetTabs.TabSelected += sheetTabSelected;
	}
	
	private void sheetTabSelected(long tabIndex)
	{
		var tab = sheetTabs.GetTabControl((int)tabIndex);
		if(tab is ChangelingSheet)
			CurrentGameSystem = Constants.GameSystem.Cofd.Changeling;
		else if (tab is MortalSheet)
			CurrentGameSystem = Constants.GameSystem.Cofd.Mortal;
		else if (tab is DndFifthSheet)
			CurrentGameSystem = Constants.GameSystem.Dnd.Fifth;
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
			if(!String.IsNullOrEmpty(json) && Container is IMetadataContainer)
			{
				Container.Deserialize(json);
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
		CurrentGameSystem = Constants.GameSystem.Cofd.Changeling;
		if(Container.IsEmpty())
		{
			Container = CofdChangelingContainer.initializeWithDefaultValues();
			saveGameSystemMetadata();
		}
		
		gameSystem = String.Empty;
		Container = null;
	}
}
