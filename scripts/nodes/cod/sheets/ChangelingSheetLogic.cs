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
		public const string Integrity = "Integrity";
		public const string Vice = "Vice";
		public const string Virtue = "Virtue";
	}
	
	private sealed new class Detail : CoreSheetLogic<Mortal>.Detail
	{
		public const string Age = "Age";
		public const string Faction = "Faction";
		public const string GroupName = "GroupName";
		public const string Vice = "Vice";
		public const string Virtue = "Virtue";
	}
	
	public override void _Ready()
	{
		
	}
	
	protected new void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName, bool nodeChanged = false)
		where T1: Control
	{
		if(node is ContractsList cl)
		{
			cl.Values = initialValue as List<Contract>;
			cl.refresh();
			cl.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else
			base.InitAndConnect(node, initialValue, handlerName, nodeChanged);
	}
}
