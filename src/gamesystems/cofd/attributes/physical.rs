use dioxus::hooks::{use_memo, use_signal};
use freya::prelude::{component, dioxus_elements, rsx, fc_to_builder,
	Element, GlobalSignal, Props, Readable, Signal, Writable};
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use super::super::LabelMinWidth;
use super::super::data::AttributesPhysical;

#[component]
pub fn AttributesPhysicalElement(signal: Signal<AttributesPhysical>, traitMax: Option<u32>) -> Element
{
	let traitMax = match traitMax
	{
		None => 5,
		Some(tm) => tm,
	};
	
	let dexterity = use_signal(|| StateValue { one: signal().dexterity, ..Default::default() });
	let stamina = use_signal(|| StateValue { one: signal().stamina, ..Default::default() });
	let strength = use_signal(|| StateValue { one: signal().strength, ..Default::default() });
	
	use_memo(move || {
		let attr = AttributesPhysical
		{
			dexterity: dexterity().one,
			stamina: stamina().one,
			strength: strength().one,
		};
		
		*signal.write() = attr;
	});
	
	return rsx!(
		rect
		{
			direction: "vertical",
			main_align: "start",
			spacing: "4",
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Strength" }
				
				StatefulTrack
				{
					max: traitMax,
					min: 1,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: strength
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Dexterity" }
				
				StatefulTrack
				{
					max: traitMax,
					min: 1,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: dexterity
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Stamina" }
				
				StatefulTrack
				{
					
					max: traitMax,
					min: 1,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: stamina
				}
			}
		}
	);
}
