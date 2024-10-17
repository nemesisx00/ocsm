using Godot;
using System.Linq;

namespace Ocsm.Dnd.Fifth.Nodes.Meta;

public partial class NumericBonusEdit : Container
{
	private static class NodePaths
	{
		public static readonly NodePath Ability = new("%Ability");
		public static readonly NodePath AbilityLabel = new("%AbilityLabel");
		public static readonly NodePath Method = new("%Method");
		public static readonly NodePath Name = new("%Name");
		public static readonly NodePath Type = new("%Type");
		public static readonly NodePath Value = new("%Value");
	}
	
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<NumericBonus> transport);
	
	private AbilityOptionsButton abilityInput;
	private Label abilityLabel;
	private OptionButton methodInput;
	private LineEdit nameInput;
	private SpinBox valueInput;
	private NumericStatOptionsButton typeInput;
	
	public NumericBonus Value { get; set; }
	
	public override void _Ready()
	{
		Value ??= new();
		
		abilityInput = GetNode<AbilityOptionsButton>(NodePaths.Ability);
		abilityLabel = GetNode<Label>(NodePaths.AbilityLabel);
		methodInput = GetNode<OptionButton>(NodePaths.Method);
		nameInput = GetNode<LineEdit>(NodePaths.Name);
		typeInput = GetNode<NumericStatOptionsButton>(NodePaths.Type);
		valueInput = GetNode<SpinBox>(NodePaths.Value);
		
		abilityInput.ItemSelected += abilityChanged;
		methodInput.ItemSelected += methodChanged;
		nameInput.TextChanged += nameChanged;
		typeInput.ItemSelected += typeChanged;
		valueInput.ValueChanged += valueChanged;
	}
	
	public void SetValue(NumericBonus numericBonus)
	{
		Value = numericBonus;
		
		abilityInput.SelectItemByText(Value.Ability?.GetLabel());
		methodInput.Selected = Value.Add ? 1 : 0;
		nameInput.Text = Value.Name;
		typeInput.SelectItemByText(Value.Type.GetLabel());
		valueInput.Value = Value.Value;
		
		toggleAbilityNodes();
	}
	
	private void doEmitSignal() => EmitSignal(SignalName.ValueChanged, new Transport<NumericBonus>(Value));
	
	private void toggleAbilityNodes()
	{
		if(Value.Type == NumericStats.AbilityScore)
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
	
	private void abilityChanged(long index)
	{
		Value.Ability = System.Enum.GetValues<Abilities>()
			.Where(a => a.GetLabel() == abilityInput.GetItemText((int)index))
			.FirstOrDefault();
		
		doEmitSignal();
	}
	
	private void methodChanged(long index)
	{
		Value.Add = index > 0;
		doEmitSignal();
	}
	
	private void nameChanged(string text)
	{
		Value.Name = text;
		doEmitSignal();
	}
	
	private void typeChanged(long index)
	{
		Value.Type = (NumericStats)index - 1;
		if(Value.Type != NumericStats.AbilityScore)
			abilityInput.Deselect();
		
		toggleAbilityNodes();
		doEmitSignal();
	}
	
	private void valueChanged(double value)
	{
		Value.Value = (int)value;
		doEmitSignal();
	}
}
