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
	
	protected void InitEntryList(EntryList node, List<string> initialValue, EntryList.ValueChangedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.Values = initialValue;
			node.Refresh();
			node.ValueChanged += handler;
		}
	}
	
	protected void InitLineEdit(LineEdit node, string initialValue, LineEdit.TextChangedEventHandler handler)
	{
		if(node is not null)
		{
			node.Text = initialValue;
			node.TextChanged += handler;
		}
	}
	
	protected void InitTextEdit(TextEdit node, string initialValue, System.Action handler)
	{
		if(node is not null)
		{
			node.Text = initialValue;
			node.TextChanged += handler;
		}
	}
	
	protected void InitToggleButton(ToggleButton node, bool initialValue, ToggleButton.StateToggledEventHandler handler)
	{
		if(node is not null)
		{
			node.CurrentState = initialValue;
			node.UpdateTexture();
			node.StateToggled += handler;
		}
	}
	
	protected void InitTrackComplex(TrackComplex node, Dictionary<StatefulButton.States, int> initialValue, TrackComplex.ValueChangedEventHandler handler, int initialMax = TrackComplex.DefaultMax)
	{
		if(node is not null)
		{
			node.UpdateMax(initialMax > 1 ? initialMax : TrackComplex.DefaultMax);
			if(initialValue is not null)
				node.Values = initialValue;
			
			node.ValueChanged += handler;
		}
	}
	
	protected void InitTrackSimple(TrackSimple node, int initialValue, TrackSimple.NodeChangedEventHandler handler, int initialMax = TrackSimple.DefaultMax)
	{
		if(node is not null)
		{
			node.UpdateMax(initialMax > 1 ? initialMax : TrackSimple.DefaultMax);
			node.UpdateValue(initialValue > 0 ? initialValue : 0);
			node.NodeChanged += handler;
		}
	}
	
	protected void InitTrackSimple(TrackSimple node, int initialValue, TrackSimple.ValueChangedEventHandler handler, int initialMax = TrackSimple.DefaultMax)
	{
		if(node is not null)
		{
			node.UpdateMax(initialMax > 1 ? initialMax : TrackSimple.DefaultMax);
			node.UpdateValue(initialValue > 0 ? initialValue : 0);
			node.ValueChanged += handler;
		}
	}
	
	protected void InitSpinBox(SpinBox node, long initialValue, Range.ValueChangedEventHandler handler)
	{
		if(node is not null)
		{
			node.Value = initialValue;
			node.ValueChanged += handler;
		}
	}
}
