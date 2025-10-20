use freya::prelude::{component, dioxus_elements, rsx, fc_to_builder, use_memo,
	use_signal, Element, GlobalSignal, Props, Readable, Signal, Writable};
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use super::super::LabelMinWidth;
use super::super::data::SkillsPhysical;

#[component]
pub fn SkillsPhysicalElement(signal: Signal<SkillsPhysical>, traitMax: Option<u32>) -> Element
{
	let traitMax = match traitMax
	{
		None => 5,
		Some(tm) => tm,
	};
	
	let athletics: Signal<StateValue> = use_signal(|| signal().athletics.into());
	let brawl: Signal<StateValue> = use_signal(|| signal().brawl.into());
	let drive: Signal<StateValue> = use_signal(|| signal().drive.into());
	let firearms: Signal<StateValue> = use_signal(|| signal().firearms.into());
	let larceny: Signal<StateValue> = use_signal(|| signal().larceny.into());
	let stealth: Signal<StateValue> = use_signal(|| signal().stealth.into());
	let survival: Signal<StateValue> = use_signal(|| signal().survival.into());
	let weaponry: Signal<StateValue> = use_signal(|| signal().weaponry.into());
	
	use_memo(move || {
		let attr = SkillsPhysical
		{
			athletics: athletics().one,
			brawl: brawl().one,
			drive: drive().one,
			firearms: firearms().one,
			larceny: larceny().one,
			stealth: stealth().one,
			survival: survival().one,
			weaponry: weaponry().one,
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
				
				label { min_width: "{LabelMinWidth}", "Athletics" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: athletics
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Brawl" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: brawl
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Drive" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: drive
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Firearms" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: firearms
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Larceny" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: larceny
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Stealth" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: stealth
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Survival" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: survival
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Weaponry" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: weaponry
				}
			}
		}
	);
}
