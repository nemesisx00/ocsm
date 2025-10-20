use freya::prelude::{component, dioxus_elements, rsx, fc_to_builder, use_memo,
	use_signal, Element, GlobalSignal, Props, Readable, Signal, Writable};
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use super::super::LabelMinWidth;
use super::super::data::SkillsMental;

#[component]
pub fn SkillsMentalElement(signal: Signal<SkillsMental>, traitMax: Option<u32>) -> Element
{
	let traitMax = match traitMax
	{
		None => 5,
		Some(tm) => tm,
	};
	
	let academics: Signal<StateValue> = use_signal(|| signal().academics.into());
	let computer: Signal<StateValue> = use_signal(|| signal().computer.into());
	let crafts: Signal<StateValue> = use_signal(|| signal().crafts.into());
	let investigation: Signal<StateValue> = use_signal(|| signal().investigation.into());
	let medicine: Signal<StateValue> = use_signal(|| signal().medicine.into());
	let occult: Signal<StateValue> = use_signal(|| signal().occult.into());
	let politics: Signal<StateValue> = use_signal(|| signal().politics.into());
	let science: Signal<StateValue> = use_signal(|| signal().science.into());
	
	use_memo(move || {
		let attr = SkillsMental
		{
			academics: academics().one,
			computer: computer().one,
			crafts: crafts().one,
			investigation: investigation().one,
			medicine: medicine().one,
			occult: occult().one,
			politics: politics().one,
			science: science().one,
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
				
				label { min_width: "{LabelMinWidth}", "Academics" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: academics
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Computer" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: computer
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Crafts" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: crafts
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Investigation" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: investigation
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Medicine" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: medicine
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Occult" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: occult
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Politics" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: politics
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Science" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: science
				}
			}
		}
	);
}
