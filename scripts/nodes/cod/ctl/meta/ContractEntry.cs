using Godot;
using System;
using System.Collections.Generic;
using OCSM;

public class ContractEntry : VBoxContainer
{
	public const string ContractInput = "ContractInputs";
	private const string ClearButton = "Clear";
	private const string DeleteButton = "Delete";
	private const string SaveButton = "Save";
	
	[Signal]
	public delegate void SaveClicked();
	[Signal]
	public delegate void DeleteConfirmed(string name);
	
	public override void _Ready()
	{
		GetNode<Contract>(NodePathBuilder.SceneUnique(ContractInput)).toggleDetails();
		GetNode<Button>(NodePathBuilder.SceneUnique(ClearButton)).Connect(Constants.Signal.Pressed, this, nameof(clearInputs));
		GetNode<Button>(NodePathBuilder.SceneUnique(SaveButton)).Connect(Constants.Signal.Pressed, this, nameof(doSave));
		GetNode<Button>(NodePathBuilder.SceneUnique(DeleteButton)).Connect(Constants.Signal.Pressed, this, nameof(handleDelete));
	}
	
	public void loadContract(OCSM.CoD.CtL.Contract contract)
	{
		GetNode<Contract>(NodePathBuilder.SceneUnique(ContractInput)).setData(contract);
	}
	
	private void clearInputs()
	{
		GetNode<Contract>(NodePathBuilder.SceneUnique(ContractInput)).clearInputs();
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
		var instance = resource.Instance<ConfirmDeleteEntry>();
		instance.EntryTypeName = "Contract";
		GetTree().CurrentScene.AddChild(instance);
		NodeUtilities.centerControl(instance, GetViewportRect().GetCenter());
		instance.Connect(Constants.Signal.Confirmed, this, nameof(doDelete));
		instance.Popup_();
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
}
