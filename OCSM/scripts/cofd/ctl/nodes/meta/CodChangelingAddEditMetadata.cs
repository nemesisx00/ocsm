using System.Linq;
using Godot;
using Ocsm.Cofd;
using Ocsm.Cofd.Ctl;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Cofd.Meta;
using Ocsm.Nodes.Meta;

namespace Ocsm.Nodes.Cofd.Ctl.Meta;

public partial class CofdChangelingAddEditMetadata : Window, IAddEditMetadata
{
	private static class NodePaths
	{
		public readonly static NodePath Contract = new("%Contract");
		public readonly static NodePath ContractRegalia = new("%Contract Regalia");
		public readonly static NodePath ContractType = new("%Contract Type");
		public readonly static NodePath Court = new("%Court");
		public readonly static NodePath Kith = new("%Kith");
		public readonly static NodePath Merit = new("%Merit");
		public readonly static NodePath MetadataSelector = new("%MetadataSelector");
		public readonly static NodePath Regalia = new("%Regalia");
		public readonly static NodePath Seeming = new("%Seeming");
		public readonly static NodePath TabContainer = new("%TabContainer");
	}
	
	private MetadataManager metadataManager;
	
	private ContractNode contractInput;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		
		CloseRequested += closeHandler;
		
		var contractEntry = GetNode<ContractEntry>(NodePaths.Contract);
		contractEntry.SaveClicked += saveContract;
		contractEntry.DeleteConfirmed += deleteContract;
		
		contractInput = contractEntry.GetNode<ContractNode>(ContractEntry.NodePaths.ContractInput);
		
		var contractRegaliaEntry = GetNode<MetadataEntry>(NodePaths.ContractRegalia);
		contractRegaliaEntry.SaveClicked += saveMetadata;
		contractRegaliaEntry.DeleteConfirmed += deleteMetadata;
		
		var contractTypeEntry = GetNode<MetadataEntry>(NodePaths.ContractType);
		contractTypeEntry.SaveClicked += saveMetadata;
		contractTypeEntry.DeleteConfirmed += deleteMetadata;
		
		var courtEntry = GetNode<MetadataEntry>(NodePaths.Court);
		courtEntry.SaveClicked += saveMetadata;
		courtEntry.DeleteConfirmed += deleteMetadata;
		
		var kithEntry = GetNode<MetadataEntry>(NodePaths.Kith);
		kithEntry.SaveClicked += saveMetadata;
		kithEntry.DeleteConfirmed += deleteMetadata;
		
		var meritEntry = GetNode<MeritEntry>(NodePaths.Merit);
		meritEntry.SaveClicked += saveMerit;
		meritEntry.DeleteConfirmed += deleteMerit;
		
		var regaliaEntry = GetNode<MetadataEntry>(NodePaths.Regalia);
		regaliaEntry.SaveClicked += saveMetadata;
		regaliaEntry.DeleteConfirmed += deleteMetadata;
		
		var seemingEntry = GetNode<MetadataEntry>(NodePaths.Seeming);
		seemingEntry.SaveClicked += saveMetadata;
		seemingEntry.DeleteConfirmed += deleteMetadata;
	}
	
	private void closeHandler() => QueueFree();
	
	private void deleteContract(string name)
	{
		if(metadataManager.Container is CofdChangelingContainer container
			&& container.Contracts.Where(c => c.Name == name).FirstOrDefault() is Contract contract)
		{
			container.Contracts.Remove(contract);
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	private void deleteMerit(string name)
	{
		if(metadataManager.Container is CofdChangelingContainer container
			&& container.Merits.Where(m => m.Name == name).FirstOrDefault() is Merit merit)
		{
			container.Merits.Remove(merit);
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	private void deleteMetadata(string name, MetadataType type)
	{
		if(metadataManager.Container is CofdChangelingContainer container
			&& container.Metadata.Where(m => m.Type == type && m.Name == name).FirstOrDefault() is Metadata entry)
		{
			container.Metadata.Remove(entry);
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	private void saveContract()
	{
		if(metadataManager.Container is CofdChangelingContainer container)
		{
			var contract = contractInput.GetData();
			
			if(container.Contracts.Where(c => c.Name == contract.Name).FirstOrDefault() is Contract existingContract)
				container.Contracts.Remove(existingContract);
			
			container.Contracts.Add(contract);
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	private void saveMerit(string name, string description, int value)
	{
		if(metadataManager.Container is CofdChangelingContainer container)
		{
			if(container.Merits.Where(m => m.Name == name).FirstOrDefault() is Merit merit)
				container.Merits.Remove(merit);
			
			container.Merits.Add(new Merit() { Description = description, Name = name, Value = value });
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	private void saveMetadata(string name, string description, MetadataType type)
	{
		if(metadataManager.Container is CofdChangelingContainer container)
		{
			if(container.Metadata.Where(m => m.Type == type && m.Name == name).FirstOrDefault() is Metadata entry)
				container.Metadata.Remove(entry);
			
			container.Metadata.Add(new()
			{
				Description = description,
				Name = name,
				Type = type,
			});
			
			metadataManager.SaveGameSystemMetadata();
		}
	}
}
