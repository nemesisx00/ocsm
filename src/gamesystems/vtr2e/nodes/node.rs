use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, Button, Element, GlobalSignal, IntoDynNode, Props, Readable};
use crate::constants::SheetListNodeHeight;
use crate::data::AppState;
use crate::gamesystems::Vtr2eSheet;

#[component]
pub fn SheetListNode(sheet: Vtr2eSheet) -> Element
{
	let name = match sheet.name.is_empty()
	{
		false => sheet.name,
		true => format!("Vampire {}", sheet.id.index),
	};
	
	let clan = match sheet.bloodline.is_empty()
	{
		false => match sheet.clan.is_empty()
		{
			false => format!("{} {}", sheet.clan, sheet.bloodline),
			true => sheet.bloodline,
		},
		true => match sheet.clan.is_empty()
		{
			false => sheet.clan,
			true => String::default(),
		},
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
					spacing: "2",
					
					label { "{name}" }
				}
				
				if !clan.is_empty()
				{
					rect
					{
						cross_align: "center",
						direction: "horizontal",
						spacing: "5",
						
						label { font_size: "10", "{clan}" }
					}
				}
			}
		}
	);
}
