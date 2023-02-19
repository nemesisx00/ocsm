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
		public const string ContractInput = "ContractInputs";
		private const string ClearButton = "Clear";
		private const string DeleteButton = "Delete";
		private const string SaveButton = "Save";
		private const string ExistingEntryName = "ExistingEntry";
		private const string ContractsName = "Contracts";
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
			
			GetNode<Contract>(NodePathBuilder.SceneUnique(ContractInput)).toggleDetails();
			GetNode<Button>(NodePathBuilder.SceneUnique(ClearButton)).Pressed += clearInputs;
			GetNode<Button>(NodePathBuilder.SceneUnique(SaveButton)).Pressed += doSave;
			GetNode<Button>(NodePathBuilder.SceneUnique(DeleteButton)).Pressed += handleDelete;
			GetNode<OptionButton>(NodePathBuilder.SceneUnique(ExistingEntryName)).ItemSelected += entrySelected;
			
			refreshMetadata();
		}
		
		public void loadContract(OCSM.CoD.CtL.Contract contract)
		{
			var contractInput = GetNode<Contract>(NodePathBuilder.SceneUnique(ContractInput));
			contractInput.setData(contract);
			NodeUtilities.autoSizeChildren(contractInput, Constants.TextInputMinHeight);
		}
		
		private void clearInputs()
		{
			var contractInput = GetNode<Contract>(NodePathBuilder.SceneUnique(ContractInput));
			contractInput.clearInputs();
			NodeUtilities.autoSizeChildren(contractInput, Constants.TextInputMinHeight);
		}
		
		private void doSave()
		{
			var data = GetNode<Contract>(NodePathBuilder.SceneUnique(ContractInput)).getData();
			if(!String.IsNullOrEmpty(data.Name))
			{
				EmitSignal(nameof(SaveClicked));
				clearInputs();
			}
			//TODO: Display error message if name is empty
		}
		
		private void handleDelete()
		{
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.Meta.ConfirmDeleteEntry);
			var instance = resource.Instantiate<ConfirmDeleteEntry>();
			instance.EntryTypeName = "Contract";
			GetTree().CurrentScene.AddChild(instance);
			instance.Confirmed += doDelete;
			instance.PopupCentered();
		}
		
		private void doDelete()
		{
			var data = GetNode<Contract>(NodePathBuilder.SceneUnique(ContractInput)).getData();
			if(!String.IsNullOrEmpty(data.Name))
			{
				EmitSignal(nameof(DeleteConfirmed), data.Name);
				clearInputs();
			}
			//TODO: Display error message if name is empty
		}
		
		private void entrySelected(long index)
		{
			var optionButton = GetNode<OptionButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
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
				var optionButton = GetNode<OptionButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
				optionButton.Clear();
				optionButton.AddItem("");
				foreach(var c in ccc.Contracts)
				{
					var ct = String.Empty;
					var r = String.Empty;
					
					if(String.IsNullOrEmpty(r) && c.Regalia is Regalia)
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
