using Godot;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Nodes;

public partial class MortalSheet : CoreSheet<Mortal>, ICharacterSheet
{
	private sealed new class NodePaths
	{
		public static readonly NodePath Integrity = new("%Advantages/%Integrity");
		public static readonly NodePath Age = new("%Details/%Age");
		public static readonly NodePath Faction = new("%Details/%Faction");
		public static readonly NodePath GroupName = new("%Details/%GroupName");
		public static readonly NodePath Vice = new("%Details/%Vice");
		public static readonly NodePath ViceLabel = new("%Advantages/%ViceLabel");
		public static readonly NodePath Virtue = new("%Details/%Virtue");
		public static readonly NodePath VirtueLabel = new("%Advantages/%VirtueLabel");
	}
	
	private Label viceLabel;
	private Label virtueLabel;
	
	public override void _Ready()
	{
		SheetData ??= new Mortal();
		
		viceLabel = GetNode<Label>(NodePaths.ViceLabel);
		virtueLabel = GetNode<Label>(NodePaths.VirtueLabel);
		
		InitTrackSimple(GetNode<TrackSimple>(NodePaths.Integrity), SheetData.Advantages.Integrity, changed_Integrity, DefaultIntegrityMax);
		viceLabel.Text = SheetData.Details.Vice;
		virtueLabel.Text = SheetData.Details.Virtue;
		
		InitSpinBox(GetNode<SpinBox>(NodePaths.Age), SheetData.Age, changed_Age);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Faction), SheetData.Details.Faction, changed_Faction);
		InitLineEdit(GetNode<LineEdit>(NodePaths.GroupName), SheetData.Details.TypePrimary, changed_GroupName);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Vice), SheetData.Details.Vice, changed_Vice);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Virtue), SheetData.Details.Virtue, changed_Virtue);
		
		base._Ready();
	}

	private void changed_Age(double number) => SheetData.Age = (int)number;
	
	private void changed_Faction(string newText)
	{
		var details = SheetData.Details;
		details.Faction = newText;
		SheetData.Details = details;
	}
	
	private void changed_GroupName(string newText)
	{
		var details = SheetData.Details;
		details.TypePrimary = newText;
		SheetData.Details = details;
	}
	
	private void changed_Integrity(int value, string name)
	{
		var advantages = SheetData.Advantages;
		advantages.Integrity = value;
		SheetData.Advantages = advantages;
	}
	
	private void changed_Vice(string newText)
	{
		var details = SheetData.Details;
		details.Vice = newText;
		SheetData.Details = details;
		
		viceLabel.Text = SheetData.Details.Vice;
	}
	
	private void changed_Virtue(string newText)
	{
		var details = SheetData.Details;
		details.Virtue = newText;
		SheetData.Details = details;
		
		virtueLabel.Text = SheetData.Details.Virtue;
	}
}
