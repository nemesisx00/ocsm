using Godot;
using OCSM;

public class ContractTypeButton : OptionButton
{
	public override void _Ready()
	{
		foreach(var t in ContractType.asList())
		{
			AddItem(t);
		}
	}
}
