using Godot;
using Ocsm.Meta;
using System;
using System.Linq;
using System.Reflection;

namespace Ocsm.Nodes.Autoload;

public partial class MetadataManager : Node
{
	[Signal]
	public delegate void GameSystemChangedEventHandler(Transport<GameSystem> gameSystemId);
	[Signal]
	public delegate void MetadataLoadedEventHandler();
	[Signal]
	public delegate void MetadataSavedEventHandler();
	
	public const string TypeName_GameSystemFactory = "GameSystemFactory";
	public static readonly NodePath NodePath = new("/root/MetadataManager");
	
	private const string NamespaceToRemove = ".Nodes";
	private const string FieldName_GameSystemFactory_Name = "Name";
	private const string MethodName_Container_InitializeWithDefaultValues = "InitializeWithDefaultValues";
	
	public GameSystemRegistry Registry { get; private set; } = new();
	
	private GameSystem gameSystem;
	
	public GameSystem CurrentGameSystem
	{
		get { return gameSystem; }
		set
		{
			if(gameSystem != value)
			{
				gameSystem = value;
				EmitSignal(SignalName.GameSystemChanged, new Transport<GameSystem>(gameSystem));
				
				if(gameSystem is not null)
				{
					Container = (IMetadataContainer)Activator.CreateInstance(gameSystem.MetadataContainerType);
					if(Container is not null)
						LoadGameSystemMetadata();
				}
				else
					Container = null;
			}
		}
	}
	
	public IMetadataContainer Container { get; private set; }
	
	private TabContainer sheetTabs;
	
	public override void _Ready()
	{
		sheetTabs = GetNode<TabContainer>(AppRoot.NodePaths.SheetTabs);
		sheetTabs.TabSelected += sheetTabSelected;
	}
	
	private void sheetTabSelected(long tabIndex)
	{
		var tab = sheetTabs.GetTabControl((int)tabIndex);
		
		if(tab is not null)
		{
			var ns = tab.GetType()
				.Namespace
				.Replace(NamespaceToRemove, string.Empty);
			
			//Find the GameSystemFactory type based on the given namespace
			var factory = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(t => t.GetTypes())
				.Where(t => t.IsClass && t.Namespace == ns && t.Name == TypeName_GameSystemFactory)
				.FirstOrDefault();
			
			if(factory is not null)
				CurrentGameSystem = Registry.GetGameSystem(
				(string)factory.GetField(
						FieldName_GameSystemFactory_Name,
						BindingFlags.Public | BindingFlags.Static
					)
					.GetRawConstantValue()
				);
			else
				CurrentGameSystem = null;
		}
		
		if(Container is not null && Container.IsEmpty())
		{
			Container.GetType()
				.GetMethod(
					MethodName_Container_InitializeWithDefaultValues,
					BindingFlags.Public | BindingFlags.Static
				)?
				.Invoke(null, null);
		}
	}
	
	public void LoadGameSystemMetadata()
	{
		if(CurrentGameSystem is not null)
		{
			var path = System.IO.Path.GetFullPath($"{FileSystemUtilities.DefaultMetadataDirectory}{CurrentGameSystem}{Constants.MetadataFileExtension}");
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
			var path = System.IO.Path.GetFullPath($"{FileSystemUtilities.DefaultMetadataDirectory}{CurrentGameSystem}{Constants.MetadataFileExtension}");
			FileSystemUtilities.WriteString(path, metadata);
			EmitSignal(SignalName.MetadataSaved);
		}
	}
}
