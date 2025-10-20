use freya::hooks::{cow_borrowed, theme_with};
use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_context,
	use_memo, use_signal, Element, GlobalSignal, Input, InputThemeWith, Readable,
	Signal, Writable};
use crate::data::{AppState, CharacterSheet};
use crate::gamesystems::CofdSheet;

/**
Component enabling display and editing of general free text character information
typically displayed at the top of a character sheet, such as character concept,
name, etc.
*/
#[component]
pub fn InfoElement() -> Element
{
	let mut sheetData = use_context::<Signal<CofdSheet>>();
	
	let mut age = use_signal(|| sheetData().age);
	let mut chronicle = use_signal(|| sheetData().chronicle);
	let mut concept = use_signal(|| sheetData().concept);
	let mut faction = use_signal(|| sheetData().faction);
	let mut group = use_signal(|| sheetData().group);
	let mut name = use_signal(|| sheetData().name);
	let mut player = use_signal(|| sheetData().player);
	let mut size = use_signal(|| sheetData().size);
	let mut vice = use_signal(|| sheetData().vice);
	let mut virtue = use_signal(|| sheetData().virtue);
	
	use_memo(move || {
		let mut sheetDataWrite = sheetData.write();
		
		sheetDataWrite.age = age();
		sheetDataWrite.chronicle = chronicle();
		sheetDataWrite.concept = concept();
		sheetDataWrite.faction = faction();
		sheetDataWrite.group = group();
		sheetDataWrite.name = name();
		sheetDataWrite.player = player();
		sheetDataWrite.size = size();
		sheetDataWrite.vice = vice();
		sheetDataWrite.name = virtue();
		
		if let Some(sheet) = AppState.write().sheets.cofd.iter_mut()
			.find(|d| d.id == sheetDataWrite.id)
		{
			sheet.update(&sheetDataWrite);
		}
	});
	
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
					"Player"
				}
				
				Input
				{
					onchange: move |value| player.set(value),
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: player,
					width: "35%",
				}
				
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Concept"
				}
				
				Input
				{
					onchange: move |value| concept.set(value),
					value: concept,
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
					onchange: move |value| name.set(value),
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: name,
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
					onchange: move |value| chronicle.set(value),
					value: chronicle,
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
					"Virtue"
				}
				
				Input
				{
					onchange: move |value| virtue.set(value),
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: virtue,
					width: "35%",
				}
			
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Vice"
				}
				
				Input
				{
					onchange: move |value| vice.set(value),
					value: vice,
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
					"Group"
				}
				
				Input
				{
					onchange: move |value| group.set(value),
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: group,
					width: "35%",
				}
				
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Faction"
				}
				
				Input
				{
					onchange: move |value| faction.set(value),
					value: faction,
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
					"Age"
				}
				
				Input
				{
					onchange: move |value: String| if let Ok(ageNum) = value.parse::<u32>()
					{
						age.set(ageNum);
					},
					theme: theme_with!(InputTheme
					{
						margin: cow_borrowed!("0 15 0 0"),
					}),
					value: age.to_string(),
					width: "35%",
				}
				
				label
				{
					min_width: "{labelMinWidth}",
					width: "5%",
					"Size"
				}
				
				Input
				{
					onchange: move |value: String| if let Ok(sizeNum) = value.parse::<u32>()
					{
						size.set(sizeNum);
					},
					value: size.to_string(),
					width: "35%",
				}
			}
		}
	);
}
