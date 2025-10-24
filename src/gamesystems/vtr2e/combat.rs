use freya::prelude::{component, dioxus_elements, fc_to_builder, rsx,
	use_context, use_memo, use_signal, Element, GlobalSignal, Readable, Signal,
	Writable};
use crate::components::NumericInput;
use crate::constants::{BorderColor, CornerRadius};
use super::data::Vtr2eSheet;

#[component]
pub fn CombatElement() -> Element
{
	let mut sheetData = use_context::<Signal<Vtr2eSheet>>();
	
	let size = use_signal(|| sheetData().size);
	let armor = use_signal(|| sheetData().armor);
	
	use_memo(move || {
		sheetData.write().armor = armor();
		sheetData.write().size = size();
	});
	
	let defense = sheetData().calculateDefense();
	let initiative = sheetData().calculateInitiative();
	let speed = sheetData().calculateSpeed();
	
	return rsx!(
		rect
		{
			cross_align: "center",
			direction: "vertical",
			margin: "0 0 5 0",
			spacing: "5",
			
			rect
			{
				cross_align: "center",
				content: "flex",
				direction: "horizontal",
				main_align: "space-evenly",
				width: "fill",
				
				rect
				{
					cross_align: "center",
					direction: "horizontal",
					spacing: "5",
					
					label { "Armor" }
					
					NumericInput
					{
						value: armor,
						width: "50",
					}
				}
				
				rect
				{
					cross_align: "center",
					direction: "horizontal",
					spacing: "5",
					
					label { "Defense" }
					
					rect
					{
						border: "1 center {BorderColor}",
						content: "flex",
						corner_radius: "{CornerRadius}",
						padding: "4 5",
						width: "40",
						
						label
						{
							text_align: "center",
							width: "flex",
							
							"{defense}"
						}
					}
				}
				
				rect
				{
					cross_align: "center",
					direction: "horizontal",
					spacing: "5",
					
					label { "Initiative" }
					
					rect
					{
						border: "1 center {BorderColor}",
						content: "flex",
						corner_radius: "{CornerRadius}",
						padding: "4 5",
						width: "40",
						
						label
						{
							text_align: "center",
							width: "flex",
							
							"{initiative}"
						}
					}
				}
				
				rect
				{
					cross_align: "center",
					direction: "horizontal",
					spacing: "5",
					
					label { "Speed" }
					
					rect
					{
						border: "1 center {BorderColor}",
						content: "flex",
						corner_radius: "{CornerRadius}",
						padding: "4 5",
						width: "40",
						
						label
						{
							text_align: "center",
							width: "flex",
							
							"{speed}"
						}
					}
				}
				
				rect
				{
					cross_align: "center",
					direction: "horizontal",
					spacing: "5",
					
					label { "Size" }
					
					NumericInput
					{
						value: size,
						max: 100,
						width: "50",
					}
				}
			}
		}
	);
}
