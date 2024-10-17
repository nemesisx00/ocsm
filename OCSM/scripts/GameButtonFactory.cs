using Godot;
using Ocsm.Nodes;

namespace Ocsm;

public static class AddSheetButtonFactory
{
	public static AddSheet GenerateButton(string label, string edition, string scenePath, string name, string iconPath = "", string disabledIconPath = "")
	{
		AddSheet node = null;
		
		var scene = GD.Load<PackedScene>(ScenePaths.AddSheet);
		if(scene.CanInstantiate())
		{
			var icon = string.IsNullOrEmpty(iconPath)
				? null
				: GD.Load<Texture2D>(iconPath);
			
			var disabledIcon = string.IsNullOrEmpty(disabledIconPath)
				? null
				: GD.Load<Texture2D>(disabledIconPath);
			
			node = scene.Instantiate<AddSheet>();
			node.Edition = edition;
			node.GameLabel = label;
			node.SheetName = name;
			node.SheetPath = scenePath;
			node.TextureNormal = icon;
			node.TextureDisabled = disabledIcon;
		}
		
		return node;
	}
}
