using Godot;
using OCSM.CoD;
using OCSM.Nodes.Sheets;

namespace OCSM.Nodes.CoD.Sheets
{
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
			if(!(SheetData is Mortal))
				SheetData = new Mortal();
			
			InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Integrity, AdvantagesPath)), SheetData.Integrity.ToString(), nameof(changed_Integrity));
			GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = SheetData.Vice;
			GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = SheetData.Virtue;
			
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Detail.Age, DetailsPath)), SheetData.Age, nameof(changed_Age));
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Faction, DetailsPath)), SheetData.Faction, nameof(changed_Faction));
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.GroupName, DetailsPath)), SheetData.GroupName, nameof(changed_GroupName));
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Vice, DetailsPath)), SheetData.Vice, nameof(changed_Vice));
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Virtue, DetailsPath)), SheetData.Virtue, nameof(changed_Virtue));
			
			base._Ready();
		}
		
		private void changed_Age(float number) { SheetData.Age = (int)number; }
		private void changed_Faction(string newText) { SheetData.Faction = newText; }
		private void changed_GroupName(string newText) { SheetData.GroupName = newText; }
		private void changed_Integrity(int value) { SheetData.Integrity = value; }
		
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