using Godot;
using Ocsm.Cofd;
using Ocsm.Nodes.Sheets;

namespace Ocsm.Nodes.Cofd.Sheets;

public partial class MortalSheet : CoreSheet<Mortal>, ICharacterSheet
{
	private sealed new class NodePath : CoreSheet<Mortal>.NodePath
	{
		public const string Integrity = "%Advantages/%Integrity";
		public const string Age = "%Details/%Age";
		public const string Faction = "%Details/%Faction";
		public const string GroupName = "%Details/%GroupName";
		public const string Vice = "%Details/%Vice";
		public const string ViceLabel = "%Advantages/%ViceLabel";
		public const string Virtue = "%Details/%Virtue";
		public const string VirtueLabel = "%Advantages/%VirtueLabel";
	}
	
	private Label viceLabel;
	private Label virtueLabel;
	
	public override void _Ready()
	{
		if(!(SheetData is Mortal))
			SheetData = new Mortal();
		
		viceLabel = GetNode<Label>(NodePath.ViceLabel);
		virtueLabel = GetNode<Label>(NodePath.VirtueLabel);
		
		InitTrackSimple(GetNode<TrackSimple>(NodePath.Integrity), SheetData.Advantages.Integrity, changed_Integrity, DefaultIntegrityMax);
		viceLabel.Text = SheetData.Details.Vice;
		virtueLabel.Text = SheetData.Details.Virtue;
		
		InitSpinBox(GetNode<SpinBox>(NodePath.Age), SheetData.Age, changed_Age);
		InitLineEdit(GetNode<LineEdit>(NodePath.Faction), SheetData.Details.Faction, changed_Faction);
		InitLineEdit(GetNode<LineEdit>(NodePath.GroupName), SheetData.Details.TypePrimary, changed_GroupName);
		InitLineEdit(GetNode<LineEdit>(NodePath.Vice), SheetData.Details.Vice, changed_Vice);
		InitLineEdit(GetNode<LineEdit>(NodePath.Virtue), SheetData.Details.Virtue, changed_Virtue);
		
		base._Ready();
	}
	
	private void changed_Age(double number) { SheetData.Age = (int)number; }
	private void changed_Faction(string newText) { SheetData.Details.Faction = newText; }
	private void changed_GroupName(string newText) { SheetData.Details.TypePrimary = newText; }
	private void changed_Integrity(long value) { SheetData.Advantages.Integrity = value; }
	
	private void changed_Vice(string newText)
	{
		SheetData.Details.Vice = newText;
		viceLabel.Text = SheetData.Details.Vice;
	}
	
	private void changed_Virtue(string newText)
	{
		SheetData.Details.Virtue = newText;
		virtueLabel.Text = SheetData.Details.Virtue;
	}
}
