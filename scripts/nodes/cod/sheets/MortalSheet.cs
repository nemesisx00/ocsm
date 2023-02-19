using Godot;
using OCSM.CoD;
using OCSM.Nodes.Sheets;

namespace OCSM.Nodes.CoD.Sheets
{
	public partial class MortalSheet : CoreSheet<Mortal>, ICharacterSheet
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
			if(!(SheetData is Mortal))
				SheetData = new Mortal();
			
			InitTrackSimple(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Integrity, AdvantagesPath)), SheetData.Integrity, changed_Integrity);
			GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = SheetData.Vice;
			GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = SheetData.Virtue;
			
			InitSpinBox(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Detail.Age, DetailsPath)), SheetData.Age, changed_Age);
			InitLineEdit(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Faction, DetailsPath)), SheetData.Faction, changed_Faction);
			InitLineEdit(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.GroupName, DetailsPath)), SheetData.GroupName, changed_GroupName);
			InitLineEdit(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Vice, DetailsPath)), SheetData.Vice, changed_Vice);
			InitLineEdit(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Virtue, DetailsPath)), SheetData.Virtue, changed_Virtue);
			
			base._Ready();
		}
		
		private void changed_Age(double number) { SheetData.Age = (int)number; }
		private void changed_Faction(string newText) { SheetData.Faction = newText; }
		private void changed_GroupName(string newText) { SheetData.GroupName = newText; }
		private void changed_Integrity(long value) { SheetData.Integrity = value; }
		
		private void changed_Vice(string newText)
		{
			SheetData.Vice = newText;
			GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = SheetData.Vice;
		}
		
		private void changed_Virtue(string newText)
		{
			SheetData.Virtue = newText;
			GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = SheetData.Virtue;
		}
	}
}
