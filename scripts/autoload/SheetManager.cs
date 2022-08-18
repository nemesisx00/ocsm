using Godot;
using System;

public class SheetManager : Node
{
	public void showNewSheetUI()
	{
		var existingNode = GetNodeOrNull<NewSheet>(Constants.NodePath.NewSheet);
		if(!(existingNode is NewSheet))
		{
			var sheetTabsNode = GetNode<TabContainer>(Constants.NodePath.SheetTabs);
			sheetTabsNode.Hide();
			
			var resource = GD.Load<PackedScene>(Constants.Scene.NewSheet);
			var instance = resource.Instance<NewSheet>();
			GetNode<Control>(Constants.NodePath.AppRoot).AddChild(instance);
		}
	}
	
	public void hideNewSheetUI()
	{
		GetNode<Control>(Constants.NodePath.SheetTabs).Show();
		GetNode<Control>(Constants.NodePath.NewSheet).QueueFree();
	}
	
	public void addNewSheet(string scenePath, string name)
	{
		if(!String.IsNullOrEmpty(scenePath) && !String.IsNullOrEmpty(name))
		{
			var resource = GD.Load<PackedScene>(scenePath);
			var instance = resource.Instance();
			instance.Name = name;
			
			var target = GetNode(Constants.NodePath.SheetTabs);
			if(target is TabContainer tc)
			{
				var dupeCount = 0;
				foreach(Node c in tc.GetChildren())
				{
					if(c.Name.Contains(instance.Name))
						dupeCount++;
				}
				
				if(dupeCount > 0)
					instance.Name = String.Format("{0} ({1})", instance.Name, dupeCount);
				
				tc.AddChild(instance);
			}
		}
	}
}
