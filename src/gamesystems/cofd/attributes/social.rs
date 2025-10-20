use dioxus::hooks::{use_memo, use_signal};
use freya::prelude::{component, dioxus_elements, rsx, fc_to_builder,
	Element, GlobalSignal, Props, Readable, Signal, Writable};
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use super::super::LabelMinWidth;
use super::super::data::AttributesSocial;

#[component]
pub fn AttributesSocialElement(signal: Signal<AttributesSocial>, traitMax: Option<u32>) -> Element
{
	let traitMax = match traitMax
	{
		None => 5,
		Some(tm) => tm,
	};
	
	let composure = use_signal(|| StateValue { one: signal().composure, ..Default::default() });
	let manipulation = use_signal(|| StateValue { one: signal().manipulation, ..Default::default() });
	let presence = use_signal(|| StateValue { one: signal().presence, ..Default::default() });
	
	use_memo(move || {
		let attr = AttributesSocial
		{
			composure: composure().one,
			manipulation: manipulation().one,
			presence: presence().one,
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
				
				label { min_width: "{LabelMinWidth}", "Presence" }
				
				StatefulTrack
				{
					
					max: traitMax,
					min: 1,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: presence
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Manipulation" }
				
				StatefulTrack
				{
					max: traitMax,
					min: 1,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: manipulation
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Composure" }
				
				StatefulTrack
				{
					max: traitMax,
					min: 1,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: composure
				}
			}
		}
	);
}
