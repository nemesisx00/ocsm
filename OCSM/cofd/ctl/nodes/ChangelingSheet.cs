using System.Collections.Generic;
using System.Linq;
using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Cofd.Ctl.Nodes.Meta;
using Ocsm.Cofd.Nodes;
using Ocsm.Meta;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Cofd.Ctl.Nodes;

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
		public static readonly NodePath Regalia1 = new(CoreSheet<Changeling>.NodePaths.Details + "/%Regalia1");
		public static readonly NodePath Regalia2 = new(CoreSheet<Changeling>.NodePaths.Details + "/%Regalia2");
		public static readonly NodePath Seeming = new(CoreSheet<Changeling>.NodePaths.Details + "/%Seeming");
		public static readonly NodePath Thread = new(CoreSheet<Changeling>.NodePaths.Details + "/%Thread");
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
	
	private MetadataManager metadataManager;
	
	private TrackSimple clarity;
	private ContractsList contractsList;
	private DynamicMetadataLabel court;
	private FlowList frailties;
	private TrackSimple glamour;
	private DynamicMetadataLabel kith;
	private MeritsFromMetadata meritsFromMetadata;
	private DynamicTextLabel needle;
	private DynamicMetadataLabel regalia1;
	private DynamicMetadataLabel regalia2;
	private DynamicMetadataLabel seeming;
	private DynamicTextLabel thread;
	private FlowList touchstones;
	private TrackSimple wyrd;
	
	public override void _ExitTree()
	{
		clarity.ValueChanged -= changed_Clarity;
		contractsList.ValueChanged -= changed_Contracts;
		court.ItemSelected -= changed_Court;
		frailties.ValueChanged -= changed_Frailties;
		glamour.ValueChanged -= changed_Glamour;
		kith.ItemSelected -= changed_Kith;
		meritsFromMetadata.AddMerit -= addExistingMerit;
		needle.TextChanged -= changed_Needle;
		regalia1.ItemSelected -= changed_FavoredRegalia;
		regalia2.ItemSelected -= changed_FavoredRegalia;
		seeming.ItemSelected -= changed_Seeming;
		thread.TextChanged -= changed_Thread;
		touchstones.ValueChanged -= changed_Touchstones;
		wyrd.ValueChanged -= changed_Wyrd;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		SheetData ??= new Changeling();
		
		clarity = GetNode<TrackSimple>(NodePaths.Clarity);
		contractsList = GetNode<ContractsList>(NodePaths.ContractsList);
		court = GetNode<DynamicMetadataLabel>(NodePaths.Court);
		frailties = GetNode<FlowList>(NodePaths.Frailties);
		glamour = GetNode<TrackSimple>(NodePaths.Glamour);
		kith = GetNode<DynamicMetadataLabel>(NodePaths.Kith);
		meritsFromMetadata = GetNode<MeritsFromMetadata>(CoreSheet<Changeling>.NodePaths.MeritsFromMetadata);
		needle = GetNode<DynamicTextLabel>(NodePaths.Needle);
		regalia1 = GetNode<DynamicMetadataLabel>(NodePaths.Regalia1);
		regalia2 = GetNode<DynamicMetadataLabel>(NodePaths.Regalia2);
		seeming = GetNode<DynamicMetadataLabel>(NodePaths.Seeming);
		thread = GetNode<DynamicTextLabel>(NodePaths.Thread);
		touchstones = GetNode<FlowList>(NodePaths.Touchstones);
		wyrd = GetNode<TrackSimple>(NodePaths.Wyrd);
		
		InitTrackSimple(clarity, SheetData.Advantages.Integrity, changed_Clarity, DefaultIntegrityMax);
		InitTrackSimple(wyrd, SheetData.Advantages.Power, changed_Wyrd, DefaultIntegrityMax);
		InitTrackSimple(glamour, SheetData.Advantages.ResourceSpent, changed_Glamour, SheetData.Advantages.ResourceMax);
		
		Metadata courtValue = null;
		Metadata seemingValue = null;
		Metadata kithValue = null;
		
		if(metadataManager.Container is CofdChangelingContainer container)
		{
			if(container.Metadata.Where(m => m.Type == MetadataType.CofdChangelingCourt && m.Name == SheetData.Details.Faction).FirstOrDefault() is Metadata c)
				courtValue = c;
			
			if(container.Metadata.Where(m => m.Type == MetadataType.CofdChangelingSeeming && m.Name == SheetData.Details.TypePrimary).FirstOrDefault() is Metadata s)
				seemingValue = s;
			
			if(container.Metadata.Where(m => m.Type == MetadataType.CofdChangelingKith && m.Name == SheetData.Details.TypeSecondary).FirstOrDefault() is Metadata k)
				kithValue = k;
		}
		
		InitDynamicMetadataLabel(court, courtValue, changed_Court);
		InitFlowList(frailties, SheetData.Frailties, changed_Frailties);
		InitDynamicMetadataLabel(kith, kithValue, changed_Kith);
		InitDynamicTextLabel(needle, SheetData.Details.Virtue, changed_Needle);
		InitDynamicMetadataLabel(regalia1, SheetData.FavoredRegalia.Key, changed_FavoredRegalia);
		InitDynamicMetadataLabel(regalia2, SheetData.FavoredRegalia.Value, changed_FavoredRegalia);
		InitDynamicMetadataLabel(seeming, seemingValue, changed_Seeming);
		InitDynamicTextLabel(thread, SheetData.Details.Vice, changed_Thread);
		InitFlowList(touchstones, SheetData.Touchstones, changed_Touchstones);
		
		InitContractsList(contractsList, SheetData.Contracts, changed_Contracts);
		
		meritsFromMetadata.AddMerit += addExistingMerit;
		
		base._Ready();
		
		//Now that CofdCore sets a default maximum on attributes/skills we need to update after base._Ready()
		var dotMax = DefaultAttributeMax;
		if(SheetData.Advantages.Power > 5)
			dotMax = SheetData.Advantages.Power;
		
		foreach(var attribute in attributes)
		{
			if(SheetData.Attributes.Find(td => td.Name == attribute.Name) is TraitDots trait)
			{
				attribute.UpdateMax(dotMax);
				attribute.UpdateValue(trait.Value);
			}
		}
		
		foreach(var skill in skills)
		{
			if(SheetData.Skills.Find(td => td.Name == skill.Name) is TraitDots trait)
			{
				skill.UpdateMax(dotMax);
				skill.UpdateValue(trait.Value);
			}
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
			var r1 = regalia1.Value;
			var r2 = regalia2.Value;
			
			pair.Key = container.Metadata
				.Where(m => m.Type == MetadataType.CofdChangelingRegalia && m == r1)
				.FirstOrDefault();
			
			pair.Value = container.Metadata
				.Where(m => m.Type == MetadataType.CofdChangelingRegalia && m == r2)
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
