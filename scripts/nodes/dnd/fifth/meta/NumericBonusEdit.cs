using Godot;
using System;
using OCSM.DnD.Fifth;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public class NumericBonusEdit : Container
	{
		private sealed class Names
		{
			public const string Ability = "Ability";
			public const string AbilityLabel = "AbilityLabel";
			public const string Method = "Method";
			public const string Name = "Name";
			public const string Type = "Type";
			public const string Value = "Value";
		}
		
		[Signal]
		public delegate void ValueChanged(Transport<NumericBonus> transport);
		
		private AbilityOptionsButton abilityInput;
		private Label abilityLabel;
		private OptionButton methodInput;
		private LineEdit nameInput;
		private SpinBox valueInput;
		private NumericStatOptionsButton typeInput;
		
		public NumericBonus Value { get; set; }
		
		public override void _Ready()
		{
			if(!(Value is NumericBonus))
				Value = new NumericBonus();
			
			abilityInput = GetNode<AbilityOptionsButton>(NodePathBuilder.SceneUnique(Names.Ability));
			abilityLabel = GetNode<Label>(NodePathBuilder.SceneUnique(Names.AbilityLabel));
			methodInput = GetNode<OptionButton>(NodePathBuilder.SceneUnique(Names.Method));
			nameInput = GetNode<LineEdit>(NodePathBuilder.SceneUnique(Names.Name));
			typeInput = GetNode<NumericStatOptionsButton>(NodePathBuilder.SceneUnique(Names.Type));
			valueInput = GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Value));
			
			abilityInput.Connect(Constants.Signal.ItemSelected, this, nameof(abilityChanged));
			methodInput.Connect(Constants.Signal.ItemSelected, this, nameof(methodChanged));
			nameInput.Connect(Constants.Signal.TextChanged, this, nameof(nameChanged));
			typeInput.Connect(Constants.Signal.ItemSelected, this, nameof(typeChanged));
			valueInput.Connect(Constants.Signal.ValueChanged, this, nameof(valueChanged));
		}
		
		public void setValue(NumericBonus numericBonus)
		{
			Value = numericBonus;
			
			abilityInput.Selected = Ability.Names.asList().IndexOf(Value.AbilityName) + 1;
			methodInput.Selected = Value.Add ? 1 : 0;
			nameInput.Text = Value.Name;
			typeInput.Selected = (int)Value.Type;
			valueInput.Value = Value.Value;
			
			toggleAbilityNodes();
		}
		
		private void doEmitSignal()
		{
			EmitSignal(nameof(ValueChanged), new Transport<NumericBonus>(Value));
		}
		
		private void toggleAbilityNodes()
		{
			if(Value.Type.Equals(NumericStat.AbilityScore))
			{
				abilityLabel.Show();
				abilityInput.Show();
			}
			else
			{
				abilityLabel.Hide();
				abilityInput.Hide();
			}
		}
		
		private void abilityChanged(int index)
		{
			Value.AbilityName = abilityInput.GetItemText(index);
			doEmitSignal();
		}
		
		private void methodChanged(int index)
		{
			Value.Add = index > 0;
			doEmitSignal();
		}
		
		private void nameChanged(string text)
		{
			Value.Name = text;
			doEmitSignal();
		}
		
		private void typeChanged(int index)
		{
			Value.Type = (NumericStat)index;
			if(!Value.Type.Equals(NumericStat.AbilityScore))
			{
				Value.AbilityName = String.Empty;
				abilityInput.Selected = 0;
			}
			toggleAbilityNodes();
			doEmitSignal();
		}
		
		private void valueChanged(float value)
		{
			Value.Value = (int)value;
			doEmitSignal();
		}
	}
}
