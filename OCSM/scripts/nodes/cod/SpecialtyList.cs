using Godot;
using System;
using System.Collections.Generic;
using OCSM.CoD;

namespace OCSM.Nodes.CoD
{
	public partial class SpecialtyList : Container
	{
		
		[Signal]
		public delegate void ValueChangedEventHandler(Transport<List<Pair<string, string>>> values);
		
		public List<Pair<string, string>> Values { get; set; } = new List<Pair<string, string>>();
		
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
			
			Values.ForEach(s => {
				if(Skill.byName(s.Key) is Skill skill)
					addInput(skill, s.Value);
			});
			
			addInput();
		}
		
		private void skillChanged(long index) { updateValues(); }
		private void valueChanged(string text) { updateValues(); }
		
		private void updateValues()
		{
			var values = new List<Pair<string, string>>();
			var children = GetChildren();
			foreach(HBoxContainer row in children)
			{
				var optButton = row.GetChild<OptionButton>(0);
				var skill = Skill.byName(optButton.GetSelectedItemText());
				var value = row.GetChild<LineEdit>(1).Text;
				
				if(children.IndexOf(row) != children.Count - 1 && !(skill is Skill) && String.IsNullOrEmpty(value))
					row.QueueFree();
				else
				{
					var sp = new Pair<string, string>();
					if(skill is Skill)
						sp.Key = skill.Name;
					if(!String.IsNullOrEmpty(value))
						sp.Value = value;
					
					if(!sp.Empty)
						values.Add(sp);
				}
			}
			
			EmitSignal(nameof(ValueChanged), new Transport<List<Pair<string, string>>>(values));
			
			if(children.Count <= values.Count)
			{
				addInput();
			}
		}
		
		private void addInput(Skill skill = null, string specialty = "")
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.Specialty);
			var instance = resource.Instantiate<HBoxContainer>();
			AddChild(instance);
			
			var option = instance.GetChild<SkillOptionButton>(0);
			var value = instance.GetChild<LineEdit>(1);
			
			if(skill is Skill && !String.IsNullOrEmpty(specialty))
			{
				option.SelectItemByText(skill.Name);
				value.Text = specialty;
			}
			
			option.ItemSelected += skillChanged;
			value.TextChanged += valueChanged;
		}
	}
}
