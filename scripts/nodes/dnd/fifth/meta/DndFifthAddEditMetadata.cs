using Godot;
using System.Collections.Generic;
using OCSM.Nodes.Autoload;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public class DndFifthAddEditMetadata : WindowDialog
	{
		private const string BackgroundsName = "Backgrounds";
		private const string FeaturesName = "FeatureEntry";
		
		[Signal]
		public delegate void MetadataChanged();
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			GetCloseButton().Connect(Constants.Signal.Pressed, this, nameof(closeHandler));
			
			var backgroundEntry = GetNode<BackgroundEntry>(NodePathBuilder.SceneUnique(BackgroundsName));
			backgroundEntry.Connect(nameof(BackgroundEntry.SaveClicked), this, nameof(saveBackground));
			backgroundEntry.Connect(nameof(BackgroundEntry.DeleteConfirmed), this, nameof(deleteBackground));
			
			var featureEntry = GetNode<FeatureEntry>(NodePathBuilder.SceneUnique(FeaturesName));
			featureEntry.Connect(nameof(FeatureEntry.SaveClicked), this, nameof(saveFeature));
			featureEntry.Connect(nameof(FeatureEntry.DeleteConfirmed), this, nameof(deleteFeature));
		}
		
		private void closeHandler()
		{
			QueueFree();
		}
		
		protected void deleteBackground(string name)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
				{
					dfc.Backgrounds.Remove(background);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		protected void deleteFeature(string name)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Features.Find(f => f.Name.Equals(name)) is OCSM.DnD.Fifth.Feature feature)
				{
					dfc.Features.Remove(feature);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		protected void saveBackground(string name, string description, List<Transport<OCSM.DnD.Fifth.Feature>> transports)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
					dfc.Backgrounds.Remove(background);
				
				var features = new List<OCSM.DnD.Fifth.Feature>();
				foreach(var transport in transports)
				{
					features.Add(transport.Value);
				}
				
				dfc.Backgrounds.Add(new Background(name, description, features));
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		protected void saveFeature(Transport<OCSM.DnD.Fifth.Feature> transport)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Features.Find(f => f.Name.Equals(transport.Value.Name)) is OCSM.DnD.Fifth.Feature feature)
					dfc.Features.Remove(feature);
				
				dfc.Features.Add(transport.Value);
				EmitSignal(nameof(MetadataChanged));
			}
		}
	}
}
