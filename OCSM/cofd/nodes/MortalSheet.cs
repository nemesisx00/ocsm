using Godot;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Nodes;

public partial class MortalSheet : CoreSheet<Mortal>, ICharacterSheet
{
	private static new class NodePaths
	{
		public static readonly NodePath Integrity = new("%Advantages/%Integrity");
		public static readonly NodePath Age = new("%Details/%Age");
		public static readonly NodePath Faction = new("%Details/%Faction");
		public static readonly NodePath GroupName = new("%Details/%GroupName");
		public static readonly NodePath Vice = new("%Details/%Vice");
		public static readonly NodePath Virtue = new("%Details/%Virtue");
	}
	
	private DynamicNumericLabel age;
	private DynamicTextLabel faction;
	private DynamicTextLabel groupName;
	private TrackSimple integrity;
	private DynamicTextLabel vice;
	private DynamicTextLabel virtue;
	
	public override void _ExitTree()
	{
		age.ValueChanged -= changed_Age;
		faction.TextChanged -= changed_Faction;
		groupName.TextChanged -= changed_GroupName;
		integrity.ValueChanged -= changed_Integrity;
		vice.TextChanged -= changed_Vice;
		virtue.TextChanged -= changed_Virtue;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		SheetData ??= new Mortal();
		
		age = GetNode<DynamicNumericLabel>(NodePaths.Age);
		faction = GetNode<DynamicTextLabel>(NodePaths.Faction);
		groupName = GetNode<DynamicTextLabel>(NodePaths.GroupName);
		integrity = GetNode<TrackSimple>(NodePaths.Integrity);
		vice = GetNode<DynamicTextLabel>(NodePaths.Vice);
		virtue = GetNode<DynamicTextLabel>(NodePaths.Virtue);
		
		InitTrackSimple(integrity, SheetData.Advantages.Integrity, changed_Integrity, DefaultIntegrityMax);
		InitDynamicNumericLabel(age, SheetData.Age, changed_Age);
		InitDynamicTextLabel(faction, SheetData.Details.Faction, changed_Faction);
		InitDynamicTextLabel(groupName, SheetData.Details.TypePrimary, changed_GroupName);
		InitDynamicTextLabel(vice, SheetData.Details.Vice, changed_Vice);
		InitDynamicTextLabel(virtue, SheetData.Details.Virtue, changed_Virtue);
		
		base._Ready();
	}
	
	private void changed_Age(double number) => SheetData.Age = (int)number;
	private void changed_Faction(string newText) => SheetData.Details.Faction = newText;
	private void changed_GroupName(string newText) => SheetData.Details.TypePrimary = newText;
	private void changed_Integrity(int value) => SheetData.Advantages.Integrity = value;
	private void changed_Vice(string newText) => SheetData.Details.Vice = newText;
	private void changed_Virtue(string newText) => SheetData.Details.Virtue = newText;
}
