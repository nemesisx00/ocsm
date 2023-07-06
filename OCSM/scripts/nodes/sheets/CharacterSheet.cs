using Godot;
using System.Collections.Generic;
using System.Text.Json;

namespace Ocsm.Nodes.Sheets
{
	public interface ICharacterSheet
	{
		string GetJsonData();
		void SetJsonData(string json);
	}

	public abstract partial class CharacterSheet<T> : Container, ICharacterSheet
		where T: Character
	{
		protected virtual T SheetData { get; set; }
		
		public override void _Ready()
		{
		}
		
		public string GetJsonData() { return JsonSerializer.Serialize(SheetData); }
		
		public void SetJsonData(string json)
		{
			var data = JsonSerializer.Deserialize<T>(json);
			if(data is T typedData)
				SheetData = typedData;
		}
		
		protected void InitEntryList(EntryList node, List<string> initialValue, EntryList.ValueChangedEventHandler handler)
		{
			if(node is EntryList)
			{
				if(initialValue is List<string>)
					node.Values = initialValue;
				node.refresh();
				node.ValueChanged += handler;
			}
		}
		
		protected void InitLineEdit(LineEdit node, string initialValue, LineEdit.TextChangedEventHandler handler)
		{
			if(node is LineEdit)
			{
				node.Text = initialValue;
				node.TextChanged += handler;
			}
		}
		
		protected void InitTextEdit(TextEdit node, string initialValue, System.Action handler)
		{
			if(node is TextEdit)
			{
				node.Text = initialValue;
				node.TextChanged += handler;
			}
		}
		
		protected void InitToggleButton(ToggleButton node, bool initialValue, ToggleButton.StateToggledEventHandler handler)
		{
			if(node is ToggleButton)
			{
				node.CurrentState = initialValue;
				node.updateTexture();
				node.StateToggled += handler;
			}
		}
		
		protected void InitTrackComplex(TrackComplex node, Dictionary<string, long> initialValue, TrackComplex.ValueChangedEventHandler handler, long initialMax = TrackComplex.DefaultMax)
		{
			if(node is TrackComplex)
			{
				node.updateMax(initialMax > 1 ? initialMax : TrackComplex.DefaultMax);
				if(initialValue is Dictionary<string, long>)
					node.Values = initialValue;
				node.ValueChanged += handler;
			}
		}
		
		protected void InitTrackSimple(TrackSimple node, long initialValue, TrackSimple.NodeChangedEventHandler handler, long initialMax = TrackSimple.DefaultMax)
		{
			if(node is TrackSimple)
			{
				node.updateMax(initialMax > 1 ? initialMax : TrackSimple.DefaultMax);
				node.updateValue(initialValue > 0 ? initialValue : 0);
				node.NodeChanged += handler;
			}
		}
		
		protected void InitTrackSimple(TrackSimple node, long initialValue, TrackSimple.ValueChangedEventHandler handler, long initialMax = TrackSimple.DefaultMax)
		{
			if(node is TrackSimple)
			{
				node.updateMax(initialMax > 1 ? initialMax : TrackSimple.DefaultMax);
				node.updateValue(initialValue > 0 ? initialValue : 0);
				node.ValueChanged += handler;
			}
		}
		
		protected void InitSpinBox(SpinBox node, long initialValue, SpinBox.ValueChangedEventHandler handler)
		{
			if(node is SpinBox)
			{
				node.Value = initialValue;
				node.ValueChanged += handler;
			}
		}
	}
}
