using System.Text.RegularExpressions;
using Godot;
using Ocsm.Meta;

namespace Ocsm.Nodes.Autoload;

public partial class SheetManager : Node
{
	public static readonly NodePath NodePath = new("/root/SheetManager");
	
	[GeneratedRegex(".*\"GameSystem\":\"(.*?)\".*")]
	private static partial Regex JsonGameSystemRegex();
	
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
				if(!string.IsNullOrEmpty(json) && instance is ICharacterSheet sheet)
				{
					sheet.SetJsonData(json);
					
					if(!string.IsNullOrEmpty(sheet.CharacterName))
						instance.Name = sheet.CharacterName;
				}
				
				var dupeCount = 0;
				foreach(Node c in tc.GetChildren())
				{
					if(c.Name.ToString().Contains(instance.Name))
						dupeCount++;
				}
				
				if(dupeCount > 0)
					instance.Name = $"{instance.Name} ({dupeCount})";
				
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
			{
				data = sheet.GetJsonData();
			}
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
				
				if(JsonGameSystemRegex().Match(json) is Match match
						&& match.Groups.Count > 0 && !string.IsNullOrEmpty(match.Groups[1].Value)
						&& metadataManager.Registry.GetGameSystem(match.Groups[1].Value) is GameSystem gs)
				{
					metadataManager.CurrentGameSystem = gs;
					
					AddNewSheet(
						gs.SheetResourcePath,
						gs.SheetDefaultName,
						json
					);
					
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
			
			GetNode<MetadataManager>(MetadataManager.NodePath).CurrentGameSystem = null;
		}
	}
}
