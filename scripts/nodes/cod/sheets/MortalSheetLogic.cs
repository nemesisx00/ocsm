using Godot;
using System;
using System.Text.Json;
using OCSM;

public class MortalSheetLogic : CoreSheetLogic<Mortal>
{
	private sealed new class Advantage : CoreSheetLogic<Mortal>.Advantage
	{
		public const string Integrity = "Integrity";
		public const string Vice = "Vice";
		public const string Virtue = "Virtue";
	}
	
	private sealed new class Detail : CoreSheetLogic<Mortal>.Detail
	{
		public const string Age = "Age";
		public const string Faction = "Faction";
		public const string GroupName = "GroupName";
		public const string Vice = "Vice";
		public const string Virtue = "Virtue";
	}
	
	public override void _Ready()
	{
		sheetData = new Mortal();
		
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Integrity, AdvantagesPath)), sheetData.Integrity.ToString(), nameof(changed_Integrity));
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = sheetData.Vice;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = sheetData.Virtue;
		
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Age, DetailsPath)), sheetData.Age > -1 ? sheetData.Age.ToString() : String.Empty, nameof(changed_Age));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Faction, DetailsPath)), sheetData.Faction, nameof(changed_Faction));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.GroupName, DetailsPath)), sheetData.GroupName, nameof(changed_GroupName));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Vice, DetailsPath)), sheetData.Vice, nameof(changed_Vice));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Virtue, DetailsPath)), sheetData.Virtue, nameof(changed_Virtue));
		
		base._Ready();
	}
	
	private void changed_Age(string newText)
	{
		int newAge;
		if(int.TryParse(newText, out newAge))
			sheetData.Age = newAge;
		else
			GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Age, DetailsPath)).Text = sheetData.Age.ToString();
	}
	
	private void changed_Faction(string newText) { sheetData.Faction = newText; }
	private void changed_GroupName(string newText) { sheetData.GroupName = newText; }
	private void changed_Integrity(int value) { sheetData.Integrity = value; GD.Print(JsonSerializer.Serialize(sheetData)); }
	
	private void changed_Vice(string newText)
	{
		sheetData.Vice = newText;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = sheetData.Vice;
	}
	
	private void changed_Virtue(string newText)
	{
		sheetData.Virtue = newText;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = sheetData.Virtue;
	}
}
