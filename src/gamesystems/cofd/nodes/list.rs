use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, Element,
	GlobalSignal, IntoDynNode, Readable};
use crate::data::AppState;
use super::node::SheetListNode;

#[component]
pub fn SheetList() -> Element
{
	let sheets = AppState().sheets.cofd;
	
	return rsx!(
		if sheets.len() > 0
		{
			label { "Chronicles of Darkness Core" }
			
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
