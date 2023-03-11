using Godot;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes
{
	public abstract partial class CustomOption : OptionButton
	{
		[Signal]
		public delegate void ItemsChangedEventHandler();
		
		protected MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			metadataManager.MetadataLoaded += refreshMetadata;
			metadataManager.MetadataSaved += refreshMetadata;
			
			refreshMetadata();
		}
		
		public void select(string text)
		{
			for(var i = 0; i < ItemCount; i++)
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
