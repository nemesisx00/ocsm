using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using OCSM;

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
	
	private const string ContractsList = "Contracts";
	
	
	public override void _Ready()
	{
		if(!(SheetData is Changeling))
			SheetData = new Changeling();
		
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Clarity, AdvantagesPath)), SheetData.Clarity, nameof(changed_Clarity));
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Wyrd, AdvantagesPath)), SheetData.Wyrd, nameof(changed_Wyrd));
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Glamour, AdvantagesPath)), SheetData.GlamourSpent, nameof(changed_Glamour));
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Needle, AdvantagesPath)).Text = SheetData.Needle;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Thread, AdvantagesPath)).Text = SheetData.Thread;
		
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Court, DetailsPath)), SheetData.Court, nameof(changed_Court));
		InitAndConnect(GetNode<ItemList>(PathBuilder.SceneUnique(Detail.Frailties, DetailsPath)), SheetData.Frailties, nameof(changed_Frailties));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Kith, DetailsPath)), SheetData.Kith, nameof(changed_Kith));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Needle, DetailsPath)), SheetData.Needle, nameof(changed_Needle));
		InitAndConnect(GetNode<RegaliaOptionButton>(PathBuilder.SceneUnique(Detail.Regalia1, DetailsPath)), SheetData.FavoredRegalia.Count > 0 ? SheetData.FavoredRegalia[0] : null, nameof(changed_FavoredRegalia));
		InitAndConnect(GetNode<RegaliaOptionButton>(PathBuilder.SceneUnique(Detail.Regalia2, DetailsPath)), SheetData.FavoredRegalia.Count > 1 ? SheetData.FavoredRegalia[1] : null, nameof(changed_FavoredRegalia));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Seeming, DetailsPath)), SheetData.Seeming, nameof(changed_Seeming));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Thread, DetailsPath)), SheetData.Thread, nameof(changed_Thread));
		InitAndConnect(GetNode<ItemList>(PathBuilder.SceneUnique(Detail.Touchstones, DetailsPath)), SheetData.Touchstones, nameof(changed_Touchstones));
		
		InitAndConnect(GetNode<ContractsList>(PathBuilder.SceneUnique(ContractsList)), SheetData.Contracts, nameof(changed_Contracts));
		
		base._Ready();
		
		NodeUtilities.autoSizeChildren(this, Constants.TextInputMinHeight);
	}
	
	protected new void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName, bool nodeChanged = false)
		where T1: Control
	{
		if(node is ContractsList cl)
		{
			cl.Values = initialValue as List<OCSM.Contract>;
			cl.refresh();
			cl.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else if(node is RegaliaOptionButton rob)
		{
			rob.Selected = Regalia.asList().FindIndex(r => r.Equals(initialValue as string)) + 1;
			rob.Connect(Constants.Signal.ItemSelected, this, handlerName);
		}
		else
			base.InitAndConnect(node, initialValue, handlerName, nodeChanged);
	}
	
	private void changed_Clarity(int value) { SheetData.Clarity = value; }
	private void changed_Contracts(SignalPayload<List<OCSM.Contract>> payload) { SheetData.Contracts = payload.Payload; }
	private void changed_Court(string value) { SheetData.Court = value; }
	private void changed_Frailties(List<string> values) { SheetData.Frailties = values; }
	private void changed_Glamour(int value) { SheetData.GlamourSpent = value; }
	private void changed_Kith(string value) { SheetData.Kith = value; }
	private void changed_Needle(string value) { SheetData.Needle = value; }
	private void changed_FavoredRegalia(int item)
	{
		var regalia = new List<string>(2);
		
		var r1 = GetNode<RegaliaOptionButton>(PathBuilder.SceneUnique(Detail.Regalia1, DetailsPath));
		var text = r1.GetItemText(r1.Selected);
		if(!String.IsNullOrEmpty(text))
			regalia.Add(text);
		
		var r2 = GetNode<RegaliaOptionButton>(PathBuilder.SceneUnique(Detail.Regalia2, DetailsPath));
		text = r2.GetItemText(r2.Selected);
		if(!String.IsNullOrEmpty(text))
			regalia.Add(text);
		
		SheetData.FavoredRegalia = regalia;
	}
	private void changed_Seeming(string value) { SheetData.Seeming = value; }
	private void changed_Thread(string value) { SheetData.Thread = value; }
	private void changed_Touchstones(List<string> values) { SheetData.Touchstones = values; }
	private void changed_Wyrd(int value) { SheetData.Wyrd = value; }
}
