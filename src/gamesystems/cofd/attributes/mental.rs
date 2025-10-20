use dioxus::hooks::{use_memo, use_signal};
use freya::prelude::{component, dioxus_elements, rsx, fc_to_builder,
	Element, GlobalSignal, Props, Readable, Signal, Writable};
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use super::super::LabelMinWidth;
use super::super::data::AttributesMental;

#[component]
pub fn AttributesMentalElement(signal: Signal<AttributesMental>, traitMax: Option<u32>) -> Element
{
	let traitMax = match traitMax
	{
		None => 5,
		Some(tm) => tm,
	};
	
	let intelligence = use_signal(|| StateValue { one: signal().intelligence, ..Default::default() });
	let resolve = use_signal(|| StateValue { one: signal().resolve, ..Default::default() });
	let wits = use_signal(|| StateValue { one: signal().wits, ..Default::default() });
	
	use_memo(move || {
		let attr = AttributesMental
		{
			intelligence: intelligence().one,
			resolve: resolve().one,
			wits: wits().one,
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
				
				label { min_width: "{LabelMinWidth}", "Intelligence" }
				
				StatefulTrack
				{
					max: traitMax,
					min: 1,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: intelligence
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Wits" }
				
				StatefulTrack
				{
					
					max: traitMax,
					min: 1,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: wits
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Resolve" }
				
				StatefulTrack
				{
					
					max: traitMax,
					min: 1,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: resolve
				}
			}
		}
	);
}
