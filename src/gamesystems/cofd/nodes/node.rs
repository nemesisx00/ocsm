use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, Button, Element, GlobalSignal, Props, Readable};
use crate::constants::SheetListNodeHeight;
use crate::data::AppState;
use crate::gamesystems::CofdSheet;

#[component]
pub fn SheetListNode(sheet: CofdSheet) -> Element
{
	let name = match sheet.name.is_empty()
	{
		false => sheet.name,
		true => format!("Mortal {}", sheet.id.index),
	};
	
	return rsx!(
		Button
		{
			onpress: move |_| AppState.write().activeId = Some(sheet.id),
			
			rect
			{
				cross_align: "center",
				direction: "vertical",
				height: "{SheetListNodeHeight}",
				main_align: "center",
				spacing: "5",
				width: "200",
				
				rect
				{
					cross_align: "center",
					direction: "horizontal",
					
					label { "{name}" }
				}
			}
		}
	);
}
