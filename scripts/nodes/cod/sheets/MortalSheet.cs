using Godot;
using System;
using System.Text.Json;
using OCSM;

public class MortalSheet : CoreSheet<Mortal>, ICharacterSheet
{
	private sealed new class Advantage : CoreSheet<Mortal>.Advantage
	{
		public const string Integrity = "Integrity";
		public const string Vice = "Vice";
		public const string Virtue = "Virtue";
	}
	
	private sealed new class Detail : CoreSheet<Mortal>.Detail
	{
		public const string Age = "Age";
		public const string Faction = "Faction";
		public const string GroupName = "GroupName";
		public const string Vice = "Vice";
		public const string Virtue = "Virtue";
	}
	
	public override void _Ready()
	{
		SheetData = new Mortal();
		
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Integrity, AdvantagesPath)), SheetData.Integrity.ToString(), nameof(changed_Integrity));
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = SheetData.Vice;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = SheetData.Virtue;
		
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Age, DetailsPath)), SheetData.Age > -1 ? SheetData.Age.ToString() : String.Empty, nameof(changed_Age));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Faction, DetailsPath)), SheetData.Faction, nameof(changed_Faction));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.GroupName, DetailsPath)), SheetData.GroupName, nameof(changed_GroupName));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Vice, DetailsPath)), SheetData.Vice, nameof(changed_Vice));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Virtue, DetailsPath)), SheetData.Virtue, nameof(changed_Virtue));
		
		base._Ready();
	}
	
	private void changed_Age(string newText)
	{
		int newAge;
		if(int.TryParse(newText, out newAge))
			SheetData.Age = newAge;
		else
			GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Age, DetailsPath)).Text = SheetData.Age.ToString();
	}
	
	private void changed_Faction(string newText) { SheetData.Faction = newText; }
	private void changed_GroupName(string newText) { SheetData.GroupName = newText; }
	private void changed_Integrity(int value) { SheetData.Integrity = value; GD.Print(JsonSerializer.Serialize(SheetData)); }
	
	private void changed_Vice(string newText)
	{
		SheetData.Vice = newText;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = SheetData.Vice;
	}
	
	private void changed_Virtue(string newText)
	{
		SheetData.Virtue = newText;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = SheetData.Virtue;
	}
}
