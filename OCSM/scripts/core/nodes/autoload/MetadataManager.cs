using Godot;
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
	public delegate void GameSystemChangedEventHandler(GameSystem gameSystem);
	[Signal]
	public delegate void MetadataLoadedEventHandler();
	[Signal]
	public delegate void MetadataSavedEventHandler();
	
	private const string FileNameFormat = "{0}" + Constants.MetadataFileExtension;
	
	private GameSystem gameSystem;
	
	public GameSystem CurrentGameSystem
	{
		get { return gameSystem; }
		set
		{
			if(gameSystem != value)
			{
				gameSystem = value;
				EmitSignal(SignalName.GameSystemChanged, (int)gameSystem);
				
				Container = gameSystem switch
				{
					GameSystem.CofdChangeling => new CofdChangelingContainer(),
					GameSystem.CofdMortal => new CofdCoreContainer(),
					GameSystem.Dnd5e => new DndFifthContainer(),
					_ => null,
				};
				
				if(Container is not null)
					LoadGameSystemMetadata();
			}
		}
	}
	
	public IMetadataContainer Container { get; private set; }
	
	private TabContainer sheetTabs;
	
	public override void _Ready()
	{
		sheetTabs = GetNode<TabContainer>(AppRoot.NodePaths.SheetTabs);
		
		CurrentGameSystem = GameSystem.None;
		sheetTabs.TabSelected += sheetTabSelected;
	}
	
	private void sheetTabSelected(long tabIndex)
	{
		var tab = sheetTabs.GetTabControl((int)tabIndex);
		if(tab is ChangelingSheet)
			CurrentGameSystem = GameSystem.CofdChangeling;
		else if (tab is MortalSheet)
			CurrentGameSystem = GameSystem.CofdMortal;
		else if (tab is DndFifthSheet)
			CurrentGameSystem = GameSystem.Dnd5e;
		else
			CurrentGameSystem = GameSystem.None;
	}
	
	public void LoadGameSystemMetadata()
	{
		if(CurrentGameSystem != GameSystem.None)
		{
			var filename = string.Format(FileNameFormat, CurrentGameSystem.ToString());
			var path = System.IO.Path.GetFullPath(FileSystemUtilities.DefaultMetadataDirectory + filename);
			var json = FileSystemUtilities.ReadString(path);
			if(!string.IsNullOrEmpty(json) && Container is not null)
			{
				Container.Deserialize(json);
				EmitSignal(SignalName.MetadataLoaded);
			}
		}
	}
	
	public void SaveGameSystemMetadata()
	{
		if(Container is not null)
		{
			var metadata = Container.Serialize();
			var filename = string.Format(FileNameFormat, CurrentGameSystem.ToString());
			var path = System.IO.Path.GetFullPath(FileSystemUtilities.DefaultMetadataDirectory + filename);
			
			FileSystemUtilities.WriteString(path, metadata);
			EmitSignal(SignalName.MetadataSaved);
		}
	}
	
	public void InitializeGameSystems()
	{
		CurrentGameSystem = GameSystem.CofdChangeling;
		if(Container.IsEmpty())
		{
			Container = CofdChangelingContainer.InitializeWithDefaultValues();
			SaveGameSystemMetadata();
		}
		
		gameSystem = GameSystem.None;
		Container = null;
	}
}
