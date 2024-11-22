using System.Linq;
using Godot;
using Godot.Collections;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Cofd.Nodes.Meta;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Meta;

namespace Ocsm.Cofd.Ctl.Nodes.Meta;

public partial class CofdChangelingAddEditMetadata : Container, IAddEditMetadata
{
	private static class AnimationName
	{
		public static readonly StringName SlideUp = new("slideUp");
	}
	
	private static class NodePaths
	{
		public static readonly NodePath AnimationPlayer = new("%AnimationPlayer");
		public static readonly NodePath CloseButton = new("%CloseButton");
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
	
	private AnimationPlayer animPlayer;
	private ContractEntry contractEntry;
	private ContractNode contractInput;
	private MetadataEntry contractRegaliaEntry;
	private MetadataEntry contractTypeEntry;
	private MetadataEntry courtEntry;
	private MetadataEntry kithEntry;
	private MeritEntry meritEntry;
	private MetadataEntry regaliaEntry;
	private MetadataEntry seemingEntry;
	
	public override void _ExitTree()
	{
		contractEntry.SaveClicked -= saveContract;
		contractEntry.DeleteConfirmed -= deleteContract;
		contractRegaliaEntry.SaveClicked -= saveMetadata;
		contractRegaliaEntry.DeleteConfirmed -= deleteMetadata;
		contractTypeEntry.SaveClicked -= saveMetadata;
		contractTypeEntry.DeleteConfirmed -= deleteMetadata;
		courtEntry.SaveClicked -= saveMetadata;
		courtEntry.DeleteConfirmed -= deleteMetadata;
		kithEntry.SaveClicked -= saveMetadata;
		kithEntry.DeleteConfirmed -= deleteMetadata;
		meritEntry.SaveMeritClicked -= saveMerit;
		meritEntry.DeleteConfirmed -= deleteMerit;
		regaliaEntry.SaveClicked -= saveMetadata;
		regaliaEntry.DeleteConfirmed -= deleteMetadata;
		seemingEntry.SaveClicked -= saveMetadata;
		seemingEntry.DeleteConfirmed -= deleteMetadata;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		animPlayer = GetNode<AnimationPlayer>(NodePaths.AnimationPlayer);
		
		contractEntry = GetNode<ContractEntry>(NodePaths.Contract);
		contractEntry.SaveClicked += saveContract;
		contractEntry.DeleteConfirmed += deleteContract;
		
		contractInput = contractEntry.GetNode<ContractNode>(ContractEntry.NodePaths.ContractInput);
		
		contractRegaliaEntry = GetNode<MetadataEntry>(NodePaths.ContractRegalia);
		contractRegaliaEntry.SaveClicked += saveMetadata;
		contractRegaliaEntry.DeleteConfirmed += deleteMetadata;
		
		contractTypeEntry = GetNode<MetadataEntry>(NodePaths.ContractType);
		contractTypeEntry.SaveClicked += saveMetadata;
		contractTypeEntry.DeleteConfirmed += deleteMetadata;
		
		courtEntry = GetNode<MetadataEntry>(NodePaths.Court);
		courtEntry.SaveClicked += saveMetadata;
		courtEntry.DeleteConfirmed += deleteMetadata;
		
		kithEntry = GetNode<MetadataEntry>(NodePaths.Kith);
		kithEntry.SaveClicked += saveMetadata;
		kithEntry.DeleteConfirmed += deleteMetadata;
		
		meritEntry = GetNode<MeritEntry>(NodePaths.Merit);
		meritEntry.SaveMeritClicked += saveMerit;
		meritEntry.DeleteConfirmed += deleteMerit;
		
		regaliaEntry = GetNode<MetadataEntry>(NodePaths.Regalia);
		regaliaEntry.SaveClicked += saveMetadata;
		regaliaEntry.DeleteConfirmed += deleteMetadata;
		
		seemingEntry = GetNode<MetadataEntry>(NodePaths.Seeming);
		seemingEntry.SaveClicked += saveMetadata;
		seemingEntry.DeleteConfirmed += deleteMetadata;
		
		GetNode<Button>(NodePaths.CloseButton).Pressed += Close;
		
		animPlayer.Play(AnimationName.SlideUp);
	}
	
	public override void _UnhandledKeyInput(InputEvent evt)
	{
		if(evt.IsActionReleased(Actions.Cancel))
			Close();
	}
	
	public void Close()
	{
		animPlayer.PlayBackwards(AnimationName.SlideUp);
		animPlayer.AnimationFinished += anim => QueueFree();
	}
	
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
	
	private void deleteMetadata(string name, Array<string> types)
	{
		if(metadataManager.Container is CofdChangelingContainer container
			&& container.Metadata.Where(m => m.Types == types.ToList() && m.Name == name).FirstOrDefault() is Metadata entry)
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
	
	private void saveMetadata(string name, string description, Array<string> types)
	{
		if(metadataManager.Container is CofdChangelingContainer container)
		{
			if(container.Metadata.Where(m => m.Types == types.ToList() && m.Name == name).FirstOrDefault() is Metadata entry)
				container.Metadata.Remove(entry);
			
			container.Metadata.Add(new()
			{
				Description = description,
				Name = name,
				Types = [.. types],
			});
			
			metadataManager.SaveGameSystemMetadata();
		}
	}
}
