using Godot;
using System.Collections.Generic;
using System.Text.Json;
using OCSM;
using OCSM.DnD.Fifth.Meta;

public class NewFeature : WindowDialog
{
	private sealed class Names
	{
		public const string Cancel = "Cancel";
		public const string Description = "Description";
		public const string Name = "Name";
		public const string Save = "Save";
		public const string Sections = "Sections";
		public const string Source = "Source";
		public const string Text = "Text";
		public const string Type = "Type";
	}
	
	private Feature Feature { get; set; }
	
	private TextEdit descriptionNode;
	private LineEdit nameNode;
	private SectionList sectionsNode;
	private LineEdit sourceNode;
	private TextEdit textNode;
	private FeatureTypeOptionButton typeNode;
	
	public override void _Ready()
	{
		Feature = new Feature();
		GetCloseButton().Connect(Constants.Signal.Pressed, this, nameof(onClose));
		GetNode<Button>(NodePathBuilder.SceneUnique(Names.Cancel)).Connect(Constants.Signal.Pressed, this, nameof(doCancel));
		GetNode<Button>(NodePathBuilder.SceneUnique(Names.Save)).Connect(Constants.Signal.Pressed, this, nameof(doSave));
		
		descriptionNode = GetNode<TextEdit>(NodePathBuilder.SceneUnique(Names.Description));
		nameNode = GetNode<LineEdit>(NodePathBuilder.SceneUnique(Names.Name));
		sectionsNode = GetNode<SectionList>(NodePathBuilder.SceneUnique(Names.Sections));
		sourceNode = GetNode<LineEdit>(NodePathBuilder.SceneUnique(Names.Source));
		textNode = GetNode<TextEdit>(NodePathBuilder.SceneUnique(Names.Text));
		typeNode = GetNode<FeatureTypeOptionButton>(NodePathBuilder.SceneUnique(Names.Type));
		
		descriptionNode.Connect(Constants.Signal.TextChanged, this, nameof(descriptionChanged));
		nameNode.Connect(Constants.Signal.TextChanged, this, nameof(nameChanged));
		sectionsNode.Connect(nameof(SectionList.ValueChanged), this, nameof(sectionsChanged));
		sourceNode.Connect(Constants.Signal.TextChanged, this, nameof(sourceChanged));
		textNode.Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		typeNode.Connect(Constants.Signal.ItemSelected, this, nameof(typeChanged));
	}
	
	private void onClose() { QueueFree(); }
	private void doCancel() { Hide(); onClose(); }
	private void doSave()
	{
		var metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		if(metadataManager.Container is DnDFifthContainer dfc)
			dfc.Features.Add(Feature);
		metadataManager.saveGameSystemMetadata();
	}
	
	private void descriptionChanged() { Feature.Description = descriptionNode.Text; }
	private void nameChanged(string text) { Feature.Name = text; }
	private void sectionsChanged(SignalPayload<List<FeatureSection>> payload) { Feature.Sections = payload.Payload; }
	private void sourceChanged(string text) { Feature.Source = text; }
	private void textChanged() { Feature.Text = textNode.Text; }
	private void typeChanged(int index) { Feature.Type = typeNode.GetItemText(index); }
}
