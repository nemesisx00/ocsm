use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx, use_context,
	use_memo, use_signal, Element, GlobalSignal, Readable, Signal, Writable};
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use crate::gamesystems::Vtr2eSheet;

#[component]
pub fn TracksElement() -> Element
{
	let mut sheetData = use_context::<Signal<Vtr2eSheet>>();
	
	let bloodPotency: Signal<StateValue> = use_signal(|| sheetData().bloodPotency.into());
	let health = use_signal(|| sheetData().health);
	let humanity: Signal<StateValue> = use_signal(|| sheetData().humanity.into());
	let vitae: Signal<StateValue> = use_signal(|| sheetData().vitae.into());
	let willpower: Signal<StateValue> = use_signal(|| sheetData().willpower.into());
	
	use_memo(move || {
		let mut sheetDataWrite = sheetData.write();
		
		sheetDataWrite.bloodPotency = bloodPotency().one;
		sheetDataWrite.health = health();
		sheetDataWrite.humanity = humanity().one;
		sheetDataWrite.vitae = vitae().one;
		sheetDataWrite.willpower = willpower().one;
	});
	
	let maxHealth = sheetData().calculateMaxHealth();
	let maxVitae = sheetData().calculateMaxVitae();
	let maxVitaePerTurn = sheetData().calculateMaxVitaePerTurn();
	let maxWillpower = sheetData().calculateMaxWillpower();
	
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
					
					label { text_align: "center", "Humanity" }
					StatefulTrack
					{
						max: 10,
						mode: StatefulMode::CircleOne,
						value: humanity
					}
				}
				
				rect
				{
					direction: "vertical",
					cross_align: "center",
					spacing: "5",
					width: "20%",
					
					label { text_align: "center", "Health" }
					StatefulTrack
					{
						max: maxHealth,
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
						max: maxWillpower,
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
					
					label { text_align: "center", "Blood Potency" }
					StatefulTrack
					{
						max: 10,
						mode: StatefulMode::CircleOne,
						value: bloodPotency
					}
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "vertical",
				width: "fill",
				
				label { main_align: "center", margin: "5 0 0 0", text_align: "center", "Vitae" }
				label { font_size: "11", main_align: "center", text_align: "center", "{maxVitaePerTurn} per turn" }
			}
			
			rect
			{
				direction: "horizontal",
				main_align: "center",
				
				StatefulTrack
				{
					max: maxVitae,
					mode: StatefulMode::BoxOne,
					rowMax: 40,
					value: vitae
				}
			}
		}
	);
}
