using Godot;
using System;

namespace OCSM
{
	public class TextValueItem : Godot.Object
	{
		public string Text { get; set; }
		public int Value { get; set; }
		
		public TextValueItem(string text, int value)
		{
			Text = text;
			Value = value;
		}

		public override string ToString()
		{
			return String.Format("{{ text: '{0}', value: {1} }}", Text, Value);
		}
	}
}
