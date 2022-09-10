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
		private const string ClassesName = "Classes";
		private const string FeaturesName = "FeatureEntry";
		private const string RacesName = "Races";
		
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
			
			var classEntry = GetNode<ClassEntry>(NodePathBuilder.SceneUnique(ClassesName));
			classEntry.Connect(nameof(ClassEntry.SaveClicked), this, nameof(saveClass));
			classEntry.Connect(nameof(ClassEntry.DeleteConfirmed), this, nameof(deleteClass));
			
			var featureEntry = GetNode<FeatureEntry>(NodePathBuilder.SceneUnique(FeaturesName));
			featureEntry.Connect(nameof(FeatureEntry.SaveClicked), this, nameof(saveFeature));
			featureEntry.Connect(nameof(FeatureEntry.DeleteConfirmed), this, nameof(deleteFeature));
			
			var raceEntry = GetNode<RaceEntry>(NodePathBuilder.SceneUnique(RacesName));
			raceEntry.Connect(nameof(RaceEntry.SaveClicked), this, nameof(saveRace));
			raceEntry.Connect(nameof(RaceEntry.DeleteConfirmed), this, nameof(deleteRace));
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
		
		protected void deleteClass(string name)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
				{
					dfc.Classes.Remove(clazz);
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
		
		protected void deleteRace(string name)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Races.Find(r => r.Name.Equals(name)) is Race race)
				{
					dfc.Races.Remove(race);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		protected void saveBackground(string name, string description, List<Transport<OCSM.DnD.Fifth.FeatureSection>> sections, List<Transport<OCSM.DnD.Fifth.Feature>> features)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
					dfc.Backgrounds.Remove(background);
				
				var sectionList = new List<OCSM.DnD.Fifth.FeatureSection>();
				foreach(var transport in sections)
				{
					sectionList.Add(transport.Value);
				}
				
				var featureList = new List<OCSM.DnD.Fifth.Feature>();
				foreach(var transport in features)
				{
					featureList.Add(transport.Value);
				}
				
				dfc.Backgrounds.Add(new Background(name, description, sectionList, featureList));
				dfc.Backgrounds.Sort();
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		protected void saveClass(string name, string description, List<Transport<OCSM.DnD.Fifth.FeatureSection>> sections, List<Transport<OCSM.DnD.Fifth.Feature>> features)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
					dfc.Classes.Remove(clazz);
				
				var sectionList = new List<OCSM.DnD.Fifth.FeatureSection>();
				foreach(var transport in sections)
				{
					sectionList.Add(transport.Value);
				}
				
				var featureList = new List<OCSM.DnD.Fifth.Feature>();
				foreach(var transport in features)
				{
					featureList.Add(transport.Value);
				}
				
				dfc.Classes.Add(new Class(OCSM.DnD.Fifth.Die.d10, name, description, sectionList, featureList));
				dfc.Classes.Sort();
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
				dfc.Features.Sort();
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		protected void saveRace(string name, string description, List<Transport<OCSM.DnD.Fifth.FeatureSection>> sections, List<Transport<OCSM.DnD.Fifth.Feature>> features)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Races.Find(r => r.Name.Equals(name)) is Race race)
					dfc.Races.Remove(race);
				
				var sectionList = new List<OCSM.DnD.Fifth.FeatureSection>();
				foreach(var transport in sections)
				{
					sectionList.Add(transport.Value);
				}
				
				var featureList = new List<OCSM.DnD.Fifth.Feature>();
				foreach(var transport in features)
				{
					featureList.Add(transport.Value);
				}
				
				dfc.Races.Add(new Race(name, description, sectionList, featureList));
				dfc.Races.Sort();
				EmitSignal(nameof(MetadataChanged));
			}
		}
	}
}
