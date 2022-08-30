using Godot;
using OCSM;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using System.Text.Json;

public class AddEditMetadata : WindowDialog
{
	private enum Tabs { Selector, Contract, ContractType, Court, Flaw, Kith, Merit, Regalia, Seeming}
	
	private const string ContractName = "Contract";
	private const string ContractTypeName = "ContractType";
	private const string CourtName = "Court";
	private const string FlawName = "Flaw";
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
		contractTypeEntry.Connect(nameof(ContractEntry.DeleteConfirmed), this, nameof(deleteContractType));
		
		var courtEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(CourtName));
		courtEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveCourt));
		courtEntry.Connect(nameof(ContractEntry.DeleteConfirmed), this, nameof(deleteCourt));
		
		var flawEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(FlawName));
		flawEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveFlaw));
		flawEntry.Connect(nameof(ContractEntry.DeleteConfirmed), this, nameof(deleteFlaw));
		
		var kithEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(KithName));
		kithEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveKith));
		kithEntry.Connect(nameof(ContractEntry.DeleteConfirmed), this, nameof(deleteKith));
		
		var meritEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(MeritName));
		meritEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveMerit));
		meritEntry.Connect(nameof(ContractEntry.DeleteConfirmed), this, nameof(deleteMerit));
		
		var regaliaEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(RegaliaName));
		regaliaEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveRegalia));
		regaliaEntry.Connect(nameof(ContractEntry.DeleteConfirmed), this, nameof(deleteRegalia));
		
		var seemingEntry = GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(SeemingName));
		seemingEntry.Connect(nameof(BasicMetadataEntry.SaveClicked), this, nameof(saveSeeming));
		seemingEntry.Connect(nameof(ContractEntry.DeleteConfirmed), this, nameof(deleteSeeming));
		
		var selector = GetNode<MetadataSelector>(NodePathBuilder.SceneUnique(MetadataSelectorName));
		selector.Connect(nameof(MetadataSelector.ContractSelected), this, nameof(editContract));
		selector.Connect(nameof(MetadataSelector.ContractTypeSelected), this, nameof(editContractType));
		selector.Connect(nameof(MetadataSelector.CourtSelected), this, nameof(editCourt));
		selector.Connect(nameof(MetadataSelector.FlawSelected), this, nameof(editFlaw));
		selector.Connect(nameof(MetadataSelector.KithSelected), this, nameof(editKith));
		selector.Connect(nameof(MetadataSelector.MeritSelected), this, nameof(editMerit));
		selector.Connect(nameof(MetadataSelector.RegaliaSelected), this, nameof(editRegalia));
		selector.Connect(nameof(MetadataSelector.SeemingSelected), this, nameof(editSeeming));
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
	
	private void deleteFlaw(string name)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Flaws.Find(f => f.Name.Equals(name)) is Merit flaw)
			{
				ccc.Flaws.Remove(flaw);
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
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(ContractTypeName)).loadMetadataEntry(contractType);
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
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(CourtName)).loadMetadataEntry(court);
				GetNode<TabContainer>(NodePathBuilder.SceneUnique(TabContainer)).CurrentTab = (int)Tabs.Court;
			}
		}
	}
	
	private void editFlaw(string name)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Flaws.Find(f => f.Name.Equals(name)) is Merit flaw)
			{
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(FlawName)).loadMetadataEntry(flaw);
				GetNode<TabContainer>(NodePathBuilder.SceneUnique(TabContainer)).CurrentTab = (int)Tabs.Flaw;
			}
		}
	}
	
	private void editKith(string name)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Kiths.Find(k => k.Name.Equals(name)) is Kith kith)
			{
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(KithName)).loadMetadataEntry(kith);
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
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(MeritName)).loadMetadataEntry(merit);
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
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(RegaliaName)).loadMetadataEntry(regalia);
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
				GetNode<BasicMetadataEntry>(NodePathBuilder.SceneUnique(SeemingName)).loadMetadataEntry(seeming);
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
			var ct = new ContractType(name, description);
			if(ccc.ContractTypes.Contains(ct))
				ccc.ContractTypes.Remove(ct);
			ccc.ContractTypes.Add(ct);
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveCourt(string name, string description)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			var c = new Court(name, description);
			if(ccc.Courts.Contains(c))
				ccc.Courts.Remove(c);
			ccc.Courts.Add(c);
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveFlaw(string name, string description)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			var m = new Merit(name, description);
			if(ccc.Flaws.Contains(m))
				ccc.Flaws.Remove(m);
			ccc.Flaws.Add(m);
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveKith(string name, string description)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			var k = new Kith(name, description);
			if(ccc.Kiths.Contains(k))
				ccc.Kiths.Remove(k);
			ccc.Kiths.Add(k);
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveMerit(string name, string description)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			var m = new Merit(name, description);
			if(ccc.Merits.Contains(m))
				ccc.Merits.Remove(m);
			ccc.Merits.Add(m);
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveRegalia(string name, string description)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			var r = new Regalia(name, description);
			if(ccc.Regalias.Contains(r))
				ccc.Regalias.Remove(r);
			ccc.Regalias.Add(r);
			EmitSignal(nameof(MetadataChanged));
		}
	}
	
	private void saveSeeming(string name, string description)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			var s = new Seeming(name, description);
			if(ccc.Seemings.Contains(s))
				ccc.Seemings.Remove(s);
			ccc.Seemings.Add(s);
			EmitSignal(nameof(MetadataChanged));
		}
	}
}
