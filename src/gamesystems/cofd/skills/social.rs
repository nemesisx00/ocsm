use freya::prelude::{component, dioxus_elements, rsx, fc_to_builder, use_memo,
	use_signal, Element, GlobalSignal, Props, Readable, Signal, Writable};
use crate::components::{StateValue, StatefulMode, StatefulTrack};
use super::super::LabelMinWidth;
use super::super::data::SkillsSocial;

#[component]
pub fn SkillsSocialElement(signal: Signal<SkillsSocial>, traitMax: Option<u32>) -> Element
{
	let traitMax = match traitMax
	{
		None => 5,
		Some(tm) => tm,
	};
	
	let animalKen: Signal<StateValue> = use_signal(|| signal().animalKen.into());
	let empathy: Signal<StateValue> = use_signal(|| signal().empathy.into());
	let expression: Signal<StateValue> = use_signal(|| signal().expression.into());
	let intimidation: Signal<StateValue> = use_signal(|| signal().intimidation.into());
	let persuasion: Signal<StateValue> = use_signal(|| signal().persuasion.into());
	let socialize: Signal<StateValue> = use_signal(|| signal().socialize.into());
	let streetwise: Signal<StateValue> = use_signal(|| signal().streetwise.into());
	let subterfuge: Signal<StateValue> = use_signal(|| signal().subterfuge.into());
	
	use_memo(move || {
		let attr = SkillsSocial
		{
			animalKen: animalKen().one,
			empathy: empathy().one,
			expression: expression().one,
			intimidation: intimidation().one,
			persuasion: persuasion().one,
			socialize: socialize().one,
			streetwise: streetwise().one,
			subterfuge: subterfuge().one,
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
				
				label { min_width: "{LabelMinWidth}", "Animal Ken" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: animalKen
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Empathy" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: empathy
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Expression" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: expression
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Intimidation" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: intimidation
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Persuasion" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: persuasion
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Socialize" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: socialize
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Streetwise" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: streetwise
				}
			}
			
			rect
			{
				cross_align: "center",
				direction: "horizontal",
				main_align: "space-between",
				spacing: "10",
				
				label { min_width: "{LabelMinWidth}", "Subterfuge" }
				
				StatefulTrack
				{
					max: traitMax,
					mode: StatefulMode::CircleOne,
					rowMax: 5,
					value: subterfuge
				}
			}
		}
	);
}
