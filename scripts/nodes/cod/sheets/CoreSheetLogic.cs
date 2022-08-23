using Godot;
using System.Collections.Generic;
using OCSM;

public class CoreSheetLogic : Container
{
	protected void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName, bool nodeChanged = false)
		where T1: Control
	{
		if(node is LineEdit le)
		{
			le.Text = initialValue as string;
			le.Connect(Constants.Signal.TextChanged, this, handlerName);
		}
		else if(node is TrackSimple ts)
		{
			var val = 0;
			if(initialValue is int)
				val = int.Parse(initialValue.ToString());
			if(val > 0)
				ts.updateValue(val);
			
			if(nodeChanged)
				ts.Connect(Constants.Signal.NodeChanged, this, handlerName);
			else
				ts.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else if(node is TrackComplex tc)
		{
			tc.Values = initialValue as Dictionary<string, int>;
			tc.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else if(node is ItemList il)
		{
			il.Values = initialValue as List<string>;
			il.refresh();
			il.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else if(node is ItemDotsList idl)
		{
			idl.Values = initialValue as List<TextValueItem>;
			idl.refresh();
			idl.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else if(node is SpecialtyList sl)
		{
			sl.Values = initialValue as List<Skill.Specialty>;
			sl.refresh();
			sl.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
	}
}
