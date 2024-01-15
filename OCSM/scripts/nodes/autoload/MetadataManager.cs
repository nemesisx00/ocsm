using Godot;
using Ocsm.Meta;
using Ocsm.Cofd.Meta;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Nodes;
using Ocsm.Cofd.Nodes;
using Ocsm.Cofd.Ctl.Nodes;

namespace Ocsm.Nodes.Autoload;

public partial class MetadataManager : Node
{
	[Signal]
	public delegate void GameSystemChangedEventHandler(int gameSystem);
	[Signal]
	public delegate void MetadataLoadedEventHandler();
	[Signal]
	public delegate void MetadataSavedEventHandler();
	
	public static readonly NodePath NodePath = new("/root/MetadataManager");
	private const string FileNameFormat = $"{{0}}{AppConstants.MetadataFileExtension}";
	
	public GameSystems CurrentGameSystem
	{
		get { return gameSystem; }
		set
		{
			if(gameSystem == GameSystems.None || !gameSystem.Equals(value))
			{
				gameSystem = value;
				_ = EmitSignal(SignalName.GameSystemChanged, (int)gameSystem);
				
				switch(gameSystem)
				{
					case GameSystems.CofdChangeling:
						Container = new CofdChangelingContainer();
						LoadGameSystemMetadata();
						break;
					
					case GameSystems.CofdMortal:
						Container = new CofdCoreContainer();
						LoadGameSystemMetadata();
						break;
					
					case GameSystems.Dnd5e:
						Container = new DndFifthContainer();
						LoadGameSystemMetadata();
						break;
					
					default:
						Container = null;
						break;
				}
			}
		}
	}
	
	public IMetadataContainer Container { get; private set; }
	
	private GameSystems gameSystem;	
	private TabContainer sheetTabs;
	
	public override void _Ready()
	{
		sheetTabs = GetNode<TabContainer>(AppRoot.NodePaths.SheetTabs);
		
		CurrentGameSystem = GameSystems.None;
		sheetTabs.TabSelected += sheetTabSelected;
	}
	
	private void sheetTabSelected(long tabIndex)
	{
		var tab = sheetTabs.GetTabControl((int)tabIndex);
		if(tab is ChangelingSheet)
			CurrentGameSystem = GameSystems.CofdChangeling;
		else if (tab is MortalSheet)
			CurrentGameSystem = GameSystems.CofdMortal;
		else if (tab is DndFifthSheet)
			CurrentGameSystem = GameSystems.Dnd5e;
		else
			CurrentGameSystem = GameSystems.None;
	}
	
	public void LoadGameSystemMetadata()
	{
		if(CurrentGameSystem != GameSystems.None)
		{
			var filename = string.Format(FileNameFormat, CurrentGameSystem.ToString());
			var path = System.IO.Path.GetFullPath(FileSystemUtilities.DefaultMetadataDirectory + filename);
			var json = FileSystemUtilities.ReadString(path);
			
			if(!string.IsNullOrEmpty(json) && Container is not null)
			{
				Container.Deserialize(json);
				_ = EmitSignal(SignalName.MetadataLoaded);
			}
		}
	}
	
	public void SaveGameSystemMetadata()
	{
		if(Container is not null)
		{
			var metadata = Container.Serialize();
			var filename = string.Format(FileNameFormat, CurrentGameSystem);
			var path = System.IO.Path.GetFullPath(FileSystemUtilities.DefaultMetadataDirectory + filename);
			
			FileSystemUtilities.WriteString(path, metadata);
			_ = EmitSignal(SignalName.MetadataSaved);
		}
	}
	
	public void InitializeGameSystems()
	{
		CurrentGameSystem = GameSystems.CofdChangeling;
		
		if(Container.IsEmpty())
		{
			Container = CofdChangelingContainer.InitializeWithDefaultValues();
			SaveGameSystemMetadata();
		}
		
		gameSystem = GameSystems.None;
		Container = null;
	}
}
