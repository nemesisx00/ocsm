using Godot;
using System.Collections.Generic;
using System.Text.Json;

namespace OCSM.Nodes.Sheets
{
	public interface ICharacterSheet
	{
		string GetJsonData();
		void SetJsonData(string json);
	}

	public abstract class CharacterSheet<T> : Container, ICharacterSheet
		where T: Character
	{
		protected virtual T SheetData { get; set; }
		
		public override void _Ready()
		{
			NodeUtilities.autoSizeChildren(this, Constants.TextInputMinHeight);
		}
		
		public string GetJsonData() { return JsonSerializer.Serialize(SheetData); }
		
		public void SetJsonData(string json)
		{
			var data = JsonSerializer.Deserialize<T>(json);
			if(data is T typedData)
				SheetData = typedData;
		}
		
		protected virtual void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName, bool nodeChanged = false)
			where T1: Control
		{
			if(node is EntryList el)
			{
				if(initialValue is List<string> entries)
					el.Values = entries;
				el.refresh();
				el.Connect(nameof(EntryList.ValueChanged), this, handlerName);
			}
			else if(node is LineEdit le)
			{
				le.Text = initialValue as string;
				le.Connect(Constants.Signal.TextChanged, this, handlerName);
			}
			else if(node is TextEdit te)
			{
				te.Text = initialValue as string;
				te.Connect(Constants.Signal.TextChanged, this, handlerName);
			}
			else if(node is ToggleButton tb)
			{
				if(initialValue is bool state)
				{
					tb.CurrentState = state;
					tb.updateTexture();
				}
				tb.Connect(nameof(ToggleButton.StateToggled), this, handlerName);
			}
			else if(node is TrackSimple ts)
			{
				if(initialValue is int number)
					ts.updateValue(number > 0 ? number : 0);
				
				if(nodeChanged)
					ts.Connect(Constants.Signal.NodeChanged, this, handlerName);
				else
					ts.Connect(nameof(TrackSimple.ValueChanged), this, handlerName);
			}
			else if(node is TrackComplex tc)
			{
				if(initialValue is Dictionary<string, int> entries)
					tc.Values = entries;
				tc.Connect(nameof(TrackComplex.ValueChanged), this, handlerName);
			}
			else if(node is SpinBox sb)
			{
				if(initialValue is int number)
					sb.Value = number;
				sb.Connect(Constants.Signal.ValueChanged, this, handlerName);
			}
		}
	}
}
