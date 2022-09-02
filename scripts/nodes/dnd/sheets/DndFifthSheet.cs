using OCSM.DnD.Fifth;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.Sheets;

namespace OCSM.Nodes.DnD.Sheets
{
	public class DndFifthSheet : CharacterSheet<FifthAdventurer>
	{
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			if(!(SheetData is FifthAdventurer))
				SheetData = new FifthAdventurer();
			
			/*
			InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Clarity, AdvantagesPath)), SheetData.Clarity, nameof(changed_Clarity));
			InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Wyrd, AdvantagesPath)), SheetData.Wyrd, nameof(changed_Wyrd));
			InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Glamour, AdvantagesPath)), SheetData.GlamourSpent, nameof(changed_Glamour));
			GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Needle, AdvantagesPath)).Text = SheetData.Needle;
			GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Thread, AdvantagesPath)).Text = SheetData.Thread;
			
			InitAndConnect(GetNode<CourtOptionButton>(NodePathBuilder.SceneUnique(Detail.Court, DetailsPath)), SheetData.Court, nameof(changed_Court));
			InitAndConnect(GetNode<EntryList>(NodePathBuilder.SceneUnique(Detail.Frailties, DetailsPath)), SheetData.Frailties, nameof(changed_Frailties));
			InitAndConnect(GetNode<KithOptionButton>(NodePathBuilder.SceneUnique(Detail.Kith, DetailsPath)), SheetData.Kith, nameof(changed_Kith));
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Needle, DetailsPath)), SheetData.Needle, nameof(changed_Needle));
			InitAndConnect(GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Detail.Regalia1, DetailsPath)), SheetData.FavoredRegalia.Count > 0 ? SheetData.FavoredRegalia[0] : null, nameof(changed_FavoredRegalia));
			InitAndConnect(GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Detail.Regalia2, DetailsPath)), SheetData.FavoredRegalia.Count > 1 ? SheetData.FavoredRegalia[1] : null, nameof(changed_FavoredRegalia));
			InitAndConnect(GetNode<SeemingOptionButton>(NodePathBuilder.SceneUnique(Detail.Seeming, DetailsPath)), SheetData.Seeming, nameof(changed_Seeming));
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Thread, DetailsPath)), SheetData.Thread, nameof(changed_Thread));
			InitAndConnect(GetNode<EntryList>(NodePathBuilder.SceneUnique(Detail.Touchstones, DetailsPath)), SheetData.Touchstones, nameof(changed_Touchstones));
			
			InitAndConnect(GetNode<ContractsList>(NodePathBuilder.SceneUnique(ContractsListName)), SheetData.Contracts, nameof(changed_Contracts));
			
			GetNode<MeritsFromMetadata>(NodePathBuilder.SceneUnique(MeritsFromMetadataName)).Connect(nameof(MeritsFromMetadata.AddMerit), this, nameof(addExistingMerit));
			*/
			
			base._Ready();
			
			NodeUtilities.autoSizeChildren(this, Constants.TextInputMinHeight);
		}
	}
}
