using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Cofd.Nodes;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class CofdChangelingAddEditMetadata : BaseAddEditMetadata
{
	private sealed class NodePaths
	{
		public static readonly NodePath ContractName = new("%Contract");
		public static readonly NodePath ContractTypeName = new("%Contract Type");
		public static readonly NodePath CourtName = new("%Court");
		public static readonly NodePath KithName = new("%Kith");
		public static readonly NodePath MeritName = new("%Merit");
		public static readonly NodePath MetadataSelectorName = new("%MetadataSelector");
		public static readonly NodePath RegaliaName = new("%Regalia");
		public static readonly NodePath SeemingName = new("%Seeming");
		public static readonly NodePath TabContainer = new("%TabContainer");
	}
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		CloseRequested += closeHandler;
		
		var contractEntry = GetNode<ContractEntry>(NodePaths.ContractName);
		contractEntry.SaveClicked += saveContract;
		contractEntry.DeleteConfirmed += deleteContract;
		
		var contractTypeEntry = GetNode<BasicMetadataEntry>(NodePaths.ContractTypeName);
		contractTypeEntry.SaveClicked += saveContractType;
		contractTypeEntry.DeleteConfirmed += deleteContractType;
		
		var courtEntry = GetNode<BasicMetadataEntry>(NodePaths.CourtName);
		courtEntry.SaveClicked += saveCourt;
		courtEntry.DeleteConfirmed += deleteCourt;
		
		var kithEntry = GetNode<BasicMetadataEntry>(NodePaths.KithName);
		kithEntry.SaveClicked += saveKith;
		kithEntry.DeleteConfirmed += deleteKith;
		
		var meritEntry = GetNode<MeritEntry>(NodePaths.MeritName);
		meritEntry.MeritSaveClicked += saveMerit;
		meritEntry.DeleteConfirmed += deleteMerit;
		
		var regaliaEntry = GetNode<BasicMetadataEntry>(NodePaths.RegaliaName);
		regaliaEntry.SaveClicked += saveRegalia;
		regaliaEntry.DeleteConfirmed += deleteRegalia;
		
		var seemingEntry = GetNode<BasicMetadataEntry>(NodePaths.SeemingName);
		seemingEntry.SaveClicked += saveSeeming;
		seemingEntry.DeleteConfirmed += deleteSeeming;
	}
	
	private void closeHandler() => QueueFree();
	
	private void deleteContract(string name)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Contracts.Find(c => c.Name.Equals(name)) is Contract contract)
			{
				ccc.Contracts.Remove(contract);
				_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	private void deleteContractType(string name)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.ContractTypes.Find(ct => ct.Name.Equals(name)) is ContractType contractType)
			{
				ccc.ContractTypes.Remove(contractType);
				_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	private void deleteCourt(string name)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Courts.Find(c => c.Name.Equals(name)) is Court court)
			{
				ccc.Courts.Remove(court);
				_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	private void deleteKith(string name)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Kiths.Find(k => k.Name.Equals(name)) is Kith kith)
			{
				ccc.Kiths.Remove(kith);
				_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	private void deleteMerit(string name)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
			{
				ccc.Merits.Remove(merit);
				_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	private void deleteRegalia(string name)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Regalias.Find(r => r.Name.Equals(name)) is Regalia regalia)
			{
				ccc.Regalias.Remove(regalia);
				_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	private void deleteSeeming(string name)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Seemings.Find(s => s.Name.Equals(name)) is Seeming seeming)
			{
				ccc.Seemings.Remove(seeming);
				_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	private void saveContract()
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			var entry = GetNode<ContractEntry>(NodePaths.ContractName);
			var contract = entry.GetNode<ContractNode>(ContractEntry.NodePaths.ContractInput).GetData();
			
			if(ccc.Contracts.Find(c => c.Name.Equals(contract.Name)) is Ocsm.Cofd.Ctl.Contract existingContract)
				ccc.Contracts.Remove(existingContract);
			
			ccc.Contracts.Add(contract);
			_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
	
	private void saveContractType(string name, string description)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.ContractTypes.Find(ct => ct.Name.Equals(name)) is ContractType contractType)
				ccc.ContractTypes.Remove(contractType);
			
			ccc.ContractTypes.Add(new ContractType() { Description = description, Name = name, });
			_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
	
	private void saveCourt(string name, string description)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Courts.Find(c => c.Name.Equals(name)) is Court court)
				ccc.Courts.Remove(court);
			
			ccc.Courts.Add(new Court() { Description = description, Name = name, });
			_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
	
	private void saveKith(string name, string description)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Kiths.Find(k => k.Name.Equals(name)) is Kith kith)
				ccc.Kiths.Remove(kith);
			
			ccc.Kiths.Add(new Kith() { Description = description, Name = name, });
			_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
	
	private void saveMerit(string name, string description, int value)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
				ccc.Merits.Remove(merit);
			
			ccc.Merits.Add(new Merit() { Description = description, Name = name, Value = value });
			_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
	
	private void saveRegalia(string name, string description)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Regalias.Find(r => r.Name.Equals(name)) is Regalia regalia)
				ccc.Regalias.Remove(regalia);
			
			ccc.Regalias.Add(new Regalia() { Description = description, Name = name, });
			_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
	
	private void saveSeeming(string name, string description)
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Seemings.Find(s => s.Name.Equals(name)) is Seeming seeming)
				ccc.Seemings.Remove(seeming);
			
			ccc.Seemings.Add(new Seeming() { Description = description, Name = name, });
			_ = EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
}
