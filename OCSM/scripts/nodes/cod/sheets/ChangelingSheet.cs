using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
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
			public const string ContractsList = "%ContractsList";
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
		
		private List<TrackSimple> attributes = new List<TrackSimple>();
		private TrackSimple glamour;
		private MeritList merits;
		private RegaliaOptionButton regalia1;
		private RegaliaOptionButton regalia2;
		private List<TrackSimple> skills = new List<TrackSimple>();
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			if(!(SheetData is Changeling))
				SheetData = new Changeling();
			
			glamour = GetNode<TrackSimple>(NodePath.Glamour);
			merits = GetNode<MeritList>(NodePath.Merits);
			regalia1 = GetNode<RegaliaOptionButton>(NodePath.Regalia1);
			regalia2 = GetNode<RegaliaOptionButton>(NodePath.Regalia2);
			
			InitTrackSimple(GetNode<TrackSimple>(NodePath.Clarity), SheetData.Advantages.Integrity, changed_Clarity, IntegrityMax);
			InitTrackSimple(GetNode<TrackSimple>(NodePath.Wyrd), SheetData.Advantages.Power, changed_Wyrd, IntegrityMax);
			InitTrackSimple(glamour, SheetData.Advantages.ResourceSpent, changed_Glamour, SheetData.Advantages.ResourceMax);
			
			GetNode<Label>(NodePath.NeedleLabel).Text = SheetData.Details.Virtue;
			GetNode<Label>(NodePath.ThreadLabel).Text = SheetData.Details.Vice;
			
			Court court = null;
			Seeming seeming = null;
			Kith kith = null;
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Courts.Find(c => c.Name.Equals(SheetData.Details.Faction)) is Court c)
					court = c;
				if(ccc.Seemings.Find(s => s.Name.Equals(SheetData.Details.TypePrimary)) is Seeming s)
					seeming = s;
				if(ccc.Kiths.Find(k => k.Name.Equals(SheetData.Details.TypeSecondary)) is Kith k)
					kith = k;
			}
			
			InitCourtOptionButton(GetNode<CourtOptionButton>(NodePath.Court), court, changed_Court);
			InitEntryList(GetNode<EntryList>(NodePath.Frailties), SheetData.Frailties, changed_Frailties);
			InitKithOptionButton(GetNode<KithOptionButton>(NodePath.Kith), kith, changed_Kith);
			InitLineEdit(GetNode<LineEdit>(NodePath.Needle), SheetData.Details.Virtue, changed_Needle);
			InitRegaliaOptionButton(regalia1, SheetData.FavoredRegalia.Key, changed_FavoredRegalia);
			InitRegaliaOptionButton(regalia2, SheetData.FavoredRegalia.Value, changed_FavoredRegalia);
			InitSeemingOptionButton(GetNode<SeemingOptionButton>(NodePath.Seeming), seeming, changed_Seeming);
			InitLineEdit(GetNode<LineEdit>(NodePath.Thread), SheetData.Details.Vice, changed_Thread);
			InitEntryList(GetNode<EntryList>(NodePath.Touchstones), SheetData.Touchstones, changed_Touchstones);
			
			InitContractsList(GetNode<ContractsList>(NodePath.ContractsList), SheetData.Contracts, changed_Contracts);
			
			GetNode<MeritsFromMetadata>(NodePath.MeritsFromMetadata).AddMerit += addExistingMerit;
			
			base._Ready();
			
			//Now that CoDCore sets a default maximum on attributes/skills we need to update after base._Ready()
			var dotMax = AttributeMax;
			if(SheetData.Advantages.Power > 5)
				dotMax = SheetData.Advantages.Power;
			
			SheetData.Attributes.ForEach(a => {
				if(GetNode<TrackSimple>(NodePath.Attributes + "/%" + a.Name) is TrackSimple node)
				{
					node.updateMax(dotMax);
					node.updateValue(a.Value);
					attributes.Add(node);
				}
			});
			
			SheetData.Skills.ForEach(s => {
				if(GetNode<TrackSimple>(NodePath.Skills + "/%" + s.Name) is TrackSimple node)
				{
					node.updateMax(dotMax);
					node.updateValue(s.Value);
					skills.Add(node);
				}
			});
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
				if(initialValue is Court)
					node.SelectItemByText(initialValue.Name);
				node.ItemSelected += handler;
			}
		}
		
		protected void InitKithOptionButton(KithOptionButton node, Kith initialValue, KithOptionButton.ItemSelectedEventHandler handler)
		{
			if(node is KithOptionButton)
			{
				if(initialValue is Kith)
					node.SelectItemByText(initialValue.Name);
				node.ItemSelected += handler;
			}
		}
		
		protected void InitRegaliaOptionButton(RegaliaOptionButton node, Regalia initialValue, RegaliaOptionButton.ItemSelectedEventHandler handler)
		{
			if(node is RegaliaOptionButton)
			{
				if(initialValue is Regalia)
					node.SelectItemByText(initialValue.Name);
				node.ItemSelected += handler;
			}
		}
		
		protected void InitSeemingOptionButton(SeemingOptionButton node, Seeming initialValue, SeemingOptionButton.ItemSelectedEventHandler handler)
		{
			if(node is SeemingOptionButton)
			{
				if(initialValue is Seeming)
					node.SelectItemByText(initialValue.Name);
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
		
		private void changed_Clarity(long value) { SheetData.Advantages.Integrity = value; }
		private void changed_Contracts(Transport<List<OCSM.CoD.CtL.Contract>> transport) { SheetData.Contracts = transport.Value; }
		
		private void changed_Court(long index)
		{
			var name = String.Empty;
			if(index > 0 && metadataManager.Container is CoDChangelingContainer ccc && ccc.Courts[(int)index - 1] is Court court)
				name = court.Name;
			
			SheetData.Details.Faction = name;
		}
		
		private void changed_Frailties(Transport<List<string>> transport) { SheetData.Frailties = transport.Value; }
		private void changed_Glamour(long value) { SheetData.Advantages.ResourceSpent = value; }
		
		private void changed_Kith(long index)
		{
			var name = String.Empty;
			if(index > 0 && metadataManager.Container is CoDChangelingContainer ccc && ccc.Kiths[(int)index - 1] is Kith kith)
				name = kith.Name;
			
			SheetData.Details.TypeSecondary = name;
		}
		
		private void changed_Needle(string value) { SheetData.Details.Virtue = value; }
		
		private void changed_FavoredRegalia(long item)
		{
			var pair = new Pair<Regalia, Regalia>();
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				var r1 = regalia1.GetSelectedItemText();
				var r2 = regalia2.GetSelectedItemText();
				pair.Key = ccc.Regalias.Where(r => r.Name.Equals(r1))
					.FirstOrDefault();
				pair.Value = ccc.Regalias.Where(r => r.Name.Equals(r2))
					.FirstOrDefault();
			}
			
			SheetData.FavoredRegalia = pair;
		}
		
		private void changed_Seeming(long index)
		{
			var name = String.Empty;
			if(index > 0 && metadataManager.Container is CoDChangelingContainer ccc && ccc.Seemings[(int)index - 1] is Seeming seeming)
				name = seeming.Name;
			
			SheetData.Details.TypePrimary = name;
		}
		
		private void changed_Thread(string value) { SheetData.Details.Vice = value; }
		private void changed_Touchstones(Transport<List<string>> transport) { SheetData.Touchstones = transport.Value; }
		
		private void changed_Wyrd(long value)
		{
			SheetData.Advantages.Power = value;
			if(Changeling.WyrdGlamour.ContainsKey(value))
			{
				SheetData.Advantages.ResourceMax = Changeling.WyrdGlamour[value];
				glamour.updateMax(SheetData.Advantages.ResourceMax);
			}
			
			var dotMax = AttributeMax;
			if(SheetData.Advantages.Power > 5)
				dotMax = SheetData.Advantages.Power;
			
			attributes.ForEach(n => n.updateMax(dotMax));
			skills.ForEach(n => n.updateMax(dotMax));
		}
	}
}
