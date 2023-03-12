using Godot;
using OCSM.CoD;
using OCSM.Nodes.Sheets;

namespace OCSM.Nodes.CoD.Sheets
{
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
			
			InitTrackSimple(GetNode<TrackSimple>(NodePath.Integrity), SheetData.Integrity, changed_Integrity);
			viceLabel.Text = SheetData.Vice;
			virtueLabel.Text = SheetData.Virtue;
			
			InitSpinBox(GetNode<SpinBox>(NodePath.Age), SheetData.Age, changed_Age);
			InitLineEdit(GetNode<LineEdit>(NodePath.Faction), SheetData.Faction, changed_Faction);
			InitLineEdit(GetNode<LineEdit>(NodePath.GroupName), SheetData.GroupName, changed_GroupName);
			InitLineEdit(GetNode<LineEdit>(NodePath.Vice), SheetData.Vice, changed_Vice);
			InitLineEdit(GetNode<LineEdit>(NodePath.Virtue), SheetData.Virtue, changed_Virtue);
			
			base._Ready();
		}
		
		private void changed_Age(double number) { SheetData.Age = (int)number; }
		private void changed_Faction(string newText) { SheetData.Faction = newText; }
		private void changed_GroupName(string newText) { SheetData.GroupName = newText; }
		private void changed_Integrity(long value) { SheetData.Integrity = value; }
		
		private void changed_Vice(string newText)
		{
			SheetData.Vice = newText;
			viceLabel.Text = SheetData.Vice;
		}
		
		private void changed_Virtue(string newText)
		{
			SheetData.Virtue = newText;
			virtueLabel.Text = SheetData.Virtue;
		}
	}
}
