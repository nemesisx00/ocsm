using Godot;
using System;
using Ocsm.Nodes.Sheets;

namespace Ocsm.Nodes.Autoload
{
	public partial class SheetManager : Node
	{
		private MetadataManager metadataManager;
		private TabContainer sheetTabs;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			sheetTabs = GetNode<TabContainer>(AppRoot.NodePath.SheetTabs);
		}
		
		public void addNewSheet(string scenePath, string name, string json = null)
		{
			if(!String.IsNullOrEmpty(scenePath) && !String.IsNullOrEmpty(name))
			{
				var resource = GD.Load<PackedScene>(scenePath);
				var instance = resource.Instantiate();
				instance.Name = name;
				
				var target = GetNode<TabContainer>(AppRoot.NodePath.SheetTabs);
				if(target is TabContainer tc)
				{
					var dupeCount = 0;
					foreach(Node c in tc.GetChildren())
					{
						if(c.Name.ToString().Contains(instance.Name))
							dupeCount++;
					}
					
					if(dupeCount > 0)
						instance.Name = String.Format("{0} ({1})", instance.Name, dupeCount);
					
					if(!String.IsNullOrEmpty(json) && instance is ICharacterSheet sheet)
						sheet.SetJsonData(json);
					
					tc.AddChild(instance);
					tc.CurrentTab = tc.GetTabCount() - 1;
				}
			}
		}
		
		public void closeActiveSheet()
		{
			if(sheetTabs is TabContainer)
			{
				var tab = sheetTabs.GetCurrentTabControl();
				if(tab is Node)
				{
					if(sheetTabs.GetTabCount() <= 1)
						showNewSheetUI();
					tab.QueueFree();
				}
			}
		}
		
		public string getActiveSheetJsonData()
		{
			string data = null;
			if(sheetTabs is TabContainer)
			{
				var tab = sheetTabs.GetCurrentTabControl();
				if(tab is ICharacterSheet sheet)
				{
					data = sheet.GetJsonData();
				}
			}
			return data;
		}
		
		public void hideNewSheetUI()
		{
			if(sheetTabs is Control && !sheetTabs.Visible)
				sheetTabs.Show();
			if(GetNodeOrNull<Control>(AppRoot.NodePath.NewSheet) is Control newSheet)
				newSheet.QueueFree();
		}
		
		public void loadSheetJsonData(string json)
		{
			if(!String.IsNullOrEmpty(json))
			{
				if(sheetTabs is TabContainer)
				{
					var loaded = false;
					if(json.Contains(Constants.GameSystem.Cofd.Changeling))
					{
						metadataManager.CurrentGameSystem = Constants.GameSystem.Cofd.Changeling;
						addNewSheet(Constants.Scene.Cofd.Changeling.Sheet, Constants.Scene.Cofd.Changeling.NewSheetName, json);
						loaded = true;
					}
					else if(json.Contains(Constants.GameSystem.Cofd.Mortal))
					{
						metadataManager.CurrentGameSystem = Constants.GameSystem.Cofd.Mortal;
						addNewSheet(Constants.Scene.Cofd.Mortal.Sheet, Constants.Scene.Cofd.Mortal.NewSheetName, json);
						loaded = true;
					}
					else if(json.Contains(Constants.GameSystem.Dnd.Fifth))
					{
						metadataManager.CurrentGameSystem = Constants.GameSystem.Dnd.Fifth;
						addNewSheet(Constants.Scene.Dnd.Fifth.Sheet, Constants.Scene.Dnd.Fifth.NewSheetName, json);
						loaded = true;
					}
					
					if(loaded)
						hideNewSheetUI();
				}
			}
		}
		
		public void showNewSheetUI()
		{
			var existingNode = GetNodeOrNull<NewSheet>(AppRoot.NodePath.NewSheet);
			if(!(existingNode is NewSheet))
			{
				sheetTabs.Hide();
				
				var resource = GD.Load<PackedScene>(Constants.Scene.NewSheet);
				var instance = resource.Instantiate<NewSheet>();
				instance.UniqueNameInOwner = true;
				GetNode<Control>(Constants.NodePath.AppRoot).AddChild(instance);
				
				GetNode<MetadataManager>(Constants.NodePath.MetadataManager).CurrentGameSystem = String.Empty;
			}
		}
	}
}
