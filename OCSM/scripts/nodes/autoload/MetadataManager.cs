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
	public delegate void GameSystemChangedEventHandler(string gameSystem);
	[Signal]
	public delegate void MetadataLoadedEventHandler();
	[Signal]
	public delegate void MetadataSavedEventHandler();
	
	public static readonly NodePath NodePath = new("/root/MetadataManager");
	private const string FileNameFormat = $"{{0}}{Constants.MetadataFileExtension}";
	
	public string CurrentGameSystem
	{
		get { return gameSystem; }
		set
		{
			if(gameSystem is null || !gameSystem.Equals(value))
			{
				gameSystem = value;
				_ = EmitSignal(SignalName.GameSystemChanged, gameSystem);
				
				switch(gameSystem)
				{
					case Constants.GameSystem.Cofd.Changeling:
						Container = new CofdChangelingContainer();
						LoadGameSystemMetadata();
						break;
					
					case Constants.GameSystem.Cofd.Mortal:
						Container = new CofdCoreContainer();
						LoadGameSystemMetadata();
						break;
					
					case Constants.GameSystem.Dnd.Fifth:
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
	
	private string gameSystem;	
	private TabContainer sheetTabs;
	
	public override void _Ready()
	{
		sheetTabs = GetNode<TabContainer>(AppRoot.NodePaths.SheetTabs);
		
		CurrentGameSystem = string.Empty;
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
			CurrentGameSystem = string.Empty;
	}
	
	public void LoadGameSystemMetadata()
	{
		if(!string.IsNullOrEmpty(CurrentGameSystem))
		{
			var filename = string.Format(FileNameFormat, CurrentGameSystem);
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
		CurrentGameSystem = Constants.GameSystem.Cofd.Changeling;
		
		if(Container.IsEmpty())
		{
			Container = CofdChangelingContainer.InitializeWithDefaultValues();
			SaveGameSystemMetadata();
		}
		
		gameSystem = string.Empty;
		Container = null;
	}
}
