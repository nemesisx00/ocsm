use freya::hooks::{cow_borrowed, theme_with};
use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_context,
	Element, GlobalSignal, Input, InputThemeWith, Readable, Signal, Writable};
use crate::gamesystems::Vtr2eSheet;

/**
Component enabling display and editing of general free text character information
typically displayed at the top of a character sheet, such as character concept,
name, etc.
*/
#[component]
pub fn InfoElement() -> Element
{
	let mut sheetData = use_context::<Signal<Vtr2eSheet>>();
	let labelMinWidth = 80;
	
	return rsx!(
		rect
		{
			direction: "vertical",
			spacing: "5",
			width: "fill",
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "center",
				width: "fill",
				
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Concept"
				}
				
				Input
				{
					onchange: move |value| sheetData.write().concept = value,
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: sheetData().concept,
					width: "75%",
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "center",
				spacing: "5",
				width: "fill",
				
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Player"
				}
				
				Input
				{
					onchange: move |value| sheetData.write().player = value,
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: sheetData().player,
					width: "35%",
				}
				
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Chronicle"
				}
				
				Input
				{
					onchange: move |value| sheetData.write().chronicle = value,
					value: sheetData().chronicle,
					width: "35%",
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "center",
				spacing: "5",
				width: "fill",
				
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Name"
				}
				
				Input
				{
					onchange: move |value| sheetData.write().name = value,
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: sheetData().name,
					width: "35%",
				}
				
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Clan"
				}
				
				Input
				{
					onchange: move |value| sheetData.write().clan = value,
					value: sheetData().clan,
					width: "35%",
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "center",
				spacing: "5",
				width: "fill",
			
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Mask"
				}
				
				Input
				{
					onchange: move |value| sheetData.write().mask = value,
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: sheetData().mask,
					width: "35%",
				}
			
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Bloodline"
				}
				
				Input
				{
					onchange: move |value| sheetData.write().bloodline = value,
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: sheetData().bloodline,
					width: "35%",
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "center",
				spacing: "5",
				width: "fill",
				
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Dirge"
				}
				
				Input
				{
					onchange: move |value| sheetData.write().dirge = value,
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: sheetData().dirge,
					width: "35%",
				}
				
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Covenant"
				}
				
				Input
				{
					onchange: move |value| sheetData.write().covenant = value,
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: sheetData().covenant,
					width: "35%",
				}
			}
		}
	);
}
