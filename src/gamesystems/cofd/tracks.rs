use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_context,
	use_memo, use_signal, Element, GlobalSignal, Readable, Signal, Writable};
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use crate::data::{AppState, CharacterSheet};
use crate::gamesystems::CofdSheet;

#[component]
pub fn TracksElement() -> Element
{
	let mut sheetData = use_context::<Signal<CofdSheet>>();
	
	let health = use_signal(|| sheetData().health);
	let integrity: Signal<StateValue> = use_signal(|| sheetData().integrity.into());
	let willpower: Signal<StateValue> = use_signal(|| sheetData().willpower.into());
	
	use_memo(move || {
		let mut sheetDataWrite = sheetData.write();
		
		sheetDataWrite.health = health();
		sheetDataWrite.integrity = integrity().one;
		sheetDataWrite.willpower = willpower().one;
		
		if let Some(sheet) = AppState.write().sheets.cofd.iter_mut()
			.find(|d| d.id == sheetDataWrite.id)
		{
			sheet.update(&sheetDataWrite);
		}
	});
	
	return rsx!(
		rect
		{
			cross_align: "center",
			direction: "vertical",
			margin: "0 0 5 0",
			spacing: "5",
			
			rect
			{
				direction: "horizontal",
				main_align: "space-evenly",
				width: "fill",
				
				rect
				{
					direction: "vertical",
					cross_align: "center",
					spacing: "5",
					width: "20%",
					
					label { text_align: "center", "Health" }
					StatefulTrack
					{
						max: sheetData().calculateMaxHealth(),
						mode: StatefulMode::BoxThree,
						value: health
					}
				}
				
				rect
				{
					direction: "vertical",
					cross_align: "center",
					spacing: "5",
					width: "20%",
					
					label { text_align: "center", "Willpower" }
					StatefulTrack
					{
						max: sheetData().calculateMaxWillpower(),
						mode: StatefulMode::BoxOne,
						value: willpower
					}
				}
				
				rect
				{
					direction: "vertical",
					cross_align: "center",
					spacing: "5",
					width: "20%",
					
					label { text_align: "center", "Integrity" }
					StatefulTrack
					{
						max: 10,
						mode: StatefulMode::CircleOne,
						value: integrity
					}
				}
			}
		}
	);
}
