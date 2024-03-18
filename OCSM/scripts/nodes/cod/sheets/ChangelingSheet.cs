using System.Collections.Generic;
using System.Linq;
using Godot;
using Ocsm.Cofd;
using Ocsm.Cofd.Ctl;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Cofd.Ctl;
using Ocsm.Nodes.Cofd.Ctl.Meta;
using Ocsm.Nodes.Sheets;

namespace Ocsm.Nodes.Cofd.Sheets;

public partial class ChangelingSheet : CoreSheet<Changeling>, ICharacterSheet
{
	private static new class NodePaths
	{
		public static readonly NodePath Clarity = new(CoreSheet<Changeling>.NodePaths.Advantages + "/%Clarity");
		public static readonly NodePath ContractsList = new("%ContractsList");
		public static readonly NodePath Court = new(CoreSheet<Changeling>.NodePaths.Details + "/%Court");
		public static readonly NodePath Frailties = new(CoreSheet<Changeling>.NodePaths.Details + "/%Frailties");
		public static readonly NodePath Glamour = new(CoreSheet<Changeling>.NodePaths.Advantages + "/%Glamour");
		public static readonly NodePath Kith = new(CoreSheet<Changeling>.NodePaths.Details + "/%Kith");
		public static readonly NodePath Needle = new(CoreSheet<Changeling>.NodePaths.Details + "/%Needle");
		public static readonly NodePath NeedleLabel = new(CoreSheet<Changeling>.NodePaths.Advantages + "/%NeedleLabel");
		public static readonly NodePath Regalia1 = new(CoreSheet<Changeling>.NodePaths.Details + "/%Regalia1");
		public static readonly NodePath Regalia2 = new(CoreSheet<Changeling>.NodePaths.Details + "/%Regalia2");
		public static readonly NodePath Seeming = new(CoreSheet<Changeling>.NodePaths.Details + "/%Seeming");
		public static readonly NodePath Thread = new(CoreSheet<Changeling>.NodePaths.Details + "/%Thread");
		public static readonly NodePath ThreadLabel = new(CoreSheet<Changeling>.NodePaths.Advantages + "/%ThreadLabel");
		public static readonly NodePath Touchstones = new(CoreSheet<Changeling>.NodePaths.Details + "/%Touchstones");
		public static readonly NodePath Wyrd = new(CoreSheet<Changeling>.NodePaths.Advantages + "/%Wyrd");
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
	
	protected static void InitMetadataOptionButton(MetadataOption node, Metadata initialValue, OptionButton.ItemSelectedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.SelectItemByText(initialValue.Name);
			node.ItemSelected += handler;
		}
	}
	
	private MetadataManager metadataManager;
	
	private readonly List<TrackSimple> attributes = [];
	private TrackSimple glamour;
	private MeritList merits;
	private MetadataOption regalia1;
	private MetadataOption regalia2;
	private readonly List<TrackSimple> skills = [];
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		SheetData ??= new Changeling();
		
		glamour = GetNode<TrackSimple>(NodePaths.Glamour);
		merits = GetNode<MeritList>(CoreSheet<Changeling>.NodePaths.Merits);
		regalia1 = GetNode<MetadataOption>(NodePaths.Regalia1);
		regalia2 = GetNode<MetadataOption>(NodePaths.Regalia2);
		
		InitTrackSimple(GetNode<TrackSimple>(NodePaths.Clarity), SheetData.Advantages.Integrity, changed_Clarity, DefaultIntegrityMax);
		InitTrackSimple(GetNode<TrackSimple>(NodePaths.Wyrd), SheetData.Advantages.Power, changed_Wyrd, DefaultIntegrityMax);
		InitTrackSimple(glamour, SheetData.Advantages.ResourceSpent, changed_Glamour, SheetData.Advantages.ResourceMax);
		
		GetNode<Label>(NodePaths.NeedleLabel).Text = SheetData.Details.Virtue;
		GetNode<Label>(NodePaths.ThreadLabel).Text = SheetData.Details.Vice;
		
		Metadata court = null;
		Metadata seeming = null;
		Metadata kith = null;
		
		if(metadataManager.Container is CofdChangelingContainer container)
		{
			if(container.Metadata.Where(m => m.Type == MetadataType.CofdChangelingCourt && m.Name == SheetData.Details.Faction).FirstOrDefault() is Metadata c)
				court = c;
			
			if(container.Metadata.Where(m => m.Type == MetadataType.CofdChangelingSeeming && m.Name == SheetData.Details.TypePrimary).FirstOrDefault() is Metadata s)
				seeming = s;
			
			if(container.Metadata.Where(m => m.Type == MetadataType.CofdChangelingKith && m.Name == SheetData.Details.TypeSecondary).FirstOrDefault() is Metadata k)
				kith = k;
		}
		
		InitMetadataOptionButton(GetNode<MetadataOption>(NodePaths.Court), court, changed_Court);
		InitEntryList(GetNode<EntryList>(NodePaths.Frailties), SheetData.Frailties, changed_Frailties);
		InitMetadataOptionButton(GetNode<MetadataOption>(NodePaths.Kith), kith, changed_Kith);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Needle), SheetData.Details.Virtue, changed_Needle);
		InitMetadataOptionButton(regalia1, SheetData.FavoredRegalia.Key, changed_FavoredRegalia);
		InitMetadataOptionButton(regalia2, SheetData.FavoredRegalia.Value, changed_FavoredRegalia);
		InitMetadataOptionButton(GetNode<MetadataOption>(NodePaths.Seeming), seeming, changed_Seeming);
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
			if(GetNode<TrackSimple>(CoreSheet<Changeling>.NodePaths.Attributes + "/%" + a.Name) is TrackSimple node)
			{
				node.UpdateMax(dotMax);
				node.UpdateValue(a.Value);
				attributes.Add(node);
			}
		});
		
		SheetData.Skills.ForEach(s => {
			if(GetNode<TrackSimple>(CoreSheet<Changeling>.NodePaths.Skills + "/%" + s.Name) is TrackSimple node)
			{
				node.UpdateMax(dotMax);
				node.UpdateValue(s.Value);
				skills.Add(node);
			}
		});
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
	
	private void changed_Clarity(int value) => SheetData.Advantages.Integrity = value;
	private void changed_Contracts(Transport<List<Ocsm.Cofd.Ctl.Contract>> transport) { SheetData.Contracts = transport.Value; }
	
	private void changed_Court(long index)
	{
		var name = string.Empty;
		
		if(index > 0 && metadataManager.Container is CofdChangelingContainer ccc
			&& ccc.Metadata.Where(m => m.Type == MetadataType.CofdChangelingCourt).ToList()[(int)index - 1] is Metadata court)
		{
			name = court.Name;
		}
		
		SheetData.Details.Faction = name;
	}
	
	private void changed_Frailties(Transport<List<string>> transport) => SheetData.Frailties = transport.Value;
	private void changed_Glamour(int value) => SheetData.Advantages.ResourceSpent = value;
	
	private void changed_Kith(long index)
	{
		var name = string.Empty;
		if(index > 0
			&& metadataManager.Container is CofdChangelingContainer container
			&& container.Metadata.Where(m => m.Type == MetadataType.CofdChangelingKith).ToList()[(int)index - 1] is Metadata kith)
		{
			name = kith.Name;
		}
		
		SheetData.Details.TypeSecondary = name;
	}
	
	private void changed_Needle(string value) => SheetData.Details.Virtue = value;
	
	private void changed_FavoredRegalia(long item)
	{
		Pair<Metadata, Metadata> pair = new();
		if(metadataManager.Container is CofdChangelingContainer container)
		{
			var r1 = regalia1.GetSelectedItemText();
			var r2 = regalia2.GetSelectedItemText();
			
			pair.Key = container.Metadata
				.Where(m => m.Type == MetadataType.CofdChangelingRegalia && m.Name == r1)
				.FirstOrDefault();
			
			pair.Value = container.Metadata
				.Where(m => m.Type == MetadataType.CofdChangelingRegalia && m.Name == r2)
				.FirstOrDefault();
		}
		
		SheetData.FavoredRegalia = pair;
	}
	
	private void changed_Seeming(long index)
	{
		var name = string.Empty;
		if(index > 0
			&& metadataManager.Container is CofdChangelingContainer container
			&& container.Metadata.Where(m => m.Type == MetadataType.CofdChangelingSeeming)
			.ToList()[(int)index - 1] is Metadata seeming)
		{
			name = seeming.Name;
		}
		
		SheetData.Details.TypePrimary = name;
	}
	
	private void changed_Thread(string value) => SheetData.Details.Vice = value;
	private void changed_Touchstones(Transport<List<string>> transport) => SheetData.Touchstones = transport.Value;
	
	private void changed_Wyrd(int value)
	{
		SheetData.Advantages.Power = value;
		
		if(Changeling.WyrdGlamour.TryGetValue(value, out int max))
		{
			SheetData.Advantages.ResourceMax = max;
			glamour.UpdateMax(SheetData.Advantages.ResourceMax);
		}
		
		var dotMax = DefaultAttributeMax;
		if(SheetData.Advantages.Power > 5)
			dotMax = SheetData.Advantages.Power;
		
		attributes.ForEach(n => n.UpdateMax(dotMax));
		skills.ForEach(n => n.UpdateMax(dotMax));
	}
}