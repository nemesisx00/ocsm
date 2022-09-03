using Godot;
using System.Collections.Generic;
using OCSM.Nodes.Meta;

namespace OCSM.DnD.Fifth.Meta
{
	public class FeaturefulMetadataEntry : BasicMetadataEntry
	{
		protected const string FeaturesName = "Features";
		
		[Signal]
		public new delegate void SaveClicked(string name, string description, List<Transport<Feature>> features);
		
		protected List<Feature> Features;
		
		public override void _Ready()
		{
			Features = new List<Feature>();
			
			base._Ready();
		}
		
		protected override void clearInputs()
		{
			base.clearInputs();
			
			Features.Clear();
		}
		
		protected override void doSave()
		{
			var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
			var description = GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text;
			
			var transports = new List<Transport<Feature>>();
			foreach(var feature in Features)
			{
				transports.Add(new Transport<Feature>(feature));
			}
			
			EmitSignal(nameof(SaveClicked), name, description, transports);
			clearInputs();
		}
		
		public virtual void loadEntry(Featureful entry)
		{
			base.loadEntry(entry);
			
			Features = entry.Features;
		}
	}
}
