using Godot;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.CoD.Meta;
using OCSM.Nodes.Meta;

namespace OCSM.Nodes.CoD.CtL.Meta
{
	public partial class CodChangelingAddEditMetadata : BaseAddEditMetadata
	{
		private sealed class NodePath
		{
			public const string ContractName = "%Contract";
			public const string ContractTypeName = "%Contract Type";
			public const string CourtName = "%Court";
			public const string KithName = "%Kith";
			public const string MeritName = "%Merit";
			public const string MetadataSelectorName = "%MetadataSelector";
			public const string RegaliaName = "%Regalia";
			public const string SeemingName = "%Seeming";
			public const string TabContainer = "%TabContainer";
		}
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			CloseRequested += closeHandler;
			
			var contractEntry = GetNode<ContractEntry>(NodePath.ContractName);
			contractEntry.SaveClicked += saveContract;
			contractEntry.DeleteConfirmed += deleteContract;
			
			var contractTypeEntry = GetNode<BasicMetadataEntry>(NodePath.ContractTypeName);
			contractTypeEntry.SaveClicked += saveContractType;
			contractTypeEntry.DeleteConfirmed += deleteContractType;
			
			var courtEntry = GetNode<BasicMetadataEntry>(NodePath.CourtName);
			courtEntry.SaveClicked += saveCourt;
			courtEntry.DeleteConfirmed += deleteCourt;
			
			var kithEntry = GetNode<BasicMetadataEntry>(NodePath.KithName);
			kithEntry.SaveClicked += saveKith;
			kithEntry.DeleteConfirmed += deleteKith;
			
			var meritEntry = GetNode<MeritEntry>(NodePath.MeritName);
			meritEntry.SaveClicked += saveMerit;
			meritEntry.DeleteConfirmed += deleteMerit;
			
			var regaliaEntry = GetNode<BasicMetadataEntry>(NodePath.RegaliaName);
			regaliaEntry.SaveClicked += saveRegalia;
			regaliaEntry.DeleteConfirmed += deleteRegalia;
			
			var seemingEntry = GetNode<BasicMetadataEntry>(NodePath.SeemingName);
			seemingEntry.SaveClicked += saveSeeming;
			seemingEntry.DeleteConfirmed += deleteSeeming;
		}
		
		private void closeHandler()
		{
			QueueFree();
		}
		
		private void deleteContract(string name)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Contracts.Find(c => c.Name.Equals(name)) is OCSM.CoD.CtL.Contract contract)
				{
					ccc.Contracts.Remove(contract);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		private void deleteContractType(string name)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.ContractTypes.Find(ct => ct.Name.Equals(name)) is ContractType contractType)
				{
					ccc.ContractTypes.Remove(contractType);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		private void deleteCourt(string name)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Courts.Find(c => c.Name.Equals(name)) is Court court)
				{
					ccc.Courts.Remove(court);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		private void deleteKith(string name)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Kiths.Find(k => k.Name.Equals(name)) is Kith kith)
				{
					ccc.Kiths.Remove(kith);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		private void deleteMerit(string name)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
				{
					ccc.Merits.Remove(merit);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		private void deleteRegalia(string name)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Regalias.Find(r => r.Name.Equals(name)) is Regalia regalia)
				{
					ccc.Regalias.Remove(regalia);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		private void deleteSeeming(string name)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Seemings.Find(s => s.Name.Equals(name)) is Seeming seeming)
				{
					ccc.Seemings.Remove(seeming);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		private void saveContract()
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				var entry = GetNode<ContractEntry>(NodePath.ContractName);
				var contract = entry.GetNode<Contract>(ContractEntry.NodePath.ContractInput).getData();
				
				if(ccc.Contracts.Find(c => c.Name.Equals(contract.Name)) is OCSM.CoD.CtL.Contract existingContract)
					ccc.Contracts.Remove(existingContract);
				
				ccc.Contracts.Add(contract);
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		private void saveContractType(string name, string description)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.ContractTypes.Find(ct => ct.Name.Equals(name)) is ContractType contractType)
					ccc.ContractTypes.Remove(contractType);
				
				ccc.ContractTypes.Add(new ContractType() { Description = description, Name = name, });
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		private void saveCourt(string name, string description)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Courts.Find(c => c.Name.Equals(name)) is Court court)
					ccc.Courts.Remove(court);
				
				ccc.Courts.Add(new Court() { Description = description, Name = name, });
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		private void saveKith(string name, string description)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Kiths.Find(k => k.Name.Equals(name)) is Kith kith)
					ccc.Kiths.Remove(kith);
				
				ccc.Kiths.Add(new Kith() { Description = description, Name = name, });
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		private void saveMerit(string name, string description, int value)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
					ccc.Merits.Remove(merit);
				
				ccc.Merits.Add(new Merit() { Description = description, Name = name, Value = value });
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		private void saveRegalia(string name, string description)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Regalias.Find(r => r.Name.Equals(name)) is Regalia regalia)
					ccc.Regalias.Remove(regalia);
				
				ccc.Regalias.Add(new Regalia() { Description = description, Name = name, });
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		private void saveSeeming(string name, string description)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Seemings.Find(s => s.Name.Equals(name)) is Seeming seeming)
					ccc.Seemings.Remove(seeming);
				
				ccc.Seemings.Add(new Seeming() { Description = description, Name = name, });
				EmitSignal(nameof(MetadataChanged));
			}
		}
	}
}
