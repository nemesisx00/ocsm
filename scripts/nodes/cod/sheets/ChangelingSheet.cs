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
		private sealed new class NodePath : CoreSheet<Changeling>.NodePath
		{
			public const string Clarity = NodePath.Advantages + "/%Clarity";
			public const string ContractsList = "%Contracts";
			public const string Court = NodePath.Details + "/%Court";
			public const string Frailties = NodePath.Details + "/%Frailties";
			public const string Glamour = NodePath.Advantages + "/%Glamour";
			public const string Kith = NodePath.Details + "/%Kith";
			public const string Needle = NodePath.Details + "/%Needle";
			public const string NeedleLabel = NodePath.Advantages + "/%NeedleLabel";
			public const string Regalia1 = NodePath.Details + "/%Regalia1";
			public const string Regalia2 = NodePath.Details + "/%Regalia2";
			public const string Seeming = NodePath.Details + "/%Seeming";
			public const string Thread = NodePath.Details + "/%Thread";
			public const string ThreadLabel = NodePath.Advantages + "/%ThreadLabel";
			public const string Touchstones = NodePath.Details + "/%Touchstones";
			public const string Wyrd = NodePath.Advantages + "/%Wyrd";
		}
		
		private MetadataManager metadataManager;
		
		private MeritList merits;
		private RegaliaOptionButton regalia1;
		private RegaliaOptionButton regalia2;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			if(!(SheetData is Changeling))
				SheetData = new Changeling();
			
			merits = GetNode<MeritList>(NodePath.Merits);
			regalia1 = GetNode<RegaliaOptionButton>(NodePath.Regalia1);
			regalia2 = GetNode<RegaliaOptionButton>(NodePath.Regalia2);
			
			InitTrackSimple(GetNode<TrackSimple>(NodePath.Clarity), SheetData.Clarity, changed_Clarity);
			InitTrackSimple(GetNode<TrackSimple>(NodePath.Wyrd), SheetData.Wyrd, changed_Wyrd);
			InitTrackSimple(GetNode<TrackSimple>(NodePath.Glamour), SheetData.GlamourSpent, changed_Glamour);
			
			GetNode<Label>(NodePath.NeedleLabel).Text = SheetData.Needle;
			GetNode<Label>(NodePath.NeedleLabel).Text = SheetData.Thread;
			
			InitCourtOptionButton(GetNode<CourtOptionButton>(NodePath.Court), SheetData.Court, changed_Court);
			InitEntryList(GetNode<EntryList>(NodePath.Frailties), SheetData.Frailties, changed_Frailties);
			InitKithOptionButton(GetNode<KithOptionButton>(NodePath.Kith), SheetData.Kith, changed_Kith);
			InitLineEdit(GetNode<LineEdit>(NodePath.Needle), SheetData.Needle, changed_Needle);
			InitRegaliaOptionButton(regalia1, SheetData.FavoredRegalia.Count > 0 ? SheetData.FavoredRegalia[0] : null, changed_FavoredRegalia);
			InitRegaliaOptionButton(regalia2, SheetData.FavoredRegalia.Count > 1 ? SheetData.FavoredRegalia[1] : null, changed_FavoredRegalia);
			InitSeemingOptionButton(GetNode<SeemingOptionButton>(NodePath.Seeming), SheetData.Seeming, changed_Seeming);
			InitLineEdit(GetNode<LineEdit>(NodePath.Thread), SheetData.Thread, changed_Thread);
			InitEntryList(GetNode<EntryList>(NodePath.Touchstones), SheetData.Touchstones, changed_Touchstones);
			
			InitContractsList(GetNode<ContractsList>(NodePath.ContractsList), SheetData.Contracts, changed_Contracts);
			
			GetNode<MeritsFromMetadata>(NodePath.MeritsFromMetadata).AddMerit += addExistingMerit;
			
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
				if(regalia1.Selected > 0 && ccc.Regalias[regalia1.Selected - 1] is Regalia r1)
					SheetData.FavoredRegalia.Add(r1);
				
				if(regalia2.Selected > 0 && ccc.Regalias[regalia2.Selected - 1] is Regalia r2)
					SheetData.FavoredRegalia.Add(r2);
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
