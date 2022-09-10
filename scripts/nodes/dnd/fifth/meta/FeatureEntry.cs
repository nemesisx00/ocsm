using Godot;
using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public class FeatureEntry : Container
	{
		private sealed class Names
		{
			public const string MetadataTypeLabel = "Feature";
			public const string Description = "Description";
			public const string Name = "Name";
			public const string Sections = "Sections";
			public const string Source = "Source";
			public const string Text = "Text";
			public const string Type = "Type";
			public const string ClearButton = "Clear";
			public const string DeleteButton = "Delete";
			public const string ExistingEntryName = "ExistingEntry";
			public const string ExistingLabelFormat = "Existing {0}";
			public const string ExistingLabelName = "ExistingLabel";
			public const string NumericBonusEditListName = "NumericBonuses";
			public const string SaveButton = "Save";
		}
		
		[Signal]
		public delegate void SaveClicked(Transport<OCSM.DnD.Fifth.Feature> feature);
		[Signal]
		public delegate void DeleteConfirmed(string name);
		
		public OCSM.DnD.Fifth.Feature Feature { get; set; }
		
		private TextEdit descriptionNode;
		private LineEdit nameNode;
		private NumericBonusEditList numericBonusesNode;
		private SectionList sectionsNode;
		private LineEdit sourceNode;
		private TextEdit textNode;
		private FeatureTypeOptionsButton typeNode;
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			if(!(Feature is OCSM.DnD.Fifth.Feature))
				Feature = new OCSM.DnD.Fifth.Feature();
			
			descriptionNode = GetNode<TextEdit>(NodePathBuilder.SceneUnique(Names.Description));
			nameNode = GetNode<LineEdit>(NodePathBuilder.SceneUnique(Names.Name));
			numericBonusesNode = GetNode<NumericBonusEditList>(NodePathBuilder.SceneUnique(Names.NumericBonusEditListName));
			sectionsNode = GetNode<SectionList>(NodePathBuilder.SceneUnique(Names.Sections));
			sourceNode = GetNode<LineEdit>(NodePathBuilder.SceneUnique(Names.Source));
			textNode = GetNode<TextEdit>(NodePathBuilder.SceneUnique(Names.Text));
			typeNode = GetNode<FeatureTypeOptionsButton>(NodePathBuilder.SceneUnique(Names.Type));
			
			descriptionNode.Connect(Constants.Signal.TextChanged, this, nameof(descriptionChanged));
			nameNode.Connect(Constants.Signal.TextChanged, this, nameof(nameChanged));
			numericBonusesNode.Connect(nameof(NumericBonusEditList.ValuesChanged), this, nameof(numericBonusesChanged));
			sectionsNode.Connect(nameof(SectionList.ValuesChanged), this, nameof(sectionsChanged));
			sourceNode.Connect(Constants.Signal.TextChanged, this, nameof(sourceChanged));
			textNode.Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			typeNode.Connect(Constants.Signal.ItemSelected, this, nameof(typeChanged));
			
			GetNode<Button>(NodePathBuilder.SceneUnique(Names.ClearButton)).Connect(Constants.Signal.Pressed, this, nameof(clearInputs));
			GetNode<Button>(NodePathBuilder.SceneUnique(Names.SaveButton)).Connect(Constants.Signal.Pressed, this, nameof(doSave));
			GetNode<Button>(NodePathBuilder.SceneUnique(Names.DeleteButton)).Connect(Constants.Signal.Pressed, this, nameof(handleDelete));
			GetNode<FeatureOptionsButton>(NodePathBuilder.SceneUnique(Names.ExistingEntryName)).Connect(Constants.Signal.ItemSelected, this, nameof(entrySelected));
			
			refreshValues();
		}
		
		private void refreshValues()
		{
			if(Feature is OCSM.DnD.Fifth.Feature)
			{
				descriptionNode.Text = Feature.Description;
				nameNode.Text = Feature.Name;
				numericBonusesNode.Values = Feature.NumericBonuses;
				numericBonusesNode.refresh();
				sectionsNode.Values = Feature.Sections;
				sectionsNode.refresh();
				sourceNode.Text = Feature.Source;
				textNode.Text = Feature.Text;
				typeNode.Selected = FeatureType.asList().FindIndex(ft => ft.Equals(Feature.Type)) + 1;
			}
		}
		
		private void clearInputs()
		{
			Feature = new OCSM.DnD.Fifth.Feature();
			refreshValues();
		}
		
		private void doSave()
		{
			EmitSignal(nameof(SaveClicked), new Transport<OCSM.DnD.Fifth.Feature>(Feature));
			clearInputs();
		}
		
		private void doDelete()
		{
			EmitSignal(nameof(DeleteConfirmed), Feature.Name);
			clearInputs();
		}
		
		private void handleDelete()
		{
			NodeUtilities.displayDeleteConfirmation(
				Names.MetadataTypeLabel,
				GetTree().CurrentScene,
				GetViewportRect().GetCenter(),
				this,
				nameof(doDelete)
			);
		}
		
		private void entrySelected(int index)
		{
			var optionsButton = GetNode<FeatureOptionsButton>(NodePathBuilder.SceneUnique(Names.ExistingEntryName));
			var name = optionsButton.GetItemText(index);
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Features.Find(f => f.Name.Equals(name)) is OCSM.DnD.Fifth.Feature feature)
				{
					loadEntry(feature);
					optionsButton.Selected = 0;
				}
			}
		}
		
		public void loadEntry(OCSM.DnD.Fifth.Feature feature)
		{
			if(feature is OCSM.DnD.Fifth.Feature)
			{
				Feature = feature;
				refreshValues();
			}
		}
		
		private void descriptionChanged() { Feature.Description = descriptionNode.Text; }
		private void nameChanged(string text) { Feature.Name = text; }
		
		private void numericBonusesChanged(List<Transport<NumericBonus>> values)
		{
			var list = new List<NumericBonus>();
			foreach(var t in values)
			{
				list.Add(t.Value);
			}
			Feature.NumericBonuses = list;
		}
		
		private void sectionsChanged(List<Transport<FeatureSection>> values)
		{
			var list = new List<FeatureSection>();
			foreach(var t in values)
			{
				list.Add(t.Value);
			}
			Feature.Sections = list;
		}
		private void sourceChanged(string text) { Feature.Source = text; }
		private void textChanged() { Feature.Text = textNode.Text; }
		private void typeChanged(int index) { Feature.Type = typeNode.GetItemText(index); }
	}
}
