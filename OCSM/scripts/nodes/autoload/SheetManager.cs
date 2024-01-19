using Godot;

namespace Ocsm.Nodes.Autoload;

public partial class SheetManager : Node
{
	public static readonly NodePath NodePath = new("/root/SheetManager");
	
	private MetadataManager metadataManager;
	private TabContainer sheetTabs;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		sheetTabs = GetNode<TabContainer>(AppRoot.NodePaths.SheetTabs);
	}
	
	public void AddNewSheet(string scenePath, string name, string json = null)
	{
		if(!string.IsNullOrEmpty(scenePath) && !string.IsNullOrEmpty(name))
		{
			var resource = GD.Load<PackedScene>(scenePath);
			var instance = resource.Instantiate();
			instance.Name = name;
			
			var target = GetNode<TabContainer>(AppRoot.NodePaths.SheetTabs);
			if(target is TabContainer tc)
			{
				var dupeCount = 0;
				foreach(var c in tc.GetChildren())
				{
					if(c.Name.ToString().Contains(instance.Name))
						dupeCount++;
				}
				
				if(dupeCount > 0)
					instance.Name = $"{instance.Name} ({dupeCount})";
				
				if(!string.IsNullOrEmpty(json) && instance is ICharacterSheet sheet)
					sheet.SetJsonData(json);
				
				tc.AddChild(instance);
				tc.CurrentTab = tc.GetTabCount() - 1;
			}
		}
	}
	
	public void CloseActiveSheet()
	{
		if(sheetTabs is not null)
		{
			var tab = sheetTabs.GetCurrentTabControl();
			
			if(tab is not null)
			{
				if(sheetTabs.GetTabCount() <= 1)
					ShowNewSheetUI();
				
				tab.QueueFree();
			}
		}
	}
	
	public string GetActiveSheetJsonData()
	{
		string data = null;
		if(sheetTabs is not null)
		{
			var tab = sheetTabs.GetCurrentTabControl();
			
			if(tab is ICharacterSheet sheet)
				data = sheet.GetJsonData();
		}
		return data;
	}
	
	public void HideNewSheetUI()
	{
		if(sheetTabs is not null && !sheetTabs.Visible)
			sheetTabs.Show();
		
		if(GetNodeOrNull<Control>(AppRoot.NodePaths.NewSheet) is Control newSheet)
			newSheet.QueueFree();
	}
	
	public void LoadSheetJsonData(string json)
	{
		if(!string.IsNullOrEmpty(json))
		{
			if(sheetTabs is not null)
			{
				var loaded = false;
				if(json.Contains(GameSystems.CofdChangeling.ToString()))
				{
					metadataManager.CurrentGameSystem = GameSystems.CofdChangeling;
					AddNewSheet(ScenePaths.Cofd.Changeling.Sheet, ScenePaths.Cofd.Changeling.NewSheetName, json);
					loaded = true;
				}
				else if(json.Contains(GameSystems.CofdMortal.ToString()))
				{
					metadataManager.CurrentGameSystem = GameSystems.CofdMortal;
					AddNewSheet(ScenePaths.Cofd.Mortal.Sheet, ScenePaths.Cofd.Mortal.NewSheetName, json);
					loaded = true;
				}
				else if(json.Contains(GameSystems.Dnd5e.ToString()))
				{
					metadataManager.CurrentGameSystem = GameSystems.Dnd5e;
					AddNewSheet(ScenePaths.Dnd.Fifth.Sheet, ScenePaths.Dnd.Fifth.NewSheetName, json);
					loaded = true;
				}
				else if(json.Contains(GameSystems.WodVtmV5.ToString()))
				{
					metadataManager.CurrentGameSystem = GameSystems.WodVtmV5;
					AddNewSheet(ScenePaths.Wod.V5.Sheet, ScenePaths.Wod.V5.NewSheetName, json);
					loaded = true;
				}
				
				if(loaded)
					HideNewSheetUI();
			}
		}
	}
	
	public void ShowNewSheetUI()
	{
		var existingNode = GetNodeOrNull<NewSheet>(AppRoot.NodePaths.NewSheet);
		if(existingNode is null)
		{
			sheetTabs.Hide();
			
			var resource = GD.Load<PackedScene>(ScenePaths.NewSheet);
			var instance = resource.Instantiate<NewSheet>();
			instance.UniqueNameInOwner = true;
			GetNode<Control>(AppRoot.NodePaths.Self).AddChild(instance);
			
			GetNode<MetadataManager>(MetadataManager.NodePath).CurrentGameSystem = GameSystems.None;
		}
	}
}
