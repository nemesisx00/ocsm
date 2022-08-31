using Godot;
using OCSM;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using System.Text.Json;

public class AddEditMetadata : WindowDialog
{
	private enum Tabs { Selector, Contract, ContractType, Court, Kith, Merit, Regalia, Seeming}
	
	private const string ContractName = "Contract";
	private const string ContractTypeName = "Contract Type";
	private const string CourtName = "Court";
	private const string KithName = "Kith";
	private const string MeritName = "Merit";
	private const string MetadataSelectorName = "MetadataSelector";
	private const string RegaliaName = "Regalia";
	private const string SeemingName = "Seeming";
	private const string TabContainer = "TabContainer";
	
	[Signal]
	public delegate void MetadataChanged();
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		GetCloseButton().Connect(Constants.Signal.Pressed, this, nameof(closeHandler));
		
		var contractEntry = GetNode<ContractEntry>(NodePathBuilder.SceneUnique(ContractName));
		contractEntry.Connect(nameof(ContractEntry.SaveClicked), this, nameof(saveContract));
		contractEntry.Connect(nameof(ContractEntry.DeleteConfirmed), this, nameof(deleteContract));
		
		var contractTypeEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(ContractTypeName));
		contractTypeEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveContractType));
		contractTypeEntry.Connect(nameof(BasicMetadataEntry.DeleteConfirmed), this, nameof(deleteContractType));
		
		var courtEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(CourtName));
		courtEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveCourt));
		courtEntry.Connect(nameof(BasicMetadataEntry.DeleteConfirmed), this, nameof(deleteCourt));
		
		var kithEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(KithName));
		kithEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveKith));
		kithEntry.Connect(nameof(BasicMetadataEntry.DeleteConfirmed), this, nameof(deleteKith));
		
		var meritEntry = GetNode<MeritEntry>(NodePathBuilder.SceneUnique(MeritName));
		meritEntry.Connect(nameof(MeritEntry.SaveClicked), this, nameof(saveMerit));
		meritEntry.Connect(nameof(MeritEntry.DeleteConfirmed), this, nameof(deleteMerit));
		
		var regaliaEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(RegaliaName));
		regaliaEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveRegalia));
		regaliaEntry.Connect(nameof(BasicMetadataEntry.DeleteConfirmed), this, nameof(deleteRegalia));
		
		var seemingEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(SeemingName));
		seemingEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveSeeming));
		seemingEntry.Connect(nameof(BasicMetadataEntry.DeleteConfirmed), this, nameof(deleteSeeming));
		
		/*
		var selector = GetNode<MetadataSelector>(NodePathBuilder.SceneUnique(MetadataSelectorName));
		selector.Connect(nameof(MetadataSelector.ContractSelected), this, nameof(editContract));
		selector.Connect(nameof(MetadataSelector.ContractTypeSelected), this, nameof(editContractType));
		selector.Connect(nameof(MetadataSelector.CourtSelected), this, nameof(editCourt));
		selector.Connect(nameof(MetadataSelector.KithSelected), this, nameof(editKith));
		selector.Connect(nameof(MetadataSelector.MeritSelected), this, nameof(editMerit));
		selector.Connect(nameof(MetadataSelector.RegaliaSelected), this, nameof(editRegalia));
		selector.Connect(nameof(MetadataSelector.SeemingSelected), this, nameof(editSeeming));
		*/
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
	
	private void editContract(string name)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Contracts.Find(c => c.Name.Equals(name)) is OCSM.CoD.CtL.Contract contract)
			{
				var entry = GetNode<ContractEntry>(NodePathBuilder.SceneUnique(ContractName));
				entry.GetNode<Contract>(NodePathBuilder.SceneUnique(ContractEntry.ContractInput)).setData(contract);
				GetNode<TabContainer>(NodePathBuilder.SceneUnique(TabContainer)).CurrentTab = (int)Tabs.Contract;
			}
		}
	}
	
	private void editContractType(string name)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.ContractTypes.Find(ct => ct.Name.Equals(name)) is ContractType contractType)
			{
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(ContractTypeName)).loadEntry(contractType);
				GetNode<TabContainer>(NodePathBuilder.SceneUnique(TabContainer)).CurrentTab = (int)Tabs.ContractType;
			}
		}
	}
	
	private void editCourt(string name)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Courts.Find(c => c.Name.Equals(name)) is Court court)
			{
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(CourtName)).loadEntry(court);
				GetNode<TabContainer>(NodePathBuilder.SceneUnique(TabContainer)).CurrentTab = (int)Tabs.Court;
			}
		}
	}
	
	private void editKith(string name)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Kiths.Find(k => k.Name.Equals(name)) is Kith kith)
			{
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(KithName)).loadEntry(kith);
				GetNode<TabContainer>(NodePathBuilder.SceneUnique(TabContainer)).CurrentTab = (int)Tabs.Kith;
			}
		}
	}
	
	private void editMerit(string name)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
			{
				GetNode<MeritEntry>(NodePathBuilder.SceneUnique(MeritName)).loadMerit(merit);
				GetNode<TabContainer>(NodePathBuilder.SceneUnique(TabContainer)).CurrentTab = (int)Tabs.Merit;
			}
		}
	}
	
	private void editRegalia(string name)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Regalias.Find(r => r.Name.Equals(name)) is Regalia regalia)
			{
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(RegaliaName)).loadEntry(regalia);
				GetNode<TabContainer>(NodePathBuilder.SceneUnique(TabContainer)).CurrentTab = (int)Tabs.Regalia;
			}
		}
	}
	
	private void editSeeming(string name)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Seemings.Find(ct => ct.Name.Equals(name)) is Seeming seeming)
			{
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(SeemingName)).loadEntry(seeming);
				GetNode<TabContainer>(NodePathBuilder.SceneUnique(TabContainer)).CurrentTab = (int)Tabs.Seeming;
			}
		}
	}
	
	private void saveContract()
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			var entry = GetNode<ContractEntry>(NodePathBuilder.SceneUnique(ContractName));
			var contract = entry.GetNode<Contract>(NodePathBuilder.SceneUnique(ContractEntry.ContractInput)).getData();
			
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
			
			ccc.ContractTypes.Add(new ContractType(name, description));
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveCourt(string name, string description)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Courts.Find(c => c.Name.Equals(name)) is Court court)
				ccc.Courts.Remove(court);
			
			ccc.Courts.Add(new Court(name, description));
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveKith(string name, string description)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Kiths.Find(k => k.Name.Equals(name)) is Kith kith)
				ccc.Kiths.Remove(kith);
			
			ccc.Kiths.Add(new Kith(name, description));
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveMerit(string name, string description, int value)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
				ccc.Merits.Remove(merit);
			
			ccc.Merits.Add(new Merit(name, description, value));
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveRegalia(string name, string description)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Regalias.Find(r => r.Name.Equals(name)) is Regalia regalia)
				ccc.Regalias.Remove(regalia);
			
			ccc.Regalias.Add(new Regalia(name, description));
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveSeeming(string name, string description)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Seemings.Find(s => s.Name.Equals(name)) is Seeming seeming)
				ccc.Seemings.Remove(seeming);
			
			ccc.Seemings.Add(new Seeming(name, description));
			EmitSignal(nameof(MetadataChanged));
		}
	}
}
