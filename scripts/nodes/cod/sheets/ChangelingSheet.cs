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
			if(initialValue is Court c && metadataManager.Container is CoDChangelingContainer ccc)
			{
				var index = ccc.Courts.FindIndex(r => r.Equals(c)) + 1;
				if(index > 0)
					cob.Selected = index;
			}
			cob.Connect(Constants.Signal.ItemSelected, this, handlerName);
		}
		else if(node is KithOptionButton kob)
		{
			if(initialValue is Kith k && metadataManager.Container is CoDChangelingContainer ccc)
			{
				var index = ccc.Courts.FindIndex(r => r.Equals(k)) + 1;
				if(index > 0)
					kob.Selected = index;
			}
			kob.Connect(Constants.Signal.ItemSelected, this, handlerName);
		}
		else if(node is RegaliaOptionButton rob)
		{
			if(initialValue is Court c && metadataManager.Container is CoDChangelingContainer ccc)
			{
				var index = ccc.Courts.FindIndex(r => r.Equals(c)) + 1;
				if(index > 0)
					rob.Selected = index;
			}
			rob.Connect(Constants.Signal.ItemSelected, this, handlerName);
		}
		else if(node is SeemingOptionButton sob)
		{
			if(initialValue is Seeming s && metadataManager.Container is CoDChangelingContainer ccc)
			{
				var index = ccc.Courts.FindIndex(r => r.Equals(s)) + 1;
				if(index > 0)
					sob.Selected = index;
			}
			sob.Connect(Constants.Signal.ItemSelected, this, handlerName);
		}
		else
			base.InitAndConnect(node, initialValue, handlerName, nodeChanged);
	}
	
	private void changed_Clarity(int value) { SheetData.Clarity = value; }
	private void changed_Contracts(SignalPayload<List<OCSM.CoD.CtL.Contract>> payload) { SheetData.Contracts = payload.Payload; }
	
	private void changed_Court(int item)
	{
		if(item > 0 && metadataManager.Container is CoDChangelingContainer ccc)
			SheetData.Court = ccc.Courts[item - 1];
		else if(item > 0)
			SheetData.Court = null;
	}
	
	private void changed_Frailties(List<string> values) { SheetData.Frailties = values; }
	private void changed_Glamour(int value) { SheetData.GlamourSpent = value; }
	
	private void changed_Kith(int item)
	{
		if(item > 0 && metadataManager.Container is CoDChangelingContainer ccc)
			SheetData.Kith = ccc.Kiths[item - 1];
		else if(item > 0)
			SheetData.Kith = null;
	}
	
	private void changed_Needle(string value) { SheetData.Needle = value; }
	private void changed_FavoredRegalia(int item)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			var regalia = new List<Regalia>(2);
			
			var r1 = GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Detail.Regalia1, DetailsPath));
			if(r1.Selected > 0)
				regalia.Add(ccc.Regalias[r1.Selected - 1]);
			
			var r2 = GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Detail.Regalia2, DetailsPath));
			if(r2.Selected > 0)
				regalia.Add(ccc.Regalias[r2.Selected - 1]);
			
			SheetData.FavoredRegalia = regalia;
		}
	}
	
	private void changed_Seeming(int item)
	{
		if(item > 0 && metadataManager.Container is CoDChangelingContainer ccc)
			SheetData.Seeming = ccc.Seemings[item - 1];
		else if(item > 0)
			SheetData.Seeming = null;
	}
	
	private void changed_Thread(string value) { SheetData.Thread = value; }
	private void changed_Touchstones(List<string> values) { SheetData.Touchstones = values; }
	private void changed_Wyrd(int value) { SheetData.Wyrd = value; }
}
