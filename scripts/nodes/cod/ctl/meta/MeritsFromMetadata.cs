using Godot;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL.Meta
{
	public class MeritsFromMetadata : Container
	{
		[Signal]
		public delegate void AddMerit(string name);
		private const string MeritsName = "ExistingMerits";
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			metadataManager.Connect(nameof(MetadataManager.MetadataLoaded), this, nameof(refreshMerits));
			metadataManager.Connect(nameof(MetadataManager.MetadataSaved), this, nameof(refreshMerits));
			
			GetNode<OptionButton>(NodePathBuilder.SceneUnique(MeritsName)).Connect(Constants.Signal.ItemSelected, this, nameof(meritSelected));
			
			refreshMerits();
		}
		
		private void refreshMerits()
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				var optionButton = GetNode<OptionButton>(NodePathBuilder.SceneUnique(MeritsName));
				optionButton.Clear();
				optionButton.AddItem("");
				foreach(var m in ccc.Merits)
				{
					optionButton.AddItem(m.Name);
				}
			}
		}
		
		private void meritSelected(int index)
		{
			if(index > 0)
			{
				var node = GetNode<OptionButton>(NodePathBuilder.SceneUnique(MeritsName));
				EmitSignal(nameof(AddMerit), node.GetItemText(index));
				node.Selected = 0;
			}
		}
	}
}
