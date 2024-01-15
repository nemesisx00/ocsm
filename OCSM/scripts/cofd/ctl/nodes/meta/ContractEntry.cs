using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class ContractEntry : Container
{
	public sealed class NodePaths
	{
		public static readonly NodePath ContractInput = new("ContractInputs");
		public static readonly NodePath ClearButton = new("Clear");
		public static readonly NodePath DeleteButton = new("Delete");
		public static readonly NodePath SaveButton = new("Save");
		public static readonly NodePath ExistingEntryName = new("ExistingEntry");
		public static readonly NodePath ContractsName = new("Contracts");
	}
	
	private const string ContractNameFormatOne = "{0} ({1})";
	private const string ContractNameFormatTwo = "{0} ({1} {2})";
	private const string ContractNameContainsParen = " (";
	private const string EntryTypeName = "Contract";
	
	[Signal]
	public delegate void SaveClickedEventHandler();
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	private static string generateEntryName(Contract contract)
	{
		var ct = string.Empty;
		var r = string.Empty;
		
		if(string.IsNullOrEmpty(r) && contract.Regalia is not null)
			r = contract.Regalia.Name;
		
		if(contract.ContractType is ContractType contractType)
		{
			ct = contractType.Name;
			if(contractType.Name.Equals(ContractType.Goblin))
				r = string.Empty;
		}
		
		var itemName = contract.Name;
		
		if(!string.IsNullOrEmpty(ct) && !string.IsNullOrEmpty(r))
			itemName = string.Format(ContractNameFormatTwo, contract.Name, ct, r);
		
		if(!string.IsNullOrEmpty(ct) && string.IsNullOrEmpty(r))
			itemName = string.Format(ContractNameFormatOne, contract.Name, ct);
		
		if(string.IsNullOrEmpty(ct) && !string.IsNullOrEmpty(r))
			itemName = string.Format(ContractNameFormatOne, contract.Name, r);
		
		return itemName;
	}
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		metadataManager.MetadataLoaded += refreshMetadata;
		metadataManager.MetadataSaved += refreshMetadata;
		
		GetNode<ContractNode>(NodePaths.ContractInput).ToggleDetails();
		GetNode<Button>(NodePaths.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePaths.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePaths.DeleteButton).Pressed += handleDelete;
		GetNode<OptionButton>(NodePaths.ExistingEntryName).ItemSelected += entrySelected;
		
		refreshMetadata();
	}
	
	public void LoadContract(Contract contract)
	{
		var contractInput = GetNode<ContractNode>(NodePaths.ContractInput);
		contractInput.SetData(contract);
	}
	
	private void clearInputs()
	{
		var contractInput = GetNode<ContractNode>(NodePaths.ContractInput);
		contractInput.ClearInputs();
	}
	
	private void doDelete()
	{
		var data = GetNode<ContractNode>(NodePaths.ContractInput).GetData();
		if(!string.IsNullOrEmpty(data.Name))
		{
			_ = EmitSignal(SignalName.DeleteConfirmed, data.Name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	private void doSave()
	{
		var data = GetNode<ContractNode>(NodePaths.ContractInput).GetData();
		if(!string.IsNullOrEmpty(data.Name))
		{
			_ = EmitSignal(SignalName.SaveClicked);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	private void entrySelected(long index)
	{
		var optionButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		var name = optionButton.GetItemText((int)index);
		if(name.Contains(ContractNameContainsParen))
			name = name[..name.IndexOf(ContractNameContainsParen)];
		
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Contracts.Find(c => c.Name.Equals(name)) is Contract contract)
			{
				LoadContract(contract);
				optionButton.Deselect();
			}
		}
	}
	
	private void handleDelete()
	{
		var resource = GD.Load<PackedScene>(ScenePaths.Meta.ConfirmDeleteEntry);
		var instance = resource.Instantiate<ConfirmDeleteEntry>();
		instance.EntryTypeName = EntryTypeName;
		GetTree().CurrentScene.AddChild(instance);
		instance.Confirmed += doDelete;
		instance.PopupCentered();
	}
	
	private void refreshMetadata()
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			var optionButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(string.Empty);
			ccc.Contracts.ForEach(c => optionButton.AddItem(generateEntryName(c)));
		}
	}
}
