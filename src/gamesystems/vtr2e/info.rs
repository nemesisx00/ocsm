use freya::hooks::{cow_borrowed, theme_with};
use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_context,
	Accordion, AccordionBody, AccordionSummary, Element, GlobalSignal, Input,
	IntoDynNode, InputThemeWith, Readable, Signal, Writable};
use crate::gamesystems::Vtr2eSheet;

#[component]
pub fn InfoAccordionElement() -> Element
{
	let sheetData = use_context::<Signal<Vtr2eSheet>>();
	
	let summary = match sheetData().name.is_empty()
	{
		false => match sheetData().clan.is_empty()
		{
			false => match sheetData().bloodline.is_empty()
			{
				false => format!("{} ({} {})", sheetData().name, sheetData().clan, sheetData().bloodline),
				true => format!("{} ({})", sheetData().name, sheetData().clan),
			},
			true => sheetData().name,
		},
		true => "Character Name".into(),
	};
	
	return rsx!(
		rect
		{
			cross_align: "center",
			direction: "vertical",
			width: "fill",
			
			rect
			{
				cross_align: "center",
				direction: "vertical",
				width: "85%",
				
				Accordion
				{
					summary: rsx!(AccordionSummary
					{
						rect
						{
							direction: "vertical",
							spacing: "5",
							width: "fill",
							
							label
							{
								text_align: "center",
								width: "fill",
								
								"{summary}"
							}
							
							if !sheetData().concept.is_empty()
							{
								label
								{
									font_size: "10",
									text_align: "center",
									width: "fill",
									
									"{sheetData().concept}"
								}
							}
						}
					}),
					
					AccordionBody
					{
						InfoElement {}
					}
				}
			}
		}
	);
}

/**
Component enabling display and editing of general free text character information
typically displayed at the top of a character sheet, such as character concept,
name, etc.
*/
#[component]
pub fn InfoElement() -> Element
{
	let labelMinWidth = 80;
	
	let mut sheetData = use_context::<Signal<Vtr2eSheet>>();
	
	return rsx!(
		rect
		{
			direction: "vertical",
			spacing: "5",
			width: "fill",
			
			rect
			{
				content: "flex",
				cross_align: "center",
				direction: "horizontal",
				main_align: "center",
				spacing: "5",
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
					value: sheetData().concept,
					width: "flex",
				}
			}
			
			rect
			{
				content: "flex",
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
					width: "flex",
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
					width: "flex",
				}
			}
			
			rect
			{
				content: "flex",
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
					width: "flex",
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
					width: "flex",
				}
			}
			
			rect
			{
				content: "flex",
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
					width: "flex",
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
					value: sheetData().bloodline,
					width: "flex",
				}
			}
			
			rect
			{
				content: "flex",
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
					width: "flex",
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
					value: sheetData().covenant,
					width: "flex",
				}
			}
		}
	);
}
