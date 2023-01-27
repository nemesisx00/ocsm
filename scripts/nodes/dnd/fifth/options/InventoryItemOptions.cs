using Godot;
using System;
using OCSM.Nodes.Autoload;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth
{
	public class InventoryItemOptions : OptionButton
	{
		private MetadataManager metadataManager;
		
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
		
		private void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var index = Selected;
				
				Clear();
				AddItem(String.Empty);
				dfc.AllItems.ForEach(i => AddItem(i.Name));
				
				Selected = index;
			}
		}
	}
}
