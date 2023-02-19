using Godot;
using System;
using System.Collections.Generic;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.CoD.CtL;
using OCSM.Nodes.CoD.CtL.Meta;
using OCSM.Nodes.Sheets;

namespace OCSM.Nodes.CoD.Sheets
{
	public partial class ChangelingSheet : CoreSheet<Changeling>, ICharacterSheet
	{
		private sealed new class Advantage : CoreSheet<Mortal>.Advantage
		{
			public const string Clarity = "Clarity";
			public const string Glamour = "Glamour";
			public const string Needle = "Needle";
			public const string Thread = "Thread";
			public const string Wyrd = "Wyrd";
		}
		
		private sealed new class Detail : CoreSheet<Mortal>.Detail
		{
			public const string Court = "Court";
			public const string Frailties = "Frailties";
			public const string Kith = "Kith";
			public const string Needle = "Needle";
			public const string Regalia1 = "Regalia1";
			public const string Regalia2 = "Regalia2";
			public const string Seeming = "Seeming";
			public const string Thread = "Thread";
			public const string Touchstones = "Touchstones";
		}
		
		private const string ContractsListName = "Contracts";
		private const string MeritsFromMetadataName = "MeritsFromMetadata";
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			if(!(SheetData is Changeling))
				SheetData = new Changeling();
			
			InitTrackSimple(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Clarity, AdvantagesPath)), SheetData.Clarity, changed_Clarity);
			InitTrackSimple(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Wyrd, AdvantagesPath)), SheetData.Wyrd, changed_Wyrd);
			InitTrackSimple(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Glamour, AdvantagesPath)), SheetData.GlamourSpent, changed_Glamour);
			
			GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Needle, AdvantagesPath)).Text = SheetData.Needle;
			GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Thread, AdvantagesPath)).Text = SheetData.Thread;
			
			InitCourtOptionButton(GetNode<CourtOptionButton>(NodePathBuilder.SceneUnique(Detail.Court, DetailsPath)), SheetData.Court, changed_Court);
			InitEntryList(GetNode<EntryList>(NodePathBuilder.SceneUnique(Detail.Frailties, DetailsPath)), SheetData.Frailties, changed_Frailties);
			InitKithOptionButton(GetNode<KithOptionButton>(NodePathBuilder.SceneUnique(Detail.Kith, DetailsPath)), SheetData.Kith, changed_Kith);
			InitLineEdit(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Needle, DetailsPath)), SheetData.Needle, changed_Needle);
			InitRegaliaOptionButton(GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Detail.Regalia1, DetailsPath)), SheetData.FavoredRegalia.Count > 0 ? SheetData.FavoredRegalia[0] : null, changed_FavoredRegalia);
			InitRegaliaOptionButton(GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Detail.Regalia2, DetailsPath)), SheetData.FavoredRegalia.Count > 1 ? SheetData.FavoredRegalia[1] : null, changed_FavoredRegalia);
			InitSeemingOptionButton(GetNode<SeemingOptionButton>(NodePathBuilder.SceneUnique(Detail.Seeming, DetailsPath)), SheetData.Seeming, changed_Seeming);
			InitLineEdit(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Thread, DetailsPath)), SheetData.Thread, changed_Thread);
			InitEntryList(GetNode<EntryList>(NodePathBuilder.SceneUnique(Detail.Touchstones, DetailsPath)), SheetData.Touchstones, changed_Touchstones);
			
			InitContractsList(GetNode<ContractsList>(NodePathBuilder.SceneUnique(ContractsListName)), SheetData.Contracts, changed_Contracts);
			
			GetNode<MeritsFromMetadata>(NodePathBuilder.SceneUnique(MeritsFromMetadataName)).AddMerit += addExistingMerit;
			
			base._Ready();
		}
		
		protected void InitContractsList(ContractsList node, List<OCSM.CoD.CtL.Contract> initialValue, ContractsList.ValueChangedEventHandler handler)
		{
			if(node is ContractsList)
			{
				if(initialValue is List<OCSM.CoD.CtL.Contract>)
					node.Values = initialValue;
				node.refresh();
				node.ValueChanged += handler;
			}
		}
		
		protected void InitCourtOptionButton(CourtOptionButton node, Court initialValue, OptionButton.ItemSelectedEventHandler handler)
		{
			if(node is CourtOptionButton)
			{
				if(initialValue is Court && metadataManager.Container is CoDChangelingContainer ccc)
				{
					var index = ccc.Courts.FindIndex(c => c.Equals(initialValue)) + 1;
					if(index > 0)
						node.Selected = index;
				}
				node.ItemSelected += handler;
			}
		}
		
		protected void InitKithOptionButton(KithOptionButton node, Kith initialValue, KithOptionButton.ItemSelectedEventHandler handler)
		{
			if(node is KithOptionButton)
			{
				if(initialValue is Kith && metadataManager.Container is CoDChangelingContainer ccc)
				{
					var index = ccc.Kiths.FindIndex(k => k.Equals(initialValue)) + 1;
					if(index > 0)
						node.Selected = index;
				}
				node.ItemSelected += handler;
			}
		}
		
		protected void InitRegaliaOptionButton(RegaliaOptionButton node, Regalia initialValue, RegaliaOptionButton.ItemSelectedEventHandler handler)
		{
			if(node is RegaliaOptionButton)
			{
				if(initialValue is Regalia && metadataManager.Container is CoDChangelingContainer ccc)
				{
					var index = ccc.Regalias.FindIndex(r => r.Equals(initialValue)) + 1;
					if(index > 0)
						node.Selected = index;
				}
				node.ItemSelected += handler;
			}
		}
		
		protected void InitSeemingOptionButton(SeemingOptionButton node, Seeming initialValue, SeemingOptionButton.ItemSelectedEventHandler handler)
		{
			if(node is SeemingOptionButton)
			{
				if(initialValue is Seeming && metadataManager.Container is CoDChangelingContainer ccc)
				{
					var index = ccc.Seemings.FindIndex(s => s.Equals(initialValue)) + 1;
					if(index > 0)
						node.Selected = index;
				}
				node.ItemSelected += handler;
			}
		}
		
		private void addExistingMerit(string name)
		{
			if(!String.IsNullOrEmpty(name) && metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
				{
					SheetData.Merits.Add(merit);
					var merits = GetNode<MeritList>(NodePathBuilder.SceneUnique(Merits));
					merits.Values = SheetData.Merits;
					merits.refresh();
				}
			}
		}
		
		private void changed_Clarity(long value) { SheetData.Clarity = value; }
		private void changed_Contracts(Transport<List<OCSM.CoD.CtL.Contract>> transport) { SheetData.Contracts = transport.Value; }
		
		private void changed_Court(long index)
		{
			if(index > 0 && metadataManager.Container is CoDChangelingContainer ccc && ccc.Courts[(int)index - 1] is Court court)
				SheetData.Court = court;
			else
				SheetData.Court = null;
		}
		
		private void changed_Frailties(Transport<List<string>> transport) { SheetData.Frailties = transport.Value; }
		private void changed_Glamour(long value) { SheetData.GlamourSpent = value; }
		
		private void changed_Kith(long index)
		{
			if(index > 0 && metadataManager.Container is CoDChangelingContainer ccc && ccc.Kiths[(int)index - 1] is Kith kith)
				SheetData.Kith = kith;
			else
				SheetData.Kith = null;
		}
		
		private void changed_Needle(string value) { SheetData.Needle = value; }
		
		private void changed_FavoredRegalia(long item)
		{
			SheetData.FavoredRegalia = new List<Regalia>(2);
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				var r1 = GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Detail.Regalia1, DetailsPath));
				if(r1.Selected > 0 && ccc.Regalias[r1.Selected - 1] is Regalia regalia1)
					SheetData.FavoredRegalia.Add(regalia1);
				
				var r2 = GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Detail.Regalia2, DetailsPath));
				if(r2.Selected > 0 && ccc.Regalias[r2.Selected - 1] is Regalia regalia2)
					SheetData.FavoredRegalia.Add(regalia2);
			}
		}
		
		private void changed_Seeming(long index)
		{
			if(index > 0 && metadataManager.Container is CoDChangelingContainer ccc && ccc.Seemings[(int)index - 1] is Seeming seeming)
				SheetData.Seeming = seeming;
			else
				SheetData.Seeming = null;
		}
		
		private void changed_Thread(string value) { SheetData.Thread = value; }
		private void changed_Touchstones(Transport<List<string>> transport) { SheetData.Touchstones = transport.Value; }
		private void changed_Wyrd(long value) { SheetData.Wyrd = value; }
	}
}
