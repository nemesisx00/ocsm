using Godot;
using System;
using System.Collections.Generic;
using OCSM;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;

public class ChangelingSheet : CoreSheet<Changeling>, ICharacterSheet
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
		
		InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Clarity, AdvantagesPath)), SheetData.Clarity, nameof(changed_Clarity));
		InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Wyrd, AdvantagesPath)), SheetData.Wyrd, nameof(changed_Wyrd));
		InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Glamour, AdvantagesPath)), SheetData.GlamourSpent, nameof(changed_Glamour));
		GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Needle, AdvantagesPath)).Text = SheetData.Needle;
		GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Thread, AdvantagesPath)).Text = SheetData.Thread;
		
		InitAndConnect(GetNode<CourtOptionButton>(NodePathBuilder.SceneUnique(Detail.Court, DetailsPath)), SheetData.Court, nameof(changed_Court));
		InitAndConnect(GetNode<ItemList>(NodePathBuilder.SceneUnique(Detail.Frailties, DetailsPath)), SheetData.Frailties, nameof(changed_Frailties));
		InitAndConnect(GetNode<KithOptionButton>(NodePathBuilder.SceneUnique(Detail.Kith, DetailsPath)), SheetData.Kith, nameof(changed_Kith));
		InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Needle, DetailsPath)), SheetData.Needle, nameof(changed_Needle));
		InitAndConnect(GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Detail.Regalia1, DetailsPath)), SheetData.FavoredRegalia.Count > 0 ? SheetData.FavoredRegalia[0] : null, nameof(changed_FavoredRegalia));
		InitAndConnect(GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Detail.Regalia2, DetailsPath)), SheetData.FavoredRegalia.Count > 1 ? SheetData.FavoredRegalia[1] : null, nameof(changed_FavoredRegalia));
		InitAndConnect(GetNode<SeemingOptionButton>(NodePathBuilder.SceneUnique(Detail.Seeming, DetailsPath)), SheetData.Seeming, nameof(changed_Seeming));
		InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Thread, DetailsPath)), SheetData.Thread, nameof(changed_Thread));
		InitAndConnect(GetNode<ItemList>(NodePathBuilder.SceneUnique(Detail.Touchstones, DetailsPath)), SheetData.Touchstones, nameof(changed_Touchstones));
		
		InitAndConnect(GetNode<ContractsList>(NodePathBuilder.SceneUnique(ContractsListName)), SheetData.Contracts, nameof(changed_Contracts));
		
		GetNode<MeritsFromMetadata>(NodePathBuilder.SceneUnique(MeritsFromMetadataName)).Connect(nameof(MeritsFromMetadata.AddMerit), this, nameof(addExistingMerit));
		
		base._Ready();
		
		NodeUtilities.autoSizeChildren(this, Constants.TextInputMinHeight);
	}
	
	protected new void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName, bool nodeChanged = false)
		where T1: Control
	{
		if(node is ContractsList cl)
		{
			cl.Values = initialValue as List<OCSM.CoD.CtL.Contract>;
			cl.refresh();
			cl.Connect(nameof(ContractsList.ValueChanged), this, handlerName);
		}
		else if(node is CourtOptionButton cob)
		{
			if(initialValue is Court court && metadataManager.Container is CoDChangelingContainer ccc)
			{
				var index = ccc.Courts.FindIndex(c => c.Equals(court)) + 1;
				if(index > 0)
					cob.Selected = index;
			}
			cob.Connect(Constants.Signal.ItemSelected, this, handlerName);
		}
		else if(node is KithOptionButton kob)
		{
			if(initialValue is Kith kith && metadataManager.Container is CoDChangelingContainer ccc)
			{
				var index = ccc.Kiths.FindIndex(k => k.Equals(kith)) + 1;
				if(index > 0)
					kob.Selected = index;
			}
			kob.Connect(Constants.Signal.ItemSelected, this, handlerName);
		}
		else if(node is RegaliaOptionButton rob)
		{
			if(initialValue is Regalia regalia && metadataManager.Container is CoDChangelingContainer ccc)
			{
				var index = ccc.Regalias.FindIndex(r => r.Equals(regalia)) + 1;
				if(index > 0)
					rob.Selected = index;
			}
			rob.Connect(Constants.Signal.ItemSelected, this, handlerName);
		}
		else if(node is SeemingOptionButton sob)
		{
			if(initialValue is Seeming seeming && metadataManager.Container is CoDChangelingContainer ccc)
			{
				var index = ccc.Seemings.FindIndex(s => s.Equals(seeming)) + 1;
				if(index > 0)
					sob.Selected = index;
			}
			sob.Connect(Constants.Signal.ItemSelected, this, handlerName);
		}
		else
			base.InitAndConnect(node, initialValue, handlerName, nodeChanged);
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
	
	private void changed_Clarity(int value) { SheetData.Clarity = value; }
	private void changed_Contracts(List<Transport<OCSM.CoD.CtL.Contract>> values)
	{
		var list = new List<OCSM.CoD.CtL.Contract>();
		foreach(var t in values)
		{
			list.Add(t.Value);
		}
		SheetData.Contracts = list;
	}
	
	private void changed_Court(int index)
	{
		if(index > 0 && metadataManager.Container is CoDChangelingContainer ccc && ccc.Courts[index - 1] is Court court)
			SheetData.Court = court;
		else
			SheetData.Court = null;
	}
	
	private void changed_Frailties(List<string> values) { SheetData.Frailties = values; }
	private void changed_Glamour(int value) { SheetData.GlamourSpent = value; }
	
	private void changed_Kith(int index)
	{
		if(index > 0 && metadataManager.Container is CoDChangelingContainer ccc && ccc.Kiths[index - 1] is Kith kith)
			SheetData.Kith = kith;
		else
			SheetData.Kith = null;
	}
	
	private void changed_Needle(string value) { SheetData.Needle = value; }
	private void changed_FavoredRegalia(int item)
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
	
	private void changed_Seeming(int index)
	{
		if(index > 0 && metadataManager.Container is CoDChangelingContainer ccc && ccc.Seemings[index - 1] is Seeming seeming)
			SheetData.Seeming = seeming;
		else
			SheetData.Seeming = null;
	}
	
	private void changed_Thread(string value) { SheetData.Thread = value; }
	private void changed_Touchstones(List<string> values) { SheetData.Touchstones = values; }
	private void changed_Wyrd(int value) { SheetData.Wyrd = value; }
}
