using Godot;
using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public partial class FeatureEntry : Container, ICanDelete
	{
		private sealed class Names
		{
			public const string MetadataTypeLabel = "Feature";
			public const string Class = "Class";
			public const string ClassLabel = "ClassLabel";
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
			public const string RequiredLevel = "RequiredLevel";
			public const string SaveButton = "Save";
		}
		
		[Signal]
		public delegate void SaveClickedEventHandler(Transport<OCSM.DnD.Fifth.Feature> feature);
		[Signal]
		public delegate void DeleteConfirmedEventHandler(string name);
		
		public OCSM.DnD.Fifth.Feature Feature { get; set; }
		
		private Label classLabel;
		private ClassOptionsButton classNode;
		private TextEdit descriptionNode;
		private LineEdit nameNode;
		private NumericBonusEditList numericBonusesNode;
		private SpinBox requiredLevel;
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
			
			classLabel = GetNode<Label>(NodePathBuilder.SceneUnique(Names.ClassLabel));
			classNode = GetNode<ClassOptionsButton>(NodePathBuilder.SceneUnique(Names.Class));
			descriptionNode = GetNode<TextEdit>(NodePathBuilder.SceneUnique(Names.Description));
			nameNode = GetNode<LineEdit>(NodePathBuilder.SceneUnique(Names.Name));
			numericBonusesNode = GetNode<NumericBonusEditList>(NodePathBuilder.SceneUnique(Names.NumericBonusEditListName));
			requiredLevel = GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.RequiredLevel));
			sectionsNode = GetNode<SectionList>(NodePathBuilder.SceneUnique(Names.Sections));
			sourceNode = GetNode<LineEdit>(NodePathBuilder.SceneUnique(Names.Source));
			textNode = GetNode<TextEdit>(NodePathBuilder.SceneUnique(Names.Text));
			typeNode = GetNode<FeatureTypeOptionsButton>(NodePathBuilder.SceneUnique(Names.Type));
			
			classNode.ItemSelected += classChanged;
			descriptionNode.TextChanged += descriptionChanged;
			nameNode.TextChanged += nameChanged;
			numericBonusesNode.ValuesChanged += numericBonusesChanged;
			requiredLevel.ValueChanged += requiredLevelChanged;
			sectionsNode.ValuesChanged += sectionsChanged;
			sourceNode.TextChanged += sourceChanged;
			textNode.TextChanged += textChanged;
			typeNode.ItemSelected += typeChanged;
			
			GetNode<Button>(NodePathBuilder.SceneUnique(Names.ClearButton)).Pressed += clearInputs;
			GetNode<Button>(NodePathBuilder.SceneUnique(Names.SaveButton)).Pressed += doSave;
			GetNode<Button>(NodePathBuilder.SceneUnique(Names.DeleteButton)).Pressed += handleDelete;
			GetNode<FeatureOptionsButton>(NodePathBuilder.SceneUnique(Names.ExistingEntryName)).ItemSelected += entrySelected;
			
			refreshValues();
		}
		
		private void refreshValues()
		{
			if(Feature is OCSM.DnD.Fifth.Feature)
			{
				classNode.select(Feature.ClassName);
				descriptionNode.Text = Feature.Description;
				nameNode.Text = Feature.Name;
				numericBonusesNode.Values = Feature.NumericBonuses;
				numericBonusesNode.refresh();
				requiredLevel.Value = Feature.RequiredLevel;
				sectionsNode.Values = Feature.Sections;
				sectionsNode.refresh();
				sourceNode.Text = Feature.Source;
				textNode.Text = Feature.Text;
				typeNode.Selected = FeatureType.asList().FindIndex(ft => ft.Equals(Feature.Type)) + 1;
			}
			
			toggleClassInput();
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
		
		public void doDelete()
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
		
		private void entrySelected(long index)
		{
			var optionsButton = GetNode<FeatureOptionsButton>(NodePathBuilder.SceneUnique(Names.ExistingEntryName));
			var name = optionsButton.GetItemText((int)index);
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
				Feature.NumericBonuses.Sort();
				refreshValues();
			}
		}
		
		private void toggleClassInput()
		{
			if(Feature.Type.Equals(FeatureType.Class))
			{
				classLabel.Show();
				classNode.Show();
			}
			else
			{
				classLabel.Hide();
				classNode.Hide();
			}
		}
		
		private void classChanged(long index) { Feature.ClassName = classNode.GetItemText((int)index); }
		private void descriptionChanged() { Feature.Description = descriptionNode.Text; }
		private void nameChanged(string text) { Feature.Name = text; }
		
		private void numericBonusesChanged(Transport<List<NumericBonus>> transport)
		{
			var list = new List<NumericBonus>();
			foreach(var nb in transport.Value)
			{
				list.Add(nb);
			}
			list.Sort();
			Feature.NumericBonuses = list;
		}
		
		private void requiredLevelChanged(double value) { Feature.RequiredLevel = (int)value; }
		
		private void sectionsChanged(Transport<List<FeatureSection>> transport)
		{
			var list = new List<FeatureSection>();
			foreach(var fs in transport.Value)
			{
				list.Add(fs);
			}
			Feature.Sections = list;
		}
		private void sourceChanged(string text) { Feature.Source = text; }
		private void textChanged() { Feature.Text = textNode.Text; }
		
		private void typeChanged(long index)
		{
			Feature.Type = typeNode.GetItemText((int)index);
			
			if(!Feature.Type.Equals(FeatureType.Class))
				classNode.Selected = 0;
			toggleClassInput();
		}
	}
}
