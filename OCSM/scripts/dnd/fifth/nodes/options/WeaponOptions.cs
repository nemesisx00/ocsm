using System.Linq;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class WeaponOptions : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			var index = Selected;
			
			Clear();
			AddItem(string.Empty);
			container.Items
				.Where(i => i.WeaponData is not null)
				.ToList()
				.ForEach(i => AddItem(i.Name));
			
			Selected = index;
			
			EmitSignal(CustomOption.SignalName.ItemsChanged);
		}
	}
}
