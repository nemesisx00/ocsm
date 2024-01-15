using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Cofd.Nodes;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class ChangelingSheet : CoreSheet<Changeling>, ICharacterSheet
{
	private sealed new class NodePaths
	{
		public static readonly NodePath Clarity = new($"{CoreSheet<Changeling>.NodePaths.Advantages}/%Clarity");
		public static readonly NodePath ContractsList = new("%ContractsList");
		public static readonly NodePath Court = new($"{CoreSheet<Changeling>.NodePaths.Details}/%Court");
		public static readonly NodePath Frailties = new($"{CoreSheet<Changeling>.NodePaths.Details}/%Frailties");
		public static readonly NodePath Glamour = new($"{CoreSheet<Changeling>.NodePaths.Advantages}/%Glamour");
		public static readonly NodePath Kith = new($"{CoreSheet<Changeling>.NodePaths.Details}/%Kith");
		public static readonly NodePath Needle = new($"{CoreSheet<Changeling>.NodePaths.Details}/%Needle");
		public static readonly NodePath NeedleLabel = new($"{CoreSheet<Changeling>.NodePaths.Advantages}/%NeedleLabel");
		public static readonly NodePath Regalia1 = new($"{CoreSheet<Changeling>.NodePaths.Details}/%Regalia1");
		public static readonly NodePath Regalia2 = new($"{CoreSheet<Changeling>.NodePaths.Details}/%Regalia2");
		public static readonly NodePath Seeming = new($"{CoreSheet<Changeling>.NodePaths.Details}/%Seeming");
		public static readonly NodePath Thread = new($"{CoreSheet<Changeling>.NodePaths.Details}/%Thread");
		public static readonly NodePath ThreadLabel = new($"{CoreSheet<Changeling>.NodePaths.Advantages}/%ThreadLabel");
		public static readonly NodePath Touchstones = new($"{CoreSheet<Changeling>.NodePaths.Details}/%Touchstones");
		public static readonly NodePath Wyrd = new($"{CoreSheet<Changeling>.NodePaths.Advantages}/%Wyrd");
	}
	
	private MetadataManager metadataManager;
	
	private readonly List<TrackSimple> attributes = [];
	private TrackSimple glamour;
	private MeritList merits;
	private RegaliaOptionButton regalia1;
	private RegaliaOptionButton regalia2;
	private readonly List<TrackSimple> skills = [];
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		SheetData ??= new Changeling();
		
		glamour = GetNode<TrackSimple>(NodePaths.Glamour);
		merits = GetNode<MeritList>(CoreSheet<Changeling>.NodePaths.Merits);
		regalia1 = GetNode<RegaliaOptionButton>(NodePaths.Regalia1);
		regalia2 = GetNode<RegaliaOptionButton>(NodePaths.Regalia2);
		
		InitTrackSimple(GetNode<TrackSimple>(NodePaths.Clarity), SheetData.Advantages.Integrity, changed_Clarity, DefaultIntegrityMax);
		InitTrackSimple(GetNode<TrackSimple>(NodePaths.Wyrd), SheetData.Advantages.Power, changed_Wyrd, DefaultIntegrityMax);
		InitTrackSimple(glamour, SheetData.Advantages.ResourceSpent, changed_Glamour, SheetData.Advantages.ResourceMax);
		
		GetNode<Label>(NodePaths.NeedleLabel).Text = SheetData.Details.Virtue;
		GetNode<Label>(NodePaths.ThreadLabel).Text = SheetData.Details.Vice;
		
		Court court = null;
		Seeming seeming = null;
		Kith kith = null;
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Courts.Find(c => c.Name.Equals(SheetData.Details.Faction)) is Court c)
				court = c;
			
			if(ccc.Seemings.Find(s => s.Name.Equals(SheetData.Details.TypePrimary)) is Seeming s)
				seeming = s;
			
			if(ccc.Kiths.Find(k => k.Name.Equals(SheetData.Details.TypeSecondary)) is Kith k)
				kith = k;
		}

		InitCourtOptionButton(GetNode<CourtOptionButton>(NodePaths.Court), court, changed_Court);
		InitEntryList(GetNode<EntryList>(NodePaths.Frailties), SheetData.Frailties, changed_Frailties);
		InitKithOptionButton(GetNode<KithOptionButton>(NodePaths.Kith), kith, changed_Kith);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Needle), SheetData.Details.Virtue, changed_Needle);
		InitRegaliaOptionButton(regalia1, SheetData.FavoredRegalia.Key, changed_FavoredRegalia);
		InitRegaliaOptionButton(regalia2, SheetData.FavoredRegalia.Value, changed_FavoredRegalia);
		InitSeemingOptionButton(GetNode<SeemingOptionButton>(NodePaths.Seeming), seeming, changed_Seeming);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Thread), SheetData.Details.Vice, changed_Thread);
		InitEntryList(GetNode<EntryList>(NodePaths.Touchstones), SheetData.Touchstones, changed_Touchstones);

		InitContractsList(GetNode<ContractsList>(NodePaths.ContractsList), SheetData.Contracts, changed_Contracts);
		
		GetNode<MeritsFromMetadata>(CoreSheet<Changeling>.NodePaths.MeritsFromMetadata).AddMerit += addExistingMerit;
		
		base._Ready();
		
		//Now that CofdCore sets a default maximum on attributes/skills we need to update after base._Ready()
		var dotMax = DefaultAttributeMax;
		if(SheetData.Advantages.Power > 5)
			dotMax = SheetData.Advantages.Power;
		
		SheetData.Attributes.ForEach(a => {
			if(GetNode<TrackSimple>($"{CoreSheet<Changeling>.NodePaths.Attributes}/%{a.Name}") is TrackSimple node)
			{
				node.Max = dotMax;
				node.Value = a.Value;
				attributes.Add(node);
			}
		});
		
		SheetData.Skills.ForEach(s => {
			if(GetNode<TrackSimple>($"{CoreSheet<Changeling>.NodePaths.Skills}/%{s.Name}") is TrackSimple node)
			{
				node.Max = dotMax;
				node.Value = s.Value;
				skills.Add(node);
			}
		});
	}
	
	protected static void InitContractsList(ContractsList node, List<Contract> initialValue, ContractsList.ValueChangedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.Values = initialValue;
			
			node.Refresh();
			node.ValueChanged += handler;
		}
	}
	
	protected static void InitCourtOptionButton(CourtOptionButton node, Court initialValue, OptionButton.ItemSelectedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.SelectItemByText(initialValue.Name);
			
			node.ItemSelected += handler;
		}
	}
	
	protected static void InitKithOptionButton(KithOptionButton node, Kith initialValue, OptionButton.ItemSelectedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.SelectItemByText(initialValue.Name);
			
			node.ItemSelected += handler;
		}
	}
	
	protected static void InitRegaliaOptionButton(RegaliaOptionButton node, Regalia initialValue, OptionButton.ItemSelectedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.SelectItemByText(initialValue.Name);
			
			node.ItemSelected += handler;
		}
	}
	
	protected static void InitSeemingOptionButton(SeemingOptionButton node, Seeming initialValue, OptionButton.ItemSelectedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.SelectItemByText(initialValue.Name);
			
			node.ItemSelected += handler;
		}
	}
	
	private void addExistingMerit(string name)
	{
		if(!string.IsNullOrEmpty(name) && metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
			{
				SheetData.Merits.Add(merit);
				merits.Values = SheetData.Merits;
				merits.Refresh();
			}
		}
	}
	
	private void changed_Clarity(int value, string name)
	{
		var advantages = SheetData.Advantages;
		advantages.Integrity = value;
		SheetData.Advantages = advantages;
	}
	
	private void changed_Contracts(Transport<List<Contract>> transport) => SheetData.Contracts = transport.Value;
	
	private void changed_Court(long index)
	{
		var name = string.Empty;
		if(index > 0 && metadataManager.Container is CofdChangelingContainer ccc && ccc.Courts[(int)index - 1] is Court court)
			name = court.Name;
		
		var details = SheetData.Details;
		details.Faction = name;
		SheetData.Details = details;
	}
	
	private void changed_Frailties(Transport<List<string>> transport) => SheetData.Frailties = transport.Value;
	
	private void changed_Glamour(int value, string name)
	{
		var advantages = SheetData.Advantages;
		advantages.ResourceSpent = value;
		SheetData.Advantages = advantages;
	}
	
	private void changed_Kith(long index)
	{
		var name = string.Empty;
		if(index > 0 && metadataManager.Container is CofdChangelingContainer ccc && ccc.Kiths[(int)index - 1] is Kith kith)
			name = kith.Name;
		
		var details = SheetData.Details;
		details.TypeSecondary = name;
		SheetData.Details = details;
	}
	
	private void changed_Needle(string value)
	{
		var details = SheetData.Details;
		details.Virtue = value;
		SheetData.Details = details;
	}
	
	private void changed_FavoredRegalia(long item)
	{
		Pair<Regalia, Regalia> pair = new();
		
		if(metadataManager.Container is CofdChangelingContainer ccc)
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
		if(index > 0 && metadataManager.Container is CofdChangelingContainer ccc && ccc.Seemings[(int)index - 1] is Seeming seeming)
			name = seeming.Name;
		
		var details = SheetData.Details;
		details.TypePrimary = name;
		SheetData.Details = details;
	}
	
	private void changed_Thread(string value)
	{
		var details = SheetData.Details;
		details.Vice = value;
		SheetData.Details = details;
	}
	
	private void changed_Touchstones(Transport<List<string>> transport) => SheetData.Touchstones = transport.Value;
	
	private void changed_Wyrd(int value, string name)
	{
		var advantages = SheetData.Advantages;
		advantages.Power = value;
		
		if(Changeling.WyrdGlamour.TryGetValue(value, out int maxGlamour))
		{
			advantages.ResourceMax = maxGlamour;
			glamour.Max = advantages.ResourceMax;
		}
		
		SheetData.Advantages = advantages;
		
		var dotMax = DefaultAttributeMax;
		if(SheetData.Advantages.Power > 5)
			dotMax = SheetData.Advantages.Power;
		
		attributes.ForEach(n => n.Max = dotMax);
		skills.ForEach(n => n.Max = dotMax);
	}
}
