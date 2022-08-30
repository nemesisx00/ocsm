using Godot;
using System;
using OCSM;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;

public class MetadataSelector : GridContainer
{
	[Signal]
	public delegate void ContractSelected(string name);
	[Signal]
	public delegate void ContractTypeSelected(string name);
	[Signal]
	public delegate void CourtSelected(string name);
	[Signal]
	public delegate void KithSelected(string name);
	[Signal]
	public delegate void MeritSelected(string name);
	[Signal]
	public delegate void RegaliaSelected(string name);
	[Signal]
	public delegate void SeemingSelected(string name);
	
	private const string ContractsName = "Contracts";
	private const string ContractTypesName = "ContractTypes";
	private const string CourtsName = "Courts";
	private const string KithsName = "Kiths";
	private const string MeritsName = "Merits";
	private const string RegaliasName = "Regalias";
	private const string SeemingsName = "Seemings";
	
	
	private const string ContractNameFormatOne = "{0} ({1})";
	private const string ContractNameFormatTwo = "{0} ({1} {2})";
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		metadataManager.Connect(nameof(MetadataManager.MetadataSaved), this, nameof(refreshMetadata));
		metadataManager.Connect(nameof(MetadataManager.MetadataLoaded), this, nameof(refreshMetadata));
		
		GetNode<OptionButton>(ContractsName).Connect(Constants.Signal.ItemSelected, this, nameof(contractSelected));
		GetNode<OptionButton>(ContractTypesName).Connect(Constants.Signal.ItemSelected, this, nameof(contractTypeSelected));
		GetNode<OptionButton>(CourtsName).Connect(Constants.Signal.ItemSelected, this, nameof(courtSelected));
		GetNode<OptionButton>(KithsName).Connect(Constants.Signal.ItemSelected, this, nameof(kithSelected));
		GetNode<OptionButton>(MeritsName).Connect(Constants.Signal.ItemSelected, this, nameof(meritSelected));
		GetNode<OptionButton>(RegaliasName).Connect(Constants.Signal.ItemSelected, this, nameof(regaliaSelected));
		GetNode<OptionButton>(SeemingsName).Connect(Constants.Signal.ItemSelected, this, nameof(seemingSelected));
		
		refreshMetadata();
	}
	
	private void refreshMetadata()
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			var optionButton = GetNode<OptionButton>(ContractsName);
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
			
			optionButton = GetNode<OptionButton>(MeritsName);
			optionButton.Clear();
			optionButton.AddItem("");
			foreach(var m in ccc.Merits)
			{
				optionButton.AddItem(m.Name);
			}
		}
	}
	
	private void contractSelected(int index)
	{
		var optionsButton = GetNode<OptionButton>(ContractsName);
		var name = optionsButton.GetItemText(index);
		if(name.Contains(" ("))
			name = name.Substring(0, name.IndexOf(" ("));
		EmitSignal(nameof(ContractSelected), name);
		optionsButton.Selected = 0;
	}
	
	private void contractTypeSelected(int index)
	{
		var optionsButton = GetNode<OptionButton>(ContractTypesName);
		EmitSignal(nameof(ContractTypeSelected), optionsButton.GetItemText(index));
		optionsButton.Selected = 0;
	}
	
	private void courtSelected(int index)
	{
		var optionsButton = GetNode<OptionButton>(CourtsName);
		EmitSignal(nameof(CourtSelected), optionsButton.GetItemText(index));
		optionsButton.Selected = 0;
	}
	
	private void kithSelected(int index)
	{
		var optionsButton = GetNode<OptionButton>(KithsName);
		EmitSignal(nameof(KithSelected), optionsButton.GetItemText(index));
		optionsButton.Selected = 0;
	}
	
	private void meritSelected(int index)
	{
		var optionsButton = GetNode<OptionButton>(MeritsName);
		EmitSignal(nameof(MeritSelected), optionsButton.GetItemText(index));
		optionsButton.Selected = 0;
	}
	
	private void regaliaSelected(int index)
	{
		var optionsButton = GetNode<OptionButton>(RegaliasName);
		EmitSignal(nameof(RegaliaSelected), optionsButton.GetItemText(index));
		optionsButton.Selected = 0;
	}
	
	private void seemingSelected(int index)
	{
		var optionsButton = GetNode<OptionButton>(SeemingsName);
		EmitSignal(nameof(SeemingSelected), optionsButton.GetItemText(index));
		optionsButton.Selected = 0;
	}
}
