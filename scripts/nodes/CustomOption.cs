using Godot;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes
{
	public abstract class CustomOption : OptionButton
	{
		[Signal]
		public delegate void ItemsChanged();
		
		protected MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			metadataManager.Connect(nameof(MetadataManager.MetadataSaved), this, nameof(refreshMetadata));
			metadataManager.Connect(nameof(MetadataManager.MetadataLoaded), this, nameof(refreshMetadata));
			
			refreshMetadata();
		}
		
		public void select(string text)
		{
			for(var i = 0; i < GetItemCount(); i++)
			{
				if(GetItemText(i).Equals(text))
				{
					Selected = i;
					break;
				}
			}
		}
		
		protected virtual void refreshMetadata() { }
	}
}
