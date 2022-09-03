using Godot;
using System.Collections.Generic;
using OCSM.Nodes.Autoload;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public class AddEditMetadata : WindowDialog
	{
		private const string BackgroundsName = "Backgrounds";
		
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
		
		protected void saveBackground(string name, string description, List<Transport<Feature>> transports)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
					dfc.Backgrounds.Remove(background);
				
				var features = new List<Feature>();
				foreach(var transport in transports)
				{
					features.Add(transport.Value);
				}
				
				dfc.Backgrounds.Add(new Background(name, description, features));
				EmitSignal(nameof(MetadataChanged));
			}
		}
	}
}
