using Godot;
using System;
using Ocsm.Cofd.Ctl;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Meta;

namespace Ocsm.Nodes.Cofd.Ctl.Meta;

public partial class ContractEntry : Container
{
	public sealed class NodePath
	{
		public const string ContractInput = "ContractInputs";
		public const string ClearButton = "Clear";
		public const string DeleteButton = "Delete";
		public const string SaveButton = "Save";
		public const string ExistingEntryName = "ExistingEntry";
		public const string ContractsName = "Contracts";
	}
	
	private const string ContractNameFormatOne = "{0} ({1})";
	private const string ContractNameFormatTwo = "{0} ({1} {2})";
	
	[Signal]
	public delegate void SaveClickedEventHandler();
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		metadataManager.MetadataLoaded += refreshMetadata;
		metadataManager.MetadataSaved += refreshMetadata;
		
		GetNode<ContractNode>(NodePath.ContractInput).toggleDetails();
		GetNode<Button>(NodePath.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePath.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePath.DeleteButton).Pressed += handleDelete;
		GetNode<OptionButton>(NodePath.ExistingEntryName).ItemSelected += entrySelected;
		
		refreshMetadata();
	}
	
	public void loadContract(Ocsm.Cofd.Ctl.Contract contract)
	{
		var contractInput = GetNode<ContractNode>(NodePath.ContractInput);
		contractInput.setData(contract);
	}
	
	private void clearInputs()
	{
		var contractInput = GetNode<ContractNode>(NodePath.ContractInput);
		contractInput.clearInputs();
	}
	
	private void doDelete()
	{
		var data = GetNode<ContractNode>(NodePath.ContractInput).getData();
		if(!String.IsNullOrEmpty(data.Name))
		{
			EmitSignal(nameof(DeleteConfirmed), data.Name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	private void doSave()
	{
		var data = GetNode<ContractNode>(NodePath.ContractInput).getData();
		if(!String.IsNullOrEmpty(data.Name))
		{
			EmitSignal(nameof(SaveClicked));
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	private void entrySelected(long index)
	{
		var optionButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
		var name = optionButton.GetItemText((int)index);
		if(name.Contains(" ("))
			name = name.Substring(0, name.IndexOf(" ("));
		
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Contracts.Find(c => c.Name.Equals(name)) is Ocsm.Cofd.Ctl.Contract contract)
			{
				loadContract(contract);
				optionButton.Deselect();
			}
		}
	}
	
	private string generateEntryName(Ocsm.Cofd.Ctl.Contract contract)
	{
		var ct = String.Empty;
		var r = String.Empty;
		
		if(String.IsNullOrEmpty(r) && contract.Regalia is ContractRegalia)
			r = contract.Regalia.Name;
		
		if(contract.ContractType is ContractType contractType)
		{
			ct = contractType.Name;
			if(contractType.Name.Equals(ContractType.Goblin))
				r = String.Empty;
		}
		
		var itemName = contract.Name;
		if(!String.IsNullOrEmpty(ct) && !String.IsNullOrEmpty(r))
			itemName = String.Format(ContractNameFormatTwo, contract.Name, ct, r);
		if(!String.IsNullOrEmpty(ct) && String.IsNullOrEmpty(r))
			itemName = String.Format(ContractNameFormatOne, contract.Name, ct);
		if(String.IsNullOrEmpty(ct) && !String.IsNullOrEmpty(r))
			itemName = String.Format(ContractNameFormatOne, contract.Name, r);
		
		return itemName;
	}
	
	private void handleDelete()
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.Meta.ConfirmDeleteEntry);
		var instance = resource.Instantiate<ConfirmDeleteEntry>();
		instance.EntryTypeName = "Contract";
		GetTree().CurrentScene.AddChild(instance);
		instance.Confirmed += doDelete;
		instance.PopupCentered();
	}
	
	private void refreshMetadata()
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			var optionButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(String.Empty);
			ccc.Contracts.ForEach(c => optionButton.AddItem(generateEntryName(c)));
		}
	}
}
