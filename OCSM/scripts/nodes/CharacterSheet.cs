using Godot;
using System.Collections.Generic;
using System.Text.Json;

namespace Ocsm.Nodes;

public abstract partial class CharacterSheet<T> : Container, ICharacterSheet
	where T: Character
{
	protected virtual T SheetData { get; set; }
	
	public string GetJsonData() => JsonSerializer.Serialize(SheetData);
	
	public void SetJsonData(string json)
	{
		var data = JsonSerializer.Deserialize<T>(json);
		
		if(data is T typedData)
			SheetData = typedData;
	}
	
	protected static void InitEntryList(EntryList node, List<string> initialValue, EntryList.ValueChangedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.Values = initialValue;
			
			node.Refresh();
			node.ValueChanged += handler;
		}
	}
	
	protected static void InitLineEdit(LineEdit node, string initialValue, LineEdit.TextChangedEventHandler handler)
	{
		if(node is not null)
		{
			node.Text = initialValue;
			node.TextChanged += handler;
		}
	}
	
	protected static void InitTextEdit(TextEdit node, string initialValue, System.Action handler)
	{
		if(node is not null)
		{
			node.Text = initialValue;
			node.TextChanged += handler;
		}
	}
	
	protected static void InitToggleButton(ToggleButton node, bool initialValue, ToggleButton.ToggleEventHandler handler)
	{
		if(node is not null)
		{
			node.State = initialValue;
			node.Toggle += handler;
		}
	}
	
	protected static void InitTrackComplex(TrackComplex node, Dictionary<StatefulButton.States, int> initialValue, TrackComplex.ValueChangedEventHandler handler, int initialMax = TrackComplex.DefaultMax)
	{
		if(node is not null)
		{
			node.UpdateMax(initialMax > 1 ? initialMax : TrackComplex.DefaultMax);
			if(initialValue is not null)
				node.Values = initialValue;
			node.ValueChanged += handler;
		}
	}
	
	protected static void InitTrackSimple(TrackSimple node, int initialValue, TrackSimple.ValueChangedEventHandler handler, int initialMax = TrackSimple.DefaultMax)
	{
		if(node is not null)
		{
			node.Max = initialMax > 1 ? initialMax : TrackSimple.DefaultMax;
			node.Value = initialValue > 0 ? initialValue : 0;
			node.ValueChanged += handler;
		}
	}
	
	protected static void InitSpinBox(SpinBox node, int initialValue, Range.ValueChangedEventHandler handler)
	{
		if(node is not null)
		{
			node.Value = initialValue;
			node.ValueChanged += handler;
		}
	}
}
