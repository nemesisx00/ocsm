using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using OCSM.CoD;

namespace OCSM.Nodes.CoD
{
	public partial class SpecialtyList : Container
	{
		
		[Signal]
		public delegate void ValueChangedEventHandler(Transport<Dictionary<Skill.Enum, string>> values);
		
		public Dictionary<Skill.Enum, string> Values { get; set; } = new Dictionary<Skill.Enum, string>();
		
		public override void _Ready()
		{
			refresh();
		}
		
		public void refresh()
		{
			foreach(Node c in GetChildren())
			{
				c.QueueFree();
			}
			
			Values.ToList().ForEach(entry => {
				if(entry.Key is Skill.Enum skill)
					addInput(skill, entry.Value);
			});
			
			addInput();
		}
		
		private void skillChanged(long index) { updateValues(); }
		private void valueChanged(string text) { updateValues(); }
		
		private void updateValues()
		{
			var values = new Dictionary<Skill.Enum, string>();
			var list = GetChildren().Select(n => n as HBoxContainer)
				.Select(row => new { option = row.GetChild<OptionButton>(0), skill = Skill.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()), text = row.GetChild<LineEdit>(1).Text })
				.Where(o => o.skill is Skill.Enum)
				.OrderBy(o => o.skill)
				.ThenBy(o => o.text)
				.ToList();
			
			// Update the values
			list.ForEach(o => {
					if(o.skill is Skill.Enum s)
					{
						if(values.ContainsKey(s))
							values[s] = o.text;
						else
							values.Add(s, o.text);
					}
				});
			
			Values = values;
			EmitSignal(nameof(ValueChanged), new Transport<Dictionary<Skill.Enum, string>>(values));
			
			// Update the option buttons' disabled items
			list.ForEach(o => {
					o.option.SetDisabledAll(false);
					Values.Keys.ToList()
						.ForEach(sk => o.option.SetDisabledByText(sk.GetLabelOrName(), true));
					
					if(o.skill is Skill.Enum s)
						o.option.SetDisabledByText(s.GetLabelOrName(), false);
				});
			
			// Clean up excess empties
			GetChildren().Select(n => n as HBoxContainer)
				.Where(row => Skill.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()) is null
								&& String.IsNullOrEmpty(row.GetChild<LineEdit>(1).Text))
				.ToList()
				.ForEach(row => row.QueueFree());
			
			NodeUtilities.rearrangeNodes(this, GetChildren()
												.OrderBy(row => Skill.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()))
												.ToList());
			
			addInput();
		}
		
		private void addInput(Skill.Enum? skill = null, string specialty = "")
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.Specialty);
			var instance = resource.Instantiate<HBoxContainer>();
			AddChild(instance);
			
			var option = instance.GetChild<SkillOptionButton>(0);
			var value = instance.GetChild<LineEdit>(1);
			
			Values.Keys.ToList()
				.ForEach(s => option.SetDisabledByText(s.GetLabelOrName(), true));
			
			if(skill is Skill.Enum s && !String.IsNullOrEmpty(specialty))
			{
				var text = s.GetLabelOrName();
				option.SetDisabledByText(text, false);
				option.SelectItemByText(text);
				value.Text = specialty;
			}
			
			option.ItemSelected += skillChanged;
			value.TextChanged += valueChanged;
		}
	}
}
