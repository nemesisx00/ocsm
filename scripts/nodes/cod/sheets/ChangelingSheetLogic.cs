using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using OCSM;

public class ChangelingSheetLogic : CoreSheetLogic<Changeling>
{
	private sealed new class Advantage : CoreSheetLogic<Mortal>.Advantage
	{
		public const string Clarity = "Clarity";
		public const string Needle = "Needle";
		public const string Thread = "Thread";
		public const string Wyrd = "Wyrd";
	}
	
	private sealed new class Detail : CoreSheetLogic<Mortal>.Detail
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
		sheetData = new Changeling();
		
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Clarity, AdvantagesPath)), sheetData.Clarity.ToString(), nameof(changed_Clarity));
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Wyrd, AdvantagesPath)), sheetData.Wyrd.ToString(), nameof(changed_Wyrd));
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Needle, AdvantagesPath)).Text = sheetData.Needle;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Thread, AdvantagesPath)).Text = sheetData.Thread;
		
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Seeming, DetailsPath)), sheetData.Seeming, nameof(changed_Seeming));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Kith, DetailsPath)), sheetData.Kith, nameof(changed_Kith));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Court, DetailsPath)), sheetData.Court, nameof(changed_Court));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Needle, DetailsPath)), sheetData.Needle, nameof(changed_Needle));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Thread, DetailsPath)), sheetData.Thread, nameof(changed_Thread));
		
		InitAndConnect(GetNode<ContractsList>(PathBuilder.SceneUnique(ContractsList)), sheetData.Contracts, nameof(changed_Contracts));
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
	
	private void changed_Clarity(int value) { sheetData.Clarity = value; }
	private void changed_Contracts(SignalPayload<List<OCSM.Contract>> payload) { sheetData.Contracts = payload.Payload; }
	private void changed_Court(string value) { sheetData.Court = value; }
	private void changed_Kith(string value) { sheetData.Kith = value; }
	private void changed_Needle(string value) { sheetData.Needle = value; }
	private void changed_Seeming(string value) { sheetData.Seeming = value; }
	private void changed_Thread(string value) { sheetData.Thread = value; }
	private void changed_Wyrd(int value) { sheetData.Wyrd = value; }
}
