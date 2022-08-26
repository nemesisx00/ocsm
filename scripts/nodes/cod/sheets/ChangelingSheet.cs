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
		public const string Needle = "Needle";
		public const string Thread = "Thread";
		public const string Wyrd = "Wyrd";
	}
	
	private sealed new class Detail : CoreSheet<Mortal>.Detail
	{
		public const string Seeming = "Seeming";
		public const string Kith = "Kith";
		public const string Court = "Court";
		public const string Needle = "Needle";
		public const string Thread = "Thread";
	}
	
	private const string ContractsList = "Contracts";
	
	
	public override void _Ready()
	{
		GD.Print("LOL");
		GD.Print(SheetData);
		if(!(SheetData is Changeling))
			SheetData = new Changeling();
		
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Clarity, AdvantagesPath)), SheetData.Clarity.ToString(), nameof(changed_Clarity));
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Wyrd, AdvantagesPath)), SheetData.Wyrd.ToString(), nameof(changed_Wyrd));
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Needle, AdvantagesPath)).Text = SheetData.Needle;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Thread, AdvantagesPath)).Text = SheetData.Thread;
		
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Seeming, DetailsPath)), SheetData.Seeming, nameof(changed_Seeming));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Kith, DetailsPath)), SheetData.Kith, nameof(changed_Kith));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Court, DetailsPath)), SheetData.Court, nameof(changed_Court));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Needle, DetailsPath)), SheetData.Needle, nameof(changed_Needle));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Thread, DetailsPath)), SheetData.Thread, nameof(changed_Thread));
		
		InitAndConnect(GetNode<ContractsList>(PathBuilder.SceneUnique(ContractsList)), SheetData.Contracts, nameof(changed_Contracts));
		
		base._Ready();
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
		else
			base.InitAndConnect(node, initialValue, handlerName, nodeChanged);
	}
	
	private void changed_Clarity(int value) { SheetData.Clarity = value; }
	private void changed_Contracts(SignalPayload<List<OCSM.Contract>> payload) { SheetData.Contracts = payload.Payload; }
	private void changed_Court(string value) { SheetData.Court = value; }
	private void changed_Kith(string value) { SheetData.Kith = value; }
	private void changed_Needle(string value) { SheetData.Needle = value; }
	private void changed_Seeming(string value) { SheetData.Seeming = value; }
	private void changed_Thread(string value) { SheetData.Thread = value; }
	private void changed_Wyrd(int value) { SheetData.Wyrd = value; }
}
