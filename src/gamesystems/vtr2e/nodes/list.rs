use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, Element,
	GlobalSignal, IntoDynNode, Readable};
use crate::data::AppState;
use super::node::SheetListNode;

#[component]
pub fn SheetList() -> Element
{
	let sheets = AppState().sheets.vtr2e;
	
	return rsx!(
		if sheets.len() > 0
		{
			label { "Vampire: The Requiem 2nd Edition" }
			
			rect
			{
				direction: "horizontal",
				spacing: "15",
				width: "fill",
				
				for sheet in sheets.iter().cloned()
				{
					SheetListNode { sheet }
				}
			}
		}
	);
}
