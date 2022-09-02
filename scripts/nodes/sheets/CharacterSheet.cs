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
		
		public string GetJsonData() { return JsonSerializer.Serialize(SheetData); }
		
		public void SetJsonData(string json)
		{
			var data = JsonSerializer.Deserialize<T>(json);
			if(data is T typedData)
				SheetData = typedData;
		}
		
		protected virtual void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName)
			where T1: Control
		{
			if(node is LineEdit le)
			{
				le.Text = initialValue as string;
				le.Connect(Constants.Signal.TextChanged, this, handlerName);
			}
			else if(node is SpinBox sb)
			{
				if(initialValue is int number)
					sb.Value = number;
				sb.Connect(Constants.Signal.ValueChanged, this, handlerName);
			}
			else if(node is EntryList el)
			{
				if(initialValue is List<string> entries)
					el.Values = entries;
				el.refresh();
				el.Connect(nameof(EntryList.ValueChanged), this, handlerName);
			}
		}
	}
}
