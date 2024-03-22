using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Meta;

namespace Ocsm.Cofd.Ctl.Nodes.Meta;

public partial class ContractEntry : Container
{
	public static class NodePaths
	{
		public readonly static NodePath ContractInput = new("%ContractInputs");
		public readonly static NodePath ClearButton = new("%Clear");
		public readonly static NodePath DeleteButton = new("%Delete");
		public readonly static NodePath SaveButton = new("%Save");
		public readonly static NodePath ExistingEntryName = new("%ExistingEntry");
		public readonly static NodePath ContractsName = new("%Contracts");
	}
	
	private static string generateEntryName(Contract contract)
	{
		var ct = string.Empty;
		var r = string.Empty;
		
		if(string.IsNullOrEmpty(r) && contract.Regalia is not null)
			r = contract.Regalia.Name;
		
		if(contract.ContractType is not null)
		{
			ct = contract.ContractType.Name;
			if(contract.ContractType.Name == "Goblin")
				r = string.Empty;
		}
		
		var itemName = contract.Name;
		
		if(!string.IsNullOrEmpty(ct) && !string.IsNullOrEmpty(r))
			itemName = $"{itemName} ({ct} {r})";
		else if(!string.IsNullOrEmpty(ct))
			itemName = $"{itemName} ({ct})";
		else if(!string.IsNullOrEmpty(r))
			itemName = $"{itemName} ({r})";
		
		return itemName;
	}
	
	[Signal]
	public delegate void SaveClickedEventHandler();
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
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
			EmitSignal(SignalName.DeleteConfirmed, data.Name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	private void doSave()
	{
		var data = GetNode<ContractNode>(NodePaths.ContractInput).GetData();
		if(!string.IsNullOrEmpty(data.Name))
		{
			EmitSignal(SignalName.SaveClicked);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	private void entrySelected(long index)
	{
		var optionButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		var name = optionButton.GetItemText((int)index);
		if(name.Contains(" ("))
			name = name[..name.IndexOf(" (")];
		
		if(metadataManager.Container is CofdChangelingContainer container
			&& container.Contracts.Find(c => c.Name == name) is Contract contract)
		{
			LoadContract(contract);
			optionButton.Deselect();
		}
	}
	
	private void handleDelete()
	{
		var resource = GD.Load<PackedScene>(ScenePaths.Meta.ConfirmDeleteEntry);
		var instance = resource.Instantiate<ConfirmDeleteEntry>();
		instance.EntryTypeName = "Contract";
		GetTree().CurrentScene.AddChild(instance);
		instance.Confirmed += doDelete;
		instance.PopupCentered();
	}
	
	private void refreshMetadata()
	{
		if(metadataManager.Container is CofdChangelingContainer container)
		{
			var optionButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(string.Empty);
			container.Contracts.ForEach(c => optionButton.AddItem(generateEntryName(c)));
		}
	}
}
