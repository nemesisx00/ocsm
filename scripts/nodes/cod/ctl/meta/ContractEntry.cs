using Godot;
using System;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.Meta;

namespace OCSM.Nodes.CoD.CtL.Meta
{
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
			
			GetNode<Contract>(NodePath.ContractInput).toggleDetails();
			GetNode<Button>(NodePath.ClearButton).Pressed += clearInputs;
			GetNode<Button>(NodePath.SaveButton).Pressed += doSave;
			GetNode<Button>(NodePath.DeleteButton).Pressed += handleDelete;
			GetNode<OptionButton>(NodePath.ExistingEntryName).ItemSelected += entrySelected;
			
			refreshMetadata();
		}
		
		public void loadContract(OCSM.CoD.CtL.Contract contract)
		{
			var contractInput = GetNode<Contract>(NodePath.ContractInput);
			contractInput.setData(contract);
		}
		
		private void clearInputs()
		{
			var contractInput = GetNode<Contract>(NodePath.ContractInput);
			contractInput.clearInputs();
		}
		
		private void doSave()
		{
			var data = GetNode<Contract>(NodePath.ContractInput).getData();
			if(!String.IsNullOrEmpty(data.Name))
			{
				EmitSignal(nameof(SaveClicked));
				clearInputs();
			}
			//TODO: Display error message if name is empty
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
		
		private void doDelete()
		{
			var data = GetNode<Contract>(NodePath.ContractInput).getData();
			if(!String.IsNullOrEmpty(data.Name))
			{
				EmitSignal(nameof(DeleteConfirmed), data.Name);
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
			
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Contracts.Find(c => c.Name.Equals(name)) is OCSM.CoD.CtL.Contract contract)
				{
					loadContract(contract);
					optionButton.Selected = 0;
				}
			}
		}
		
		private void refreshMetadata()
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				var optionButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
				optionButton.Clear();
				optionButton.AddItem("");
				foreach(var c in ccc.Contracts)
				{
					var ct = String.Empty;
					var r = String.Empty;
					
					if(String.IsNullOrEmpty(r) && c.Regalia is ContractRegalia)
						r = c.Regalia.Name;
					
					if(c.ContractType is ContractType)
					{
						ct = c.ContractType.Name;
						if(c.ContractType.Name.Equals("Goblin"))
							r = String.Empty;
					}
					
					var itemName = c.Name;
					if(!String.IsNullOrEmpty(ct) && !String.IsNullOrEmpty(r))
						itemName = String.Format(ContractNameFormatTwo, c.Name, ct, r);
					if(!String.IsNullOrEmpty(ct) && String.IsNullOrEmpty(r))
						itemName = String.Format(ContractNameFormatOne, c.Name, ct);
					if(String.IsNullOrEmpty(ct) && !String.IsNullOrEmpty(r))
						itemName = String.Format(ContractNameFormatOne, c.Name, r);
					
					optionButton.AddItem(itemName);
				}
			}
		}
	}
}
