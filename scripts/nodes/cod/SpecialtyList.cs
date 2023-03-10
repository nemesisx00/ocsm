using Godot;
using System;
using System.Collections.Generic;
using OCSM.CoD;

namespace OCSM.Nodes.CoD
{
	public partial class SpecialtyList : Container
	{
		
		[Signal]
		public delegate void ValueChangedEventHandler(Transport<List<Specialty>> values);
		
		public List<Specialty> Values { get; set; } = new List<Specialty>();
		
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
			
			foreach(var specialty in Values)
			{
				var skill = Skill.byName(specialty.Skill);
				if(skill is Skill)
					addInput(skill, specialty.Value);
			}
			
			addInput();
		}
		
		private void skillChanged(long index) { updateValues(); }
		private void valueChanged(string text) { updateValues(); }
		
		private void updateValues()
		{
			var values = new List<Specialty>();
			var children = GetChildren();
			foreach(HBoxContainer row in children)
			{
				var optButton = row.GetChild<OptionButton>(0);
				var skill = Skill.byName(optButton.GetItemText(optButton.Selected));
				var value = row.GetChild<LineEdit>(1).Text;
				
				if(children.IndexOf(row) != children.Count - 1 && !(skill is Skill) && String.IsNullOrEmpty(value))
					row.QueueFree();
				else
				{
					var sp = new Specialty();
					if(skill is Skill)
						sp.Skill = skill.Name;
					if(!String.IsNullOrEmpty(value))
						sp.Value = value;
					
					if(!sp.Empty)
						values.Add(sp);
				}
			}
			
			EmitSignal(nameof(ValueChanged), new Transport<List<Specialty>>(values));
			
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
				option.Selected = Skill.asList().FindIndex(s => s.Equals(skill)) + 1;
				value.Text = specialty;
			}
			
			option.ItemSelected += skillChanged;
			value.TextChanged += valueChanged;
		}
	}
}
